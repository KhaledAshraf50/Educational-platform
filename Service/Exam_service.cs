using Luno_platform.Models;
using Luno_platform.Repository;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public class Exam_service: BaseService<Exams>, IExam_service
    {
        private readonly IExam_repo _examRepo;
        public Exam_service(IExam_repo examRepo) : base(examRepo)
        {
            _examRepo = examRepo;
        }
        public List<Question> GetExamsbyid(int Examid)
        {
            return _examRepo.GetExamsbyid(Examid);
        }

        //public bool HasStudentTakenExam(int studentId, int examId)
        //{
        //    return Repo.StudentStatistics.Any(s => s.StudentID == studentId && s.ExamId == examId);
        //}


        public int CorrectExamAndSave(int examId, int studentId, Dictionary<int, string> answers)
        {
            int totalDegree = 0;

            var questions = _examRepo.GetExamsbyid(examId);

            foreach (var q in questions)
            {
                answers.TryGetValue(q.questionID, out string studentAnswer);

                // 1) حفظ الإجابة
                var ans = new StudentAnswer
                {
                    StudentID = studentId,
                    ExamId = examId,
                    QuestionId = q.questionID,
                    studentanswer = studentAnswer ?? "No Answer"
                };
                _examRepo.SaveStudentAnswer(ans);

                // 2) التصحيح
                if (!string.IsNullOrEmpty(studentAnswer) &&
                    studentAnswer.Trim().ToLower() == q.correctAnswer.Trim().ToLower())
                {
                    totalDegree += q.degree;
                }
            }

            // 3) حفظ النتيجة
            var stats = new StudentStatistics
            {
                StudentID = studentId,
                ExamId = examId,
                degree = totalDegree
            };

            _examRepo.SaveStudentStatistics(stats);

            return totalDegree;
        }


    }
}
