using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface IPaymentRepository : I_BaseRepository<Payments>
    {
        IEnumerable<Payments> GetPayments(DateTime start, DateTime end);

    }
}
