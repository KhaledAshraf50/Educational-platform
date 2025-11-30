using Luno_platform.Models;
using Luno_platform.Repository;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public class Task_service: BaseService<Tasks>, ITask_service
    {
        private readonly ITask_repo _taskRepo;
        public Task_service(ITask_repo taskRepo) : base(taskRepo)
        {
            _taskRepo = taskRepo;
        }
        public List<Question> GetTaskbyid(int Examid)
        {
            return _taskRepo.GetTasksbyid(Examid);
        }

  


        public int CorrectTaskAndSave(int taskid, int studentId, Dictionary<int, string> answers)
        {
            int totalDegree = 0;

            var questions = _taskRepo.GetTasksbyid(taskid);

            foreach (var q in questions)
            {
                answers.TryGetValue(q.questionID, out string studentAnswer);

                // 1) حفظ الإجابة
                var ans = new StudentAnswer
                {
                    StudentID = studentId,
                    TaskId = taskid ,
                    QuestionId = q.questionID,
                    studentanswer = studentAnswer ?? "No Answer"
                };
                _taskRepo.SaveStudentAnswer(ans);

                // 2) التصحيح
                if (!string.IsNullOrEmpty(studentAnswer) &&
                    studentAnswer.Trim().ToLower() == q.correctAnswer.Trim().ToLower())
                {
                    totalDegree += q.degree;
                }
            }

            // 3) حفظ النتيجة
            var stats = new studentstaistics_in_task
            {
                StudentID = studentId,
                TaskId = taskid,
                degree = totalDegree
            };

            _taskRepo.SaveStudentStatistics(stats);

            return totalDegree;
        }

       
    }
}

