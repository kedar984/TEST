using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SqlHelps.Web.Models;
using SqlHelps.Web.Services;

namespace SqlHelps.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEmailService _emailService;

    public HomeController(ILogger<HomeController> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitInquiry(InquiryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for inquiry submission");
            return View("Index", model);
        }

        try
        {
            _logger.LogInformation("Processing inquiry for {FullName}", model.FullName);
            
            string subject = $"New Website Inquiry from {model.FullName}";
            string body = $"<h3>New Inquiry</h3>" +
                          $"<p><strong>Name:</strong> {model.FullName}</p>" +
                          $"<p><strong>Email:</strong> {model.Email}</p>" +
                          $"<p><strong>Message:</strong></p>" +
                          $"<p>{model.Message}</p>";

            await _emailService.SendEmailAsync("kedar.giri@sqlhelps.com", subject, body);

            TempData["SuccessMessage"] = "Thank you for your inquiry. Our experts will contact you shortly.";
            return Redirect(Url.Action("Index", "Home") + "#contact");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting inquiry for {FullName}", model.FullName);
            ModelState.AddModelError("", "There was an error sending your message. Please try again later.");
            return View("Index", model);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
