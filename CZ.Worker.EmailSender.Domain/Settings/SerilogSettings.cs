namespace CZ.Worker.EmailSender.Domain.Settings;

public class SerilogSettings
{
    public string MinimumLevel { get; set; }
    public string OutputTemplate { get; set; }
    public SerilogConsoleSettings Console { get; set; }
    public SerilogAzureBlobSettings AzureBlob { get; set; }
    public SerilogFileSettings File { get; set; }
}

public class SerilogConsoleSettings
{
    public string RestrictedToMinimumLevel { get; set; }
}

public class SerilogAzureBlobSettings
{
    public string StorageContainerName { get; set; }
    public string StorageFileName { get; set; }
    public bool WriteInBatches { get; set; }
    public int PeriodSeconds { get; set; }
    public int BatchPostingLimit { get; set; }
}

public class SerilogFileSettings
{
    public string RestrictedToMinimumLevel { get; set; }
    public string Path { get; set; }
}
