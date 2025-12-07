using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Payments> GetPayments(DateTime start, DateTime end)
        {
            return _Context.Payments
                .Include(p => p.Courses)
                    .ThenInclude(c => c.Instructor)
                        .ThenInclude(i => i.User)
                .Where(p => p.date >= start && p.date <= end)
                .ToList();
        }

        public List<Payments> GetStudentPayments(int studentId)
        {
            return Table.Where(p => p.StudentID == studentId).ToList();
        }
        public List<Payments> GetPaymentsByStatus(string status)
        {
            return _Context.Payments
                .Where(p => p.status == status)
                .Include(p => p.Courses)
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .OrderByDescending(p => p.date)
                .ToList();
        }
    }
}

