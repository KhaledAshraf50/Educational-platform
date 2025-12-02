using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class Exam_repo:  BaseRepository<Exams>, IExam_repo
    {
        public Exam_repo(LunoDBContext database) : base(database)
        {
        }

        public List<Question> GetExamsbyid(int Examid)
        {
            return _Context.Questions
                 .Where(q => q.ExamId == Examid)
                .ToList();
        }

        public List<Question> GetTasksbyid(int TaskId)
        {
            return _Context.Questions
                            .Where(q => q.TaskId == TaskId)
                           .ToList();
        }

        public void SaveStudentAnswer(StudentAnswer answer)
        {
            _Context.StudentAnswers.Add(answer);
            _Context.SaveChanges();
        }

        public void SaveStudentStatistics(StudentStatistics stats)
        {
            _Context.StudentStatistics.Add(stats);
            _Context.SaveChanges();
        }

        public void SaveStudentStatisticsintask(studentstaistics_in_task stats)
        {
            _Context.Studentstaistics_In_Tasks.Add(stats);
            _Context.SaveChanges();
        }
    }
}
