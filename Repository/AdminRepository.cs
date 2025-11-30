using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Luno_platform.Repository
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(LunoDBContext lunoDBContext) : base(lunoDBContext)
        {
        }

        //private readonly LunoDBContext _context;

        //public AdminRepository(LunoDBContext context)
        //{
        //    _context = context;
        //}

        public Users GetAdminByUserId(int userId)
        {
            return _Context.Users.FirstOrDefault(u => u.Id == userId);
        }
        public Admin GetAdmin(int userId)
        {
            return _Context.Admins.Include(u=>u.User).FirstOrDefault(u => u.UserId == userId);
        }

        public int GetTotalStudents()
        {
            return _Context.Students.Count();
        }

        public int GetTotalInstructors()
        {
            return _Context.Instructors.Count();
        }

        public int GetTotalCoursesByStatus(string status)
        {
            return _Context.Courses.Count(c => c.status == status);
        }

        public int GetTotalCoursesPending()
        {
            return _Context.Courses.Count(c => c.status == "قيد المراجعة");
        }

        public decimal GetTotalPayments()
        {
            return _Context.Payments.Sum(p => p.amountPayment);
        }
    }
}
