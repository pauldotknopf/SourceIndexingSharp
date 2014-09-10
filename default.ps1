Framework '4.0x86'

properties {
    $base_dir = resolve-path .
    $tools_dir = "$base_dir\tools"
    $source_dir = "$base_dir\src"
    $build_dir = "$base_dir\build"
    $result_dir = "$build_dir\results"
    $dist_dir = "$base_dir\release"
    $global:config = "Debug"
	$buildNumber = if ( $env:APPVEYOR_BUILD_NUMBER  -ne $NULL) { $env:APPVEYOR_BUILD_NUMBER  } else { "0" }
	$version = "1.0.0.$buildNumber"
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
    $global:config = "Release"
}

task compile -depends clean { 
	exec { msbuild /t:Clean /t:Build /p:Configuration=$config /p:Platform='Any CPU' /p:EmbedExtractor=true $source_dir\SourceIndexingSharp.sln }
}

task commonAssemblyInfo {
    $commit = if ($env:BUILD_VCS_NUMBER -ne $NULL) { $env:BUILD_VCS_NUMBER } else { git log -1 --pretty=format:%H }
    create-globalAssemblyInfo "$commit" "$source_dir\GlobalAssemblyInfo.cs"
}

task test {
	create_directory "$result_dir"
    #exec { & $nunitPath\nunit-console.exe $source_dir\SourceIndexingSharp.Tests\bin\$config\SourceIndexingSharp.Tests.dll /xml=$result_dir\SourceIndexingSharp.Tests.xml }
    if($env:APPVEYOR -ne $NULL) {
        "Uploading unit test reports to AppVeyor"
        $wc = New-Object 'System.Net.WebClient'
        $wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit/$($env:APPVEYOR_JOB_ID)", "$result_dir\SourceIndexingSharp.Tests.xml" )
    }
}

task dist {
	create_directory $build_dir
	create_directory $dist_dir
	copy_files "$source_dir\SourceIndexingSharp\bin\$config" "$dist_dir"
	copy_files "$source_dir\SourceIndexingSharp.Build\bin\$config" "$dist_dir"
	copy_files "$source_dir\SourceIndexingSharp.Extractor\bin\$config" "$dist_dir"
}

function global:create-globalAssemblyInfo($commit, $filename)
{
	$date = Get-Date
    "using System;
using System.Reflection;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: CLSCompliant(true)]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyCopyrightAttribute(""Copyright Paul Knopf 2013-" + $date.Year + """)]
[assembly: AssemblyProductAttribute(""SourceIndexingSharp"")]
[assembly: AssemblyTrademarkAttribute(""SourceIndexingSharp"")]
[assembly: AssemblyCompanyAttribute("""")]
[assembly: AssemblyConfigurationAttribute(""$config"")]
[assembly: AssemblyInformationalVersionAttribute(""$config"")]"  | out-file $filename -encoding "ASCII"    
}

function global:copy_files($source, $destination, $regexFilter = $NULL) {

    create_directory $destination

	if($regexFilter -eq $null) {
		$regexFilter = ".*"
	}

    Get-ChildItem $source -Recurse -Exclude $exclude | Where-Object {$_.FullName -match $regexFilter} |  Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}


function global:create_directory($directory_name)
{
    mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:delete_directory($directory_name)
{
    rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

#Get-ChildItem -Path $source | Where-Object { $_.Name -match $filter } | Copy-Item -Destination $destination