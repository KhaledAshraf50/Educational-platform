using Luno_platform.Models;
using Luno_platform.Repository;

namespace Luno_platform.Service
{
    public class PaymentService : BaseService<Payments>, IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly studentRepo _studentRepo;

        public PaymentService(IPaymentRepo paymentRepo, studentRepo studentRepo) : base(paymentRepo)
        {
            _paymentRepo = paymentRepo;
            _studentRepo = studentRepo;
        }

        public void CreatePayment(int userId, int courseId, decimal amount)
        {
            int? studentId = _studentRepo.GetStudentIdByUserId(userId);

            if (studentId == null)
                throw new Exception("User is not a student.");

            // تسجيل الدفع
            Payments payment = new Payments
            {
                StudentID = studentId.Value,
                courseId = courseId,
                amountPayment = amount,
                status = "تحت المراجعة",
                date = DateTime.Now
            };
            _paymentRepo.AddPayment(payment);

            // تسجيل الكورس عند الطالب
            Student_Courses studentCourse = new Student_Courses
            {
                StudentId = studentId.Value,
                CourseId = courseId
            };
            _studentRepo.AddStudentCourse(studentCourse);
        }


        public List<Payments> GetStudentPayments(int userId)
        {
            int? studentId = _studentRepo.GetStudentIdByUserId(userId);

            if (studentId == null)
                return new List<Payments>();

            return _paymentRepo.GetStudentPayments(studentId.Value);
        }
    }
}
