﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LibGit2Sharp;
using Newtonsoft.Json;

namespace SourceIndexingSharp
{
    public class PdbCommandProcessor : IPdbCommandProcessor
    {
        private readonly IStringExpander _stringExpander;
        private readonly IPathResolver _pathResolver;

        public PdbCommandProcessor(IStringExpander stringExpander, IPathResolver pathResolver)
        {
            _stringExpander = stringExpander;
            _pathResolver = pathResolver;
        }

        public void Process(List<string> pdbFiles, string command, string relativeDirectory)
        {
            if (pdbFiles == null)
                return;

            if (!pdbFiles.Any())
                return;

            foreach (var pdbFile in pdbFiles)
            {
                if (!File.Exists(pdbFile))
                    throw new Exception(string.Format("The pdb file {0} doesn't exist.", pdbFile));
            }

            if (string.IsNullOrEmpty(relativeDirectory))
                relativeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var indexCommand = JsonConvert.DeserializeObject<IndexCommand>(command);

            if (string.IsNullOrEmpty(indexCommand.Provider))
                throw new Exception("You must specify a provider to use to do the indexing");

            switch (indexCommand.Provider)
            {
                case "Stash":
                    var stashCommand = JsonConvert.DeserializeObject<StashCommand>(command);
                    ProcessStash(pdbFiles,
                        _pathResolver.ResolvePath(stashCommand.GitDirectory, relativeDirectory),
                        stashCommand.Host, stashCommand.Project,
                        stashCommand.Repository,
                        stashCommand.Username,
                        stashCommand.Password);
                    break;
                case "Url":
                    var urlCommand = JsonConvert.DeserializeObject<UrlCommand>(command);
                    ProcessUrl(pdbFiles,
                        _pathResolver.ResolvePath(urlCommand.GitDirectory, relativeDirectory),
                        urlCommand.UrlFormat);
                    break;
                default:
                    throw new Exception(string.Format("The provider {0} is not known.", indexCommand.Provider));
            }
        }


        public virtual void ProcessStash(List<string> pdbFiles, string gitDirectory, string host, string project, string repository, string username, string password)
        {
            if (pdbFiles == null)
                return;

            if (!pdbFiles.Any())
                return;

            foreach (var pdbFile in pdbFiles)
            {
                if (!File.Exists(pdbFile))
                    throw new Exception(string.Format("The pdb file {0} doesn't exist.", pdbFile));
            }
        }

        public virtual void ProcessUrl(List<string> pdbFiles, string gitDirectory, string urlFormat)
        {
            if (pdbFiles == null)
                return;

            if (!pdbFiles.Any())
                return;

            foreach (var pdbFile in pdbFiles)
            {
                if (!File.Exists(pdbFile))
                    throw new Exception(string.Format("The pdb file {0} doesn't exist.", pdbFile));
            }
        }

        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        class IndexCommand
        {
            public string Provider { get; set; }
        }

        class StashCommand
        {
            public string GitDirectory { get; set; }

            public string Host { get; set; }

            public string Project { get; set; }

            public string Repository { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }
        }

        class UrlCommand
        {
            public string GitDirectory { get; set; }

            public string UrlFormat { get; set; }
        }

        // ReSharper restore ClassNeverInstantiated.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
