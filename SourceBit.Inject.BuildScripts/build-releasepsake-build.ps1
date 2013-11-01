Properties {
    #Solution
    $currentDir = Resolve-Path .
    $baseDir = Resolve-Path ../
	$packagesDir = Resolve-Path ../packages/
    $buildConfig = "Release"
	# Common
	$buildDate = Get-Date -format "yyyy-MMM-dd"
    # MSBuild
    $msbuildVerbosity = "minimal"
    $msbuildCpuCount = [System.Environment]::ProcessorCount / 2
    $msbuildParralel = $true
    # NuGet
    $nugetPackageDir = ".packages"
    $nugetPackagePath = "$baseDir\$nugetPackageDir"
    #nunit
    $nunitConsole = "$packagesDir\NUnit.2.6.3-complete\nunit-console.exe"
	#nuget
	$nugetpath = "$packagesDir\nuget\nuget.exe"
}

Task Default -Depends Clean, Compile, Unit-Test, Nuget-Update, Nuget-Pack, Nuget-CopyPackage, Nuget-Publish

Task Clean {

    foreach($projInfo in Get-Projects-Info)
    {
        $projDir = $projInfo.Directory;

        Delete "$projDir\bin"
        Write-Host "Clean-up folder $projDir\bin"
        
        Delete "$projDir\obj"
        Write-Host "Clean-up folder $projDir\obj"
    }
}

Task Compile {
    foreach($projInfo in Get-Projects-Info)
    {
        $projFile = $projInfo.FullName
        
        try
        {
            exec { msbuild /nologo /v:$msbuildVerbosity /m:$msbuildCpuCount /p:BuildInParralel=$msbuildParralel /p:Configuration="$buildConfig" /p:Platform="AnyCPU" /p:CustomAfterMicrosoftCommonTargets="$packagesDir\StyleCop\StyleCop.Targets" /p:StyleCopTreatErrorsAsWarnings=true /p:OutDir="$($projInfo.ProjectOutput)" "$projFile" }
        }
        catch
        {
            Exit-Build "Failed to build project '$projFile'"
        }
    }
}

Task Unit-Test {
    $failurePattern = ",\s+[1-9]\d*\s+failed,"

    $successPattern = "Test assembly:.+seconds"

    foreach($projInfo in Get-Projects-Info)
    {
        if ($projInfo.IsTestProject)
        {
            $assemblyPath = "$($projInfo.ProjectOutput + $projInfo.AssemblyName).dll"

            & $nunitConsole $assemblyPath
        }
    }
}

Task Nuget-Update {
    & $nugetpath "Update", "-self"
}

Task Nuget-Pack {
    & $nugetpath "Pack", "$baseDir\nuget\SourceBit.Inject.nuspec"
}

Task Nuget-CopyPackage {
    Move-Item "$currentDir\*.nupkg" "$baseDir\releases\" -force
}

Task Nuget-Publish {
	Get-Content "$baseDir\..\nuget.key" | Foreach-Object{
	   $apiKey = $_
	}

    & $nugetpath "SetApiKey", "$apiKey"

    & $nugetpath "Push", "$baseDir\releases\SourceBit.Inject.0.0.0.1.nupkg"
}

function Get-Projects-Info
{
    $paths = New-Object System.Collections.Generic.List[PSObject]

    # Gets the project files based on a given file's extension
    $projFiles = @(gci "$baseDir" -Recurse -Filter "*.csproj")

    foreach($file in $projFiles)
    {
        $projDir = $file.Directory

        $fullName = $file.FullName

        $bin = "$projDir\bin\$buildConfig\"

        $projDocument = New-Object XML

        $projDocument.Load($fullName)

        # Gets the assembly's name out of the project's file
        $assemblyName = ($projDocument.Project.PropertyGroup | where { $_.AssemblyName -ne $null }).AssemblyName
        
        $isTestProject = ($assemblyName -like "*Tests*")

        $paths.Add(@{
            File = $file;
            Directory = $projDir;
            FullName = $fullName;
            ProjectOutput = $bin;
            AssemblyName = $assemblyName;
            IsTestProject = $isTestProject
        })
    }

    return $paths;
}

function Delete([String]$Path)
{
    del "$Path" -Force -Recurse -ErrorAction SilentlyContinue
}