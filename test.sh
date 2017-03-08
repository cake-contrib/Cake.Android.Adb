#!/bin/sh
mono tools/nuget/nuget.exe update -self
mono tools/nuget/nuget.exe install Cake -OutputDirectory tools -ExcludeVersion

mono tools/Cake/Cake.exe test.cake --verbosity=diagnostic
