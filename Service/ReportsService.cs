using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public class ReportsService : IReportsService
    {

        private readonly IPaymentRepo _paymentRepo;

        public ReportsService(IPaymentRepo paymentRepo) 
        {
            _paymentRepo = paymentRepo;
        }

        public IEnumerable<Payments> GetPayments(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public (DateTime start, DateTime end) GetRange(string filter)
        {
            var today = DateTime.Today;

            return filter switch
            {
                "today" => (today, today.AddDays(1)),
                "yesterday" => (today.AddDays(-1), today),
                "last2days" => (today.AddDays(-2), today.AddDays(-1)),
                "thisMonth" => (new DateTime(today.Year, today.Month, 1),
                                new DateTime(today.Year, today.Month, 1).AddMonths(1)),
                _ => (today, today.AddDays(1))
            };
        }

        public AdminReportsViewModel GetReport(string filter)
        {
            var (start, end) = GetRange(filter);

            var payments = _paymentRepo.GetPayments(start, end).ToList();

            // Summary
            int totalSold = payments.Count;
            decimal revenue = payments.Sum(p => p.amountPayment);
            decimal profit = revenue * 0.68m;

            // Top Instructors
            var topInstructors = payments
                .GroupBy(p => p.Courses.instructorID)
                .Select(g => new TopInstructorVM
                {
                    InstructorName = g.First().Courses.Instructor.User.fname + " " +
                                     g.First().Courses.Instructor.User.lastName,
                    TotalSold = g.Count(),
                    Revenue = g.Sum(x => x.amountPayment)
                })
                .OrderByDescending(x => x.TotalSold)
                 .Take(10)
                .ToList();

            // Top Courses
            var topCourses = payments
                .GroupBy(p => p.courseId)
                .Select(g => new TopCourseVM
                {
                    CourseName = g.First().Courses.CourseName,
                    TotalSold = g.Count(),
                    Revenue = g.Sum(x => x.amountPayment)
                })
                .OrderByDescending(x => x.TotalSold)
                 .Take(10)
                .ToList();

            return new AdminReportsViewModel
            {
                SelectedFilter = filter,
                TotalCoursesSold = totalSold,
                TotalRevenue = revenue,
                ProfitRate = profit,
                TopInstructors = topInstructors,
                TopCourses = topCourses
            };
        }

    }
}
