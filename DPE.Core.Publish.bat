color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd Release.Core

dotnet publish -o ..\DPE.Core\bin\Release\netcoreapp3.1\  -c Release

md ..\.PublishFiles

xcopy ..\DPE.Core\bin\Release\netcoreapp3.1\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd