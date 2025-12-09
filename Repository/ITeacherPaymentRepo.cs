using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface ITeacherPaymentRepo : I_BaseRepository<Teacher_payment2>
    {
        List<Teacher_payment2> GetByInstructorId(int instructorId);
        List<Teacher_payment2> GetByStatus(string status);
    }
}