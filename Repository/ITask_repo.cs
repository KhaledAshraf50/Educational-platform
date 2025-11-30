using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface ITask_repo: I_BaseRepository<Tasks>
    {
        List<Question> GetTasksbyid(int Examid);
        void SaveStudentAnswer(StudentAnswer answer);
        void SaveStudentStatistics(studentstaistics_in_task stats);
    }
}
