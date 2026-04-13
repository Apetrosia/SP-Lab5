using GreenswampRazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GreenswampRazorPages.Pages;

public class ContactModel : PageModel
{
    private readonly ICsvService _csvService;

    public ContactModel(ICsvService csvService)
    {
        _csvService = csvService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name too long")]
    public string Name { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [EduEmail(ErrorMessage = "Only .edu email addresses are allowed")]
    public string Email { get; set; }

    [BindProperty]
    public string Topic { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Message cannot be empty")]
    [StringLength(2000, ErrorMessage = "Message too long (max 2000 characters)")]
    public string Message { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _csvService.AppendContactAsync(Name, Email, Topic, Message);
        
        TempData["ContactSuccess"] = "Your message has been sent! We'll hop back to you soon.";
        return RedirectToPage();
    }
}

public class EduEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        var email = value.ToString();
        if (email.Contains("@") && email.Split('@')[1].EndsWith(".edu", StringComparison.OrdinalIgnoreCase))
            return ValidationResult.Success;
        return new ValidationResult(ErrorMessage ?? "Only .edu email addresses are allowed.");
    }
}