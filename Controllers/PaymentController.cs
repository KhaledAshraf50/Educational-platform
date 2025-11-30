using Luno_platform.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    [Authorize]
    [HttpPost]
    public IActionResult Pay(int courseId, decimal amount)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

        try
        {
            _paymentService.CreatePayment(userId, courseId, amount);
            return RedirectToAction("Success"); // صفحة نجاح الدفع
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public IActionResult Success()
    {
        return View();
    }
}
