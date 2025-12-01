using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class AddQuestionVM
    {
        public int ExamId { get; set; }

        public List<QuestionItem> Questions { get; set; } = new List<QuestionItem>();
    }
}
