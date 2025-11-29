using Luno_platform.Models;
using System.Collections.Generic;

namespace Luno_platform.Service
{
    public interface IPaymentService : I_BaseService<Payments>
    {
        void CreatePayment(int userId, int courseId, decimal amount);
        List<Payments> GetStudentPayments(int userId);
    }
}
