using Luno_platform.Models;
using Luno_platform.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private IstudentService istudentService;
    private Icourses_service icourses_Service;
    public PaymentController(IstudentService studentService,IPaymentService paymentService, Icourses_service icourses_Service)
    {
        _paymentService = paymentService;
        istudentService = studentService;
        this.icourses_Service = icourses_Service;
    }
    [Authorize]
    [HttpPost]
    public IActionResult Pay(int courseId, decimal amount)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);
        var course = icourses_Service.Infocourse(courseId);
        try
        {
            if (student.Balance>= amount)
            {
                
                istudentService.ChargeBalanceAfterPay(userId, amount);
                _paymentService.CreatePayment(userId, courseId, amount);
                TempData["AlertMessage"] = "لقد تم الاشتراك في هذا الكورس";
            }
            else
            {
                TempData["AlertMessage"] = "الرصيد غير كافي من فضلك قم بشحن رصيدك اولا";
                //return RedirectToAction("show_details_courses", "Homepage", new { courseid = courseId, fromTask = true });
            }
         
            return RedirectToAction("show_details_courses", "Homepage", new { courseid = courseId, fromTask = true });
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
