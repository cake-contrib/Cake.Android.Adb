#tool nuget:?package=NUnit.ConsoleRunner&version=3.6.0
#addin nuget:?package=Cake.Android.SdkManager

var sln = "./Cake.Android.Adb.sln";
var nuspec = "./Cake.Android.Adb.nuspec";

var target = Argument ("target", "all");
var configuration = Argument ("configuration", "Release");

var NUGET_VERSION = Argument("APPVEYOR_BUILD_VERSION", Argument("nugetversion", "1.0.18"));
var isRunningOnWindows = IsRunningOnWindows();
var SDK_URL_BASE = "https://dl.google.com/android/repository/tools_r{0}-{1}.zip";
var SDK_VERSION = "25.2.3";
var ANDROID_HOME =  EnvironmentVariable("ANDROID_HOME");

Task ("externals")
	.WithCriteria (!FileExists ("./android-sdk/android-sdk.zip") && string.IsNullOrEmpty(ANDROID_HOME))
	.Does (() => 
{
	var url = string.Format (SDK_URL_BASE, SDK_VERSION, "macosx");
	if (IsRunningOnWindows ())
		url = string.Format (SDK_URL_BASE, SDK_VERSION, "windows");

	EnsureDirectoryExists ("./android-sdk/");

	DownloadFile (url, "./android-sdk/android-sdk.zip");

	Unzip ("./android-sdk/android-sdk.zip", "./android-sdk/");

	// Install platform-tools so we get adb
	AndroidSdkManagerInstall (new [] { "platform-tools" }, new AndroidSdkManagerToolSettings {
		SdkRoot = "./android-sdk/", 
	});
});

Task ("libs").IsDependentOn ("externals").Does (() => 
{
	NuGetRestore (sln);

	DotNetBuild (sln, c => c.Configuration = configuration);
});

Task ("nuget").IsDependentOn ("libs").Does (() => 
{
	NuGetPack (nuspec, new NuGetPackSettings { 
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		Version = NUGET_VERSION,
		// NuGet messes up path on mac, so let's add ./ in front again
		BasePath = "././",
	});	
});

Task("tests")
.IsDependentOn("libs")
.WithCriteria(() => !isRunningOnWindows)
.Does(() =>
{
	NUnit3("./**/bin/" + configuration + "/*.Tests.dll");
});

Task ("clean").Does (() => 
{
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");

	CleanDirectories ("./**/Components");
	CleanDirectories ("./**/tools");

	CleanDirectories ("./android-sdk");
	
	DeleteFiles ("./**/*.apk");
});

Task ("all").IsDependentOn("nuget").IsDependentOn ("tests");

RunTarget (target);