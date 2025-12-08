using Luno_platform.Models;
using Luno_platform.Repository;

namespace Luno_platform.Service
{
    public class PaymentService : BaseService<Payments>, IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IstudentRepo _studentRepo;
        private readonly I_instructor_repo _instructorRepo;
        private readonly ITeacherPaymentRepo _teacherPaymentRepo;
        private readonly LunoDBContext _context;

        public PaymentService(
             IPaymentRepo paymentRepo,
             IstudentRepo studentRepo,
             I_instructor_repo instructorRepo,
             ITeacherPaymentRepo teacherPaymentRepo,
             LunoDBContext context) : base(paymentRepo)
        {
            _paymentRepo = paymentRepo;
            _studentRepo = studentRepo;
            _instructorRepo = instructorRepo;
            _teacherPaymentRepo = teacherPaymentRepo;
            _context = context;
        }

        public void CreatePayment(int userId, int courseId, decimal amount)
        {
            int? studentId = _studentRepo.GetStudentIdByUserId(userId);

            if (studentId == null)
                throw new Exception("User is not a student.");

            Payments payment = new Payments
            {
                StudentID = studentId.Value,
                courseId = courseId,
                amountPayment = amount,
                status = "مقبول", 
                date = DateTime.Now
            };
            _paymentRepo.AddPayment(payment);

            Student_Courses studentCourse = new Student_Courses
            {
                StudentId = studentId.Value,
                CourseId = courseId
            };
            _studentRepo.AddStudentCourse(studentCourse);

            decimal instructorAmount = amount * 0.60m;

            var course = _context.Courses.Find(courseId);
            if (course == null)
                throw new Exception("Course not found.");

            var teacherPayment2 = new Teacher_payment2
            {
                PaymentRefId = payment.ID, 
                instructorID = course.instructorID,
                AmountPaid = instructorAmount,
                Status = "تحت التحويل",
                PaymentDate = DateTime.Now
            };
            _teacherPaymentRepo.Add(teacherPayment2);
            _teacherPaymentRepo.Save();

            var instructor = _instructorRepo.GetById(course.instructorID);
            if (instructor != null)
            {
                instructor.PendingBalance += instructorAmount;
                _instructorRepo.Update(instructor);
                _instructorRepo.Save();
            }
        }
        public List<Payments> GetStudentPayments(int userId)
        {
            int? studentId = _studentRepo.GetStudentIdByUserId(userId);

            if (studentId == null)
                return new List<Payments>();

            return _paymentRepo.GetStudentPayments(studentId.Value);
        }
        public void TransferToInstructor(int paymentId)
        {
            // 1. جلب Teacher_payment المربوط بالـ Payment
            var teacherPayment = _context.Teacher_Payments2
                .FirstOrDefault(tp => tp.PaymentRefId == paymentId && tp.Status == "تحت التحويل");

            if (teacherPayment == null)
                throw new Exception("لم يتم العثور على سجل الدفع للمدرس");

            var instructor = _instructorRepo.GetById(teacherPayment.instructorID);
            if (instructor == null)
                throw new Exception("المدرس غير موجود");

            decimal amount = teacherPayment.AmountPaid;

            teacherPayment.Status = "محول";
            _context.Teacher_Payments2.Update(teacherPayment);
            _context.SaveChanges();

            instructor.PendingBalance -= amount;
            instructor.AvailableBalance += amount;
            instructor.TotalEarnings += amount;

            _instructorRepo.Update(instructor);
            _instructorRepo.Save();
        }
        public void CancelEnrollment(int paymentId)
        {
            var payment = _paymentRepo.GetById(paymentId);

            if (payment == null)
                throw new Exception("Payment not found.");

            if (payment.status != "مقبول")
                throw new Exception("يمكن إلغاء الاشتراك للمعاملات المقبولة فقط");

            _studentRepo.RemoveStudentCourse(payment.StudentID, payment.courseId);

            payment.status = "ملغي";
            _paymentRepo.Update(payment);
            _paymentRepo.Save();

            var course = _context.Courses.Find(payment.courseId);
            if (course != null)
            {
                decimal instructorAmount = payment.amountPayment * 0.60m;

                var teacherPayment = _context.Teacher_Payments
                    .FirstOrDefault(tp => tp.PaymentRefId == payment.ID && tp.Status == "تحت التحويل");

                if (teacherPayment != null)
                {
                    _context.Teacher_Payments.Remove(teacherPayment);
                    _context.SaveChanges();
                }
                // تحديث PendingBalance للمدرس
                var instructor = _instructorRepo.GetById(course.instructorID);
                if (instructor != null)
                {
                    instructor.PendingBalance -= instructorAmount;
                    _instructorRepo.Update(instructor);
                    _instructorRepo.Save();
                }
            }
        }
        //public void AcceptPayment(int paymentId)
        //{
        //    var payment = _paymentRepo.GetById(paymentId);

        //    if (payment == null)
        //        throw new Exception("Payment not found.");

        //    if (payment.status != "تحت المراجعة")
        //        throw new Exception("يمكن قبول المعاملات المعلقة فقط");

        //    // ✅ 1. تحديث حالة الدفع
        //    payment.status = "مقبول";
        //    _paymentRepo.Update(payment);
        //    _paymentRepo.Save();

        //    // ✅ 2. تسجيل الكورس عند الطالب
        //    Student_Courses studentCourse = new Student_Courses
        //    {
        //        StudentId = payment.StudentID,
        //        CourseId = payment.courseId
        //    };
        //    _studentRepo.AddStudentCourse(studentCourse);

        //    // ✅ 3. حساب نسبة المدرس (60%)
        //    decimal instructorAmount = payment.amountPayment * 0.60m;

        //    // ✅ 4. الحصول على معرف المدرس من الكورس
        //    var course = _context.Courses.Find(payment.courseId);
        //    if (course == null)
        //        throw new Exception("Course not found.");

        //    // ✅ 5. تسجيل في جدول Teacher_payment
        //    var teacherPayment2 = new Teacher_payment2
        //    {
        //        instructorID = course.instructorID,
        //        AmountPaid = instructorAmount,
        //        Status = "مستحق", // Pending
        //        PaymentDate = DateTime.Now
        //    };
        //    _teacherPaymentRepo.Add(teacherPayment2);
        //    _teacherPaymentRepo.Save();

        //    // ✅ 6. تحديث PendingBalance للمدرس
        //    var instructor = _instructorRepo.GetById(course.instructorID);
        //    if (instructor != null)
        //    {
        //        instructor.PendingBalance += instructorAmount;
        //        instructor.TotalEarnings += instructorAmount;
        //        _instructorRepo.Update(instructor);
        //        _instructorRepo.Save();
        //    }
        //}

        //public void RejectPayment(int paymentId, string reason = null)
        //{
        //    var payment = _paymentRepo.GetById(paymentId);

        //    if (payment == null)
        //        throw new Exception("Payment not found.");

        //    if (payment.status != "تحت المراجعة")
        //        throw new Exception("يمكن رفض المعاملات المعلقة فقط");

        //    payment.status = "مرفوض";
        //    _paymentRepo.Update(payment);
        //    _paymentRepo.Save();
        //}
        public List<Payments> GetPaymentsByStatus(string status)
        {
            return _paymentRepo.GetPaymentsByStatus(status);
        }
        public decimal GetTotalRevenue()
        {
            return _context.Payments
                .Where(p => p.status == "مقبول")
                .Sum(p => (decimal?)p.amountPayment) ?? 0;
        }

        public decimal GetPlatformProfit()
        {
            return GetTotalRevenue() * 0.40m;
        }

        public decimal GetTotalInstructorsDue()
        {
            return GetTotalRevenue() * 0.60m;
        }

        public void AcceptPayment(int paymentId)
        {
            // تم إلغاء نظام الموافقة
            return;
        }

        public void RejectPayment(int paymentId, string reason = null)
        {
            // تم إلغاء نظام الرفض
            return;
        }
    }
}
