using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Repository
{
    public interface IParentRepo : I_BaseRepository<Parent>
    {
        public List<Student> GetStds(int id);
        public List<Classes> GetClasses();

        public Student GetStudentDetails(int id);
        public Student GetStudentByNationalID(string nationalID);
        public Student GetStudentById(int id);
        public List<Student> GetStudentBelongToParent(int parentId);
        public void UpdateStudent(Student student);
        public Parent GetParent(int id);

        public Parent EditParentSetting(ParentSettingVM pVM);

        public int GetNoOfStudents(int parentId);
        public StudentProgressVM GetStudentProgress(int studentId);
        Parent GetByUserId(int userId);
        List<Payments> GetPayments(int studentId);


    }
}
