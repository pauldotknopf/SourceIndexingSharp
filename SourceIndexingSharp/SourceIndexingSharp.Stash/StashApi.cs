using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SourceIndexingSharp.Stash
{
    public class StashApi : IStashApi
    {
        public void ExtractSource(string destination, string host, string project, string repository, string file, string commit, StashCredentials credentials)
        {
            if (File.Exists(destination))
                File.Delete(destination);

            var url = new Uri(BuildUrl(host, project, repository, file, commit));

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            if (credentials != null)
                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));

            using (var response = (HttpWebResponse)httpRequest.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new SourceIndexException("There was a problem requesting a source file from stash. " + response.StatusCode);

                using (var responseStream = response.GetResponseStream())
                {
                    GetFileResponse getFileResponse = null;

                    using (var responseStreamReader = new StreamReader(responseStream))
                        getFileResponse = JsonConvert.DeserializeObject<GetFileResponse>(responseStreamReader.ReadToEnd());

                    if(!getFileResponse.IsLastPage)
                        throw new Exception("The GetFileResponse indicates that we don't have all of our lines of code!");

                    using (var fileStream = File.OpenWrite(destination))
                        using (var fileStreamWriter = new StreamWriter(fileStream))
                            foreach (var line in getFileResponse.Lines)
                                fileStreamWriter.WriteLine(line.Text);
                }
            }
        }

        private string BuildUrl(string host, string project, string repository, string file, string commit)
        {
            return string.Format("http://{0}/rest/api/1.0/projects/{1}/repos/{2}/browse/{3}?at={4}", host, project, repository, file, commit);
        }

        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        class GetFileResponse
        {
            [JsonProperty("start")]
            public long Start { get; set; }
            [JsonProperty("size")]
            public long Size { get; set; }
            [JsonProperty("isLastPage")]
            public bool IsLastPage { get; set; }
            [JsonProperty("lines")]
            public List<GetFileResponseLine> Lines { get; set; } 
            
        }

        class GetFileResponseLine
        {
            [JsonProperty("text")]
            public string Text { get; set; }
        }

        // ReSharper restore ClassNeverInstantiated.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
