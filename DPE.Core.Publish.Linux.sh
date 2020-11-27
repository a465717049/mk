rm -rf .PublishFiles;
dotnet build;
dotnet publish -o /server/web/DPE/DPE.Core/bin/Release/netcoreapp3.1 -c Release;
cp -r /server/web/DPE/DPE.Core/bin/Release/netcoreapp3.1 .PublishFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";
