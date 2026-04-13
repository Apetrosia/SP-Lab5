using CsvHelper;
using System.Globalization;

namespace GreenswampRazorPages.Services;

public class CsvService : ICsvService
{
    private readonly string _filePath;
    private readonly ILogger<CsvService> _logger;

    public CsvService(ILogger<CsvService> logger)
    {
        _logger = logger;
        var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        if (!Directory.Exists(dataDir))
            Directory.CreateDirectory(dataDir);
        _filePath = Path.Combine(dataDir, "contacts.csv");
        
        if (!File.Exists(_filePath))
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<ContactRecord>();
            csv.NextRecord();
        }
    }

    public async Task AppendContactAsync(string name, string email, string topic, string message)
    {
        var record = new ContactRecord
        {
            Timestamp = DateTime.Now,
            Name = name,
            Email = email,
            Topic = topic,
            Message = message
        };

        await using var writer = new StreamWriter(_filePath, append: true);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        await csv.NextRecordAsync();
        _logger.LogInformation("Contact saved: {Email} at {Time}", email, DateTime.Now);
    }

    private class ContactRecord
    {
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
    }
}