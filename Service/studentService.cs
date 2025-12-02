using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public class studentService : BaseService<Student>, IstudentService
    {
        private readonly IstudentRepo _repository;

        public studentService(IstudentRepo repository) : base(repository)
        {
            _repository = repository;
        }

        // دالة جلب بيانات الطالب
        public Student GetStudent(int id)
        {

            var student = _repository.GetStudent(id);

            if (student == null)
                throw new InvalidOperationException("الطالب غير موجود");

            return student;
        }

        // دالة جلب كل الكورسات الخاصة بالطالب
        public List<Courses> GetStudentCourses(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح", nameof(studentId));

            return _repository.GetStudentCourses(studentId);
        }

        // دالة جلب الكورسات مع البيانات الكاملة (مثل الدرجات)
        public List<StudentCourseFullDataVM> GetStudentCoursesFullData(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح", nameof(studentId));

            return _repository.GetStudentCoursesFullData(studentId);
        }

        // دالة جلب الكورسات مع Pagination
        public List<Courses> GetStudentCourses(int studentId, int page = 1, int pageSize = 10)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح", nameof(studentId));

            if (page <= 0)
                page = 1; // إعادة للصفحة الأولى إذا الرقم غير صحيح

            if (pageSize <= 0)
                pageSize = 10; // قيمة افتراضية إذا الرقم غير صحيح

            return _repository.GetStudentCourses(studentId, page, pageSize);
        }

        public List<Payments> GetPayments(int studentId)
        {
            return _repository.GetPayments(studentId);
        }
        public int? GetStudentIdByUserId(int userId)
        {
            return _repository.GetStudentIdByUserId(userId);
        }

        public int getStudentId(int userid)
        {
            return _repository.getStudentId(userid);
        }

        public bool isSubdcrip(int studentid, int courseid)
        {
            return _repository.isSubdcrip(studentid, courseid);

        }
    }
}
