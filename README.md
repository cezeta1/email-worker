# Email Sender Worker Service

A .NET-based email sending service that handles templated email processing and sending through a worker service architecture.

## 🛠️ Built With

[![Azure][Azure-badge]][Azure-url] &nbsp; &nbsp;
[![NET 9.0][Dotnet-badge]][Dotnet-url] &nbsp; &nbsp;

## 📁 Project Structure

- **CZ.Worker.EmailSender**: Main worker service application
- **[...].Domain**: Core domain models
- **[...].Services**: Service layer implementation
- **[...].Templates**: Email template definitions
- **[...].EmailEngine**: Email sending functionality
- **[...].TemplateResolver**: Template processing

## ⚙️ Setup

1. Clone the repository
2. Open `CZ.Worker.EmailSender.sln` in your IDE
3. Configure the application settings (see below)
4. Build and run the service

### Configuration

The following settings need to be configured in `appsettings.json`:

```json
{
  
  "EmailSenderSettings": {
    "AzureConnString": "<secret>",
    "TopicName": "<env. specific>",
    "SubscriptionName": "<env. specific>",
    "DevPrefix": "<env. specific>"
  },

  "EmailEngineSettings": {
    "SenderName": "<env. specific>",
    "SenderEmail": "<env. specific>",
    "SendGridAPIKey": "<secret>"
  }
}
```

### Logging 

```json 
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    
    "SerilogSettings": {
        "LogMessageType": "Simple",
        "MinimumLevel": "Verbose",
        "OutputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{Version}] [{TraceId}] [{WorkerId}] || {Message:lj}{NewLine}{Exception}",
        "Console": {
          "RestrictedToMinimumLevel": "Verbose"
        },
        "AzureBlob": {
            "StorageContainerName": "emailworkerlogs",
            "StorageFileName": "{yyyy}-{MM}-{dd}/EmailSenderWorker-Log-{WorkerId}-{yyyy}-{MM}-{dd}-{HH}:{mm}.txt",
            "WriteInBatches": true,
            "PeriodSeconds": 15,
            "BatchPostingLimit": 50
        },
        "File": {
            "RestrictedToMinimumLevel": "Error",
            "Path": "log.txt"
        }
    }
}
``` 

### Email Sender Service

```json 
{
  "EmailSenderSettings": {
    "AzureConnString": "<secret>",
    "TopicName": "<env. specific>",
    "SubscriptionName": "<env. specific>",
    "DevPrefix": "<env. specific>"
  }
}
```

### Email Engine

```json 
{
  "EmailEngineSettings": {
    "SenderName": "<env. specific>",
    "SenderEmail": "<env. specific>",
    "SendGridAPIKey": "<secret>"
  }
}
``` 

Replace all placeholder values (`<...>`) with your actual configuration values.

## 💻 Usage

Once configured, the worker service will:

1. Listen for email requests on the configured Azure Service Bus queue
2. Process email templates
3. Send emails using the configured SMTP settings
4. Log operations through the configured logging service

## 📝 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details.

<!-- References -->

[Azure-badge]: https://img.shields.io/badge/microsoft%20azure-0089D6?style=for-the-badge&logo=microsoft-azure&logoColor=white
[Azure-url]: https://azure.microsoft.com/
[Dotnet-badge]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[Dotnet-url]: https://dotnet.microsoft.com/
