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

app.MapControllerRoute(
    name: "profile",
    pattern: "profile/{username}",
    defaults: new { controller = "Profile", action = "Index" });

app.MapControllerRoute(
    name: "postDetail",
    pattern: "feed/post/{postId}",
    defaults: new { controller = "Feed", action = "PostDetail" });

app.MapControllerRoute(
    name: "ponds",
    pattern: "ponds/{tag}",
    defaults: new { controller = "Ponds", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feed}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();