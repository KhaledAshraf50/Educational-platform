using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class Task_repo: BaseRepository<Tasks>, ITask_repo
    {
        public Task_repo(LunoDBContext database) : base(database)
        {
        }

        public List<Question> GetTasksbyid(int taskid)
        {
            return _Context.Questions
                 .Where(q => q.TaskId == taskid)
                .ToList();
        }


        public void SaveStudentAnswer(StudentAnswer answer)
        {
            _Context.StudentAnswers.Add(answer);
            _Context.SaveChanges();
        }

        public void SaveStudentStatistics(studentstaistics_in_task stats)
        {
            _Context.Studentstaistics_In_Tasks.Add(stats);
            _Context.SaveChanges();
        }

       
    }
}

