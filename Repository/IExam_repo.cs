using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface IExam_repo: I_BaseRepository<Exams>
    {
        List<Question> GetExamsbyid(int Examid);
        List<Question> GetTasksbyid(int Examid);

        void SaveStudentAnswer(StudentAnswer answer);
        void SaveStudentStatistics(StudentStatistics stats);
        void SaveStudentStatisticsintask(studentstaistics_in_task stats);

    }
}
