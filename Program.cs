using GreenswampRazorPages;
using GreenswampRazorPages.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<HttpRequestLogger>();
app.UseRouting();
app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/NotFound");
app.MapRazorPages();

app.Run();