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

            Payments payment = new Payments
            {
                StudentID = studentId.Value,
                courseId = courseId,
                amountPayment = amount,
                status = "مقبول",
                date = DateTime.Now
            };

            _paymentRepo.AddPayment(payment);
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
