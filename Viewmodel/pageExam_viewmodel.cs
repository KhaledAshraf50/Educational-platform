using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class pageExam_viewmodel
    {
        public List<Question> questions { get; set; }
        public Exams examinfo { get; set; }
        public  bool issubscrip { get; set; }
        public int courseID { get; set; }
    }
}
