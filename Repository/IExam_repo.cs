using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface IExam_repo: I_BaseRepository<Exams>
    {
        List<Question> GetExamsbyid(int Examid);
        void SaveStudentAnswer(StudentAnswer answer);
        void SaveStudentStatistics(StudentStatistics stats);
    }
}
