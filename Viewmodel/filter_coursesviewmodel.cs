using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Viewmodel
{
    public class filter_coursesviewmodel
    {
        // فلاتر المستخدم
        public int? subjectid { get; set; }
        public int ? Level { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }


        public string? coursesname { get; set; }


        // البيانات اللي هنرجعها للصفحة
        public List<Courses> Courses { get; set; }
        public List<Subject>? subject { get; set; }








    }
}
