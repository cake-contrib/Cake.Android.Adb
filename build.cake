var sln = "./Cake.Android.SdkManager.sln";
var nuspec = "./Cake.Android.SdkManager.nuspec";

var target = Argument ("target", "libs");

var NUGET_VERSION = Argument("nugetversion", "0.9999");

var SDK_URL_BASE = "https://dl.google.com/android/repository/tools_r{0}-{1}.zip";
var SDK_VERSION = "25.2.3";

Task ("externals")
	.WithCriteria (!FileExists ("./android-sdk/android-sdk.zip"))
	.Does (() => 
{
	var url = string.Format (SDK_URL_BASE, SDK_VERSION, "macosx");
	if (IsRunningOnWindows ())
		url = string.Format (SDK_URL_BASE, SDK_VERSION, "windows");

	EnsureDirectoryExists ("./android-sdk/");

	DownloadFile (url, "./android-sdk/android-sdk.zip");

	Unzip ("./android-sdk/android-sdk.zip", "./android-sdk/");

	// Install platform-tools so we get adb
	StartProcess ("./android-sdk/tools/bin/sdkmanager", new ProcessSettings { Arguments = "platform-tools" });
});

Task ("libs").Does (() => 
{
	NuGetRestore (sln);

	DotNetBuild (sln, c => c.Configuration = "Release");
});

Task ("nuget").IsDependentOn ("libs").Does (() => 
{
	CreateDirectory ("./nupkg/");

	NuGetPack (nuspec, new NuGetPackSettings { 
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./nupkg/",
		Version = NUGET_VERSION,
		// NuGet messes up path on mac, so let's add ./ in front again
		BasePath = "././",
	});	
});

Task ("clean").Does (() => 
{
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");

	CleanDirectories ("./**/Components");
	CleanDirectories ("./**/tools");

	DeleteFiles ("./**/*.apk");
});

RunTarget (target);