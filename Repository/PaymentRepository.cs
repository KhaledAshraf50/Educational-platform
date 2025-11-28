using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public class PaymentRepo : BaseRepository<Payments>, IPaymentRepo
    {
        public PaymentRepo(LunoDBContext db) : base(db)
        {
        }

        public void AddPayment(Payments payment)
        {
            Table.Add(payment);
            Save();
        }

        public List<Payments> GetStudentPayments(int studentId)
        {
            return Table.Where(p => p.StudentID == studentId).ToList();
        }
    }
}
