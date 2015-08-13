$msBuildPath = "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"

$solutionDir = Resolve-Path "../"
Push-Location $solutionDir

$testProjects = ls -r -inc *Tests.csproj
Foreach($project in $testProjects)
{
	Write-Host "Building appx package for $project"		
	& $msBuildPath $project
}

$vstestDir = "${env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow"
$testAssemblies = ls -r -inc *Tests*.appx
Foreach($testAssembly in $testAssemblies)
{
	$certFile = $testAssembly -replace "appx", "cer"
	Import-Certificate -FilePath $certFile -CertStoreLocation 'cert:\LocalMachine\Root' -Verbose 
	&"$vstestDir\vstest.console.exe" "$testAssembly"  /InIsolation
}

Pop-Location