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
    }
}
