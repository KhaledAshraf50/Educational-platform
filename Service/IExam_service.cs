using Luno_platform.Models;

namespace Luno_platform.Service
{
    public interface IExam_service: I_BaseService<Exams>
    {
        List<Question> GetExamsbyid(int Examid);

        int CorrectExamAndSave(int examId, int studentId, Dictionary<int, string> answers);

    }
}
