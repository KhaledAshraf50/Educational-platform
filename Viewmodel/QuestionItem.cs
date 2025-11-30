using System.ComponentModel.DataAnnotations;

namespace Luno_platform.Viewmodel
{
    public class QuestionItem
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string ChooseA { get; set; }

        [Required]
        public string ChooseB { get; set; }

        [Required]
        public string ChooseC { get; set; }

        [Required]
        public string ChooseD { get; set; }

        [Required]
        public string CorrectAnswer { get; set; } // A, B, C, D
    }
}
