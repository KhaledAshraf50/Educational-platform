using Luno_platform.Models;
using Luno_platform.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private IstudentService istudentService;
    private Icourses_service icourses_Service;
    private readonly PaymobService _paymobService;
    private readonly IConfiguration _configuration;

    public PaymentController(
        IstudentService studentService,
        IPaymentService paymentService,
        Icourses_service icourses_Service,
        PaymobService paymobService,
        IConfiguration configuration)
    {
        _paymentService = paymentService;
        istudentService = studentService;
        this.icourses_Service = icourses_Service;
        _paymobService = paymobService;
        _configuration = configuration;
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
            if (student.Balance >= amount)
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
    [Authorize(Roles ="student")]
    [HttpGet]
    public IActionResult SelectPaymentMethod(int courseId)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);
        var course = icourses_Service.Infocourse(courseId);

        if (course == null)
            return NotFound();

        ViewBag.CourseId = courseId;
        ViewBag.CourseName = course.Course.CourseName;
        ViewBag.Amount = course.Course.price;
        ViewBag.StudentBalance = student.Balance;

        return View();
    }

    // ✅ 1. الدفع من الرصيد
    [Authorize]
    [HttpPost]
    public IActionResult PayFromBalance(int courseId, decimal amount)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);

        try
        {
            if (student.Balance >= amount)
            {
                istudentService.ChargeBalanceAfterPay(userId, amount);
                _paymentService.CreatePayment(userId, courseId, amount);
                TempData["AlertMessage"] = "✅ تم الاشتراك في الكورس بنجاح من رصيدك!";
            }
            else
            {
                TempData["AlertMessage"] = "❌ الرصيد غير كافي. اختر طريقة دفع أخرى.";
            }

            return RedirectToAction("show_details_courses", "Homepage", new { courseid = courseId, fromTask = true });
        }
        catch (Exception ex)
        {
            TempData["AlertMessage"] = $"❌ حدث خطأ: {ex.Message}";
            return RedirectToAction("SelectPaymentMethod", new { courseId });
        }
    }

    // ✅ 2. الدفع بالبطاقة (Paymob)
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PayByCard(int courseId, decimal amount)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);

        try
        {
            string orderId = $"ORDER_{userId}_{courseId}_{DateTime.Now.Ticks}";

            string paymentUrl = await _paymobService.CreateCardPaymentLinkAsync(
                amount,
                orderId,
                student.User.fname ?? "Student",
                student.User.lastName ?? "User",
                student.User.Email ?? "student@example.com",
                student.User.PhoneNumber ?? "01000000000"
            );

            // حفظ معلومات الدفع مؤقتاً
            TempData["PendingPayment"] = System.Text.Json.JsonSerializer.Serialize(new
            {
                OrderId = orderId,
                CourseId = courseId,
                UserId = userId,
                Amount = amount
            });

            return Redirect(paymentUrl);
        }
        catch (Exception ex)
        {
            TempData["AlertMessage"] = $" حدث خطأ في الدفع: {ex.Message}";
            return RedirectToAction("SelectPaymentMethod", new { courseId });
        }
    }

    // ✅ 3. الدفع بالمحفظة - اختيار نوع المحفظة
    [Authorize]
    [HttpPost]
    public IActionResult PayByWallet(int courseId, decimal amount)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);
        var course = icourses_Service.Infocourse(courseId);

        ViewBag.CourseId = courseId;
        ViewBag.CourseName = course.Course.CourseName;
        ViewBag.Amount = amount;
        ViewBag.StudentName = student.User.fname + " " + student.User.lastName;
        ViewBag.StudentPhone = student.User.PhoneNumber ?? "01000000000";

        return View("SelectWalletType");
    }

    // ✅ 3.1 تأكيد بيانات المحفظة
    [Authorize]
    [HttpPost]
    public IActionResult ConfirmWalletPayment(int courseId, decimal amount, string walletType)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);
        var course = icourses_Service.Infocourse(courseId);

        ViewBag.CourseId = courseId;
        ViewBag.CourseName = course.Course.CourseName;
        ViewBag.Amount = amount;
        ViewBag.WalletType = walletType;
        ViewBag.StudentName = student.User.fname + " " + student.User.lastName;
        ViewBag.StudentPhone = student.User.PhoneNumber ?? "01000000000";
        ViewBag.StudentEmail = student.User.Email ?? "";

        return View("ConfirmWalletPayment");
    }

    // ✅ 3.2 تنفيذ الدفع بالمحفظة
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProcessWalletPayment(
        int courseId,
        decimal amount,
        string walletType,
        string phoneNumber)
    {
        int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        var student = istudentService.GetStudent(userId);

        try
        {
            // التحقق من رقم الموبايل
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 11 || !phoneNumber.StartsWith("01"))
            {
                TempData["AlertMessage"] = " رقم الموبايل غير صحيح. يجب أن يبدأ بـ 01 ويتكون من 11 رقم";
                return RedirectToAction("ConfirmWalletPayment", new { courseId, amount, walletType });
            }

            string orderId = $"ORDER_{userId}_{courseId}_{DateTime.Now.Ticks}";

            // محاولة إنشاء رابط الدفع
            string paymentUrl = await _paymobService.CreateWalletPaymentLinkAsync(
                amount,
                orderId,
                student.User.fname ?? "Student",
                student.User.lastName ?? "User",
                student.User.Email ?? "student@example.com",
                phoneNumber
            );

            // التحقق من أن الرابط صحيح
            if (string.IsNullOrEmpty(paymentUrl))
            {
                throw new Exception("فشل في إنشاء رابط الدفع من Paymob");
            }

            // حفظ معلومات الدفع المعلق
            TempData["PendingPayment"] = System.Text.Json.JsonSerializer.Serialize(new
            {
                OrderId = orderId,
                CourseId = courseId,
                UserId = userId,
                Amount = amount,
                WalletType = walletType,
                PhoneNumber = phoneNumber
            });

            // التوجيه لصفحة Paymob
            return Redirect(paymentUrl);
        }
        catch (Exception ex)
        {
            // عرض رسالة الخطأ بالتفصيل
            TempData["AlertMessage"] = $" حدث خطأ: {ex.Message}";

            // الرجوع لصفحة التأكيد
            return RedirectToAction("ConfirmWalletPayment", new { courseId, amount, walletType });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult PaymentCallback()
    {
        try
        {
            // استقبال البيانات من Paymob
            var success = Request.Query["success"].ToString();
            var orderId = Request.Query["order"].ToString();
            var amountCents = Request.Query["amount_cents"].ToString();
            var hmac = Request.Query["hmac"].ToString();

            // التحقق من HMAC (أمان)
            if (!VerifyHmac(hmac, orderId, success, amountCents))
            {
                TempData["AlertMessage"] = "❌ فشل التحقق من صحة الدفع";
                return RedirectToAction("Index", "Home");
            }

            if (success == "true")
            {
                // استخراج البيانات من OrderId
                var parts = orderId.Split('_');
                if (parts.Length < 3)
                {
                    TempData["AlertMessage"] = "❌ خطأ في معرف الطلب";
                    return RedirectToAction("Index", "Home");
                }

                int userId = int.Parse(parts[1]);
                int courseId = int.Parse(parts[2]);
                var course = icourses_Service.Infocourse(courseId);

                // ✅ تسجيل الدفع فوراً (مفيش موافقة من الأدمن)
                _paymentService.CreatePayment(userId, courseId, course.Course.price);

                TempData["AlertMessage"] = "✅ تم الدفع بنجاح! تم تسجيلك في الكورس";
                return RedirectToAction("PaymentSuccess", new { courseId });
            }
            else
            {
                TempData["AlertMessage"] = "❌ فشلت عملية الدفع";
                var pendingJson = TempData["PendingPayment"]?.ToString();
                if (!string.IsNullOrEmpty(pendingJson))
                {
                    var pending = System.Text.Json.JsonSerializer.Deserialize<dynamic>(pendingJson);
                    return RedirectToAction("SelectPaymentMethod", new { courseId = pending.CourseId });
                }
                return RedirectToAction("Index", "Home");
            }
        }
        catch (Exception ex)
        {
            TempData["AlertMessage"] = $"❌ حدث خطأ: {ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }

    // ✅ 5. صفحة نجاح الدفع
    [Authorize]
    public IActionResult PaymentSuccess()
    {
        return View();
    }

    // ✅ 6. التحقق من HMAC (للأمان)
    private bool VerifyHmac(string receivedHmac, string orderId, string success, string amountCents)
    {
        var hmacSecret = _configuration["Paymob:HmacSecret"];
        if (string.IsNullOrEmpty(hmacSecret))
            return true; // في حالة عدم تفعيل HMAC

        var data = $"{orderId}{success}{amountCents}";
        using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hmacSecret)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            var calculatedHmac = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return calculatedHmac == receivedHmac.ToLower();
        }
    }
}