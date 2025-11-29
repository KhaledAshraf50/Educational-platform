using Luno_platform.Models;
using System.Collections.Generic;

namespace Luno_platform.Repository
{
    public interface IPaymentRepo : I_BaseRepository<Payments>
    {
        void AddPayment(Payments payment);
        List<Payments> GetStudentPayments(int studentId);
        IEnumerable<Payments> GetPayments(DateTime start, DateTime end);

    }
}
