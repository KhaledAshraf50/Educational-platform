using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Luno_platform.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly LunoDBContext _context;

        public AdminRepository(LunoDBContext context)
        {
            _context = context;
        }

        public Users GetAdminByUserId(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public int GetTotalStudents()
        {
            return _context.Students.Count();
        }

        public int GetTotalInstructors()
        {
            return _context.Instructors.Count();
        }

        public int GetTotalCoursesByStatus(string status)
        {
            return _context.Courses.Count(c => c.status == status);
        }

        public int GetTotalCoursesPending()
        {
            return _context.Courses.Count(c => c.status == "قيد المراجعة");
        }

        public decimal GetTotalPayments()
        {
            return _context.Payments.Sum(p => p.amountPayment);
        }
    }
}
