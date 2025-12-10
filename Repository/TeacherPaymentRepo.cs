using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class TeacherPaymentRepo : BaseRepository<Teacher_payment2>, ITeacherPaymentRepo
    {
        public TeacherPaymentRepo(LunoDBContext database) : base(database)
        {
        }

        public List<Teacher_payment2> GetByInstructorId(int instructorId)
        {
            return _Context.Teacher_Payments2
                .Where(tp => tp.instructorID == instructorId)
                .Include(tp => tp.Instructor)
                    .ThenInclude(i => i.User)
                .OrderByDescending(tp => tp.PaymentDate)
                .ToList();
        }

        public List<Teacher_payment2> GetByStatus(string status)
        {
            return _Context.Teacher_Payments2
                .Where(tp => tp.Status == status)
                .Include(tp => tp.Instructor)
                    .ThenInclude(i => i.User)
                .OrderByDescending(tp => tp.PaymentDate)
                .ToList();
        }
    }
}