using Luno_platform.Models;
using Luno_platform.Repository;

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
    }
}
