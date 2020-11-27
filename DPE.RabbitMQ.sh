rm -rf .MQFiles;
dotnet build;
dotnet publish -o /server/web/DPE/DPE.Core.RabbitMQ.Subscribe/bin/Debug/netcoreapp3.1;
cp -r /server/web/DPE/DPE.Core.RabbitMQ.Subscribe/bin/Debug/netcoreapp3.1 .MQFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";
