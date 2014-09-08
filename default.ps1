$framework = '4.0x86'

properties {
    $base_dir = resolve-path .
    $tools_dir = "$base_dir\tools"
    $source_dir = "$base_dir\src"
    $build_dir = "$base_dir\build"
    $result_dir = "$build_dir\results"
    $dist_dir = "$base_dir\release"
    $config = "Debug"
    $nunitPath = "$tools_dir\NUnit-2.6.3"
}

task default -depends local
task local -depends compile, test
task full -depends local, dist
task ci -depends clean, release, commonAssemblyInfo, local, dist

task clean {
    delete_directory "$build_dir"
    delete_directory "$dist_dir"
}

task release {
    $config = "release"
}

task compile -depends clean { 
    exec { msbuild /t:Clean /t:Build /p:Configuration=$config /p:Platform='Any CPU' $source_dir\SourceIndexingSharp.sln }
}

task commonAssemblyInfo {
    $commit = if ($env:BUILD_VCS_NUMBER -ne $NULL) { $env:BUILD_VCS_NUMBER } else { git log -1 --pretty=format:%H }
    $commit
    create-globalAssemblyInfo "$commit" "$source_dir\GlobalAssemblyInfo.cs"
}

task test {
	create_directory "$result_dir"
    exec { & $nunitPath\nunit-console.exe $source_dir\SourceIndexingSharp.Tests\bin\$config\SourceIndexingSharp.Tests.dll /xml=$result_dir\SourceIndexingSharp.Tests.xml }
    if($enc:APPVEYOR -ne $NULL) {
        "Uploading unit test reports to AppVeyor"
        $wc = New-Object 'System.Net.WebClient'
        $wc.UploadFile("https://ci.appveyor.com/api/testresults/xunit/$($env:APPVEYOR_JOB_ID)", $result_dir\SourceIndexingSharp.Tests.xml)
    }
}

task dist {

}

function global:create_directory($directory_name)
{
    mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:delete_directory($directory_name)
{
    rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:create-globalAssemblyInfo($commit, $filename)
{
 
}