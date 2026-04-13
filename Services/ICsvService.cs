namespace GreenswampRazorPages.Services;

public interface ICsvService
{
    Task AppendContactAsync(string name, string email, string topic, string message);
}