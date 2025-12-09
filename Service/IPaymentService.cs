using Luno_platform.Models;
using System.Collections.Generic;

namespace Luno_platform.Service
{
    public interface IPaymentService : I_BaseService<Payments>
    {
        void CreatePayment(int userId, int courseId, decimal amount);
        List<Payments> GetStudentPayments(int userId);
        void AcceptPayment(int paymentId);
        void RejectPayment(int paymentId, string reason = null);
        void CancelEnrollment(int paymentId);
        List<Payments> GetPaymentsByStatus(string status);
        decimal GetTotalRevenue(); // إجمالي المبيعات
        decimal GetPlatformProfit(); // ربح المنصة 40%
        decimal GetTotalInstructorsDue(); // المستحق للمدرسين 60%
        void TransferToInstructor(int paymentId); // تحويل نسبة المدرس
    }
}
