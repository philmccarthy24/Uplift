if ($args.Length -lt 2)
{
	Write-Host "Need source config file and destination dir"
	exit
}
$targetDir = $args[1]
$targetFile = Split-Path -Path $args[0] -Leaf
$targetConfig = Join-Path -Path $args[1] -ChildPath $targetFile
(Get-Content $args[0]) | ForEach-Object { $_ -replace "{REPLACE_ME}", $targetDir } | Set-Content $targetConfig