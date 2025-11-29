using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class PaymentRepository : BaseRepository<Payments>, IPaymentRepository
    {


        public PaymentRepository(LunoDBContext db) : base(db)
        {

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

    }
}

