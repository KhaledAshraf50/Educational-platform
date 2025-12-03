using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Models
{
    public class LunoDBContext : IdentityDbContext<Users, ApplicationRole, int>
    {
        public LunoDBContext(DbContextOptions<LunoDBContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public  DbSet<Tasks> Tasks { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentStatistics> StudentStatistics { get; set; }
        public DbSet<studentstaistics_in_task> Studentstaistics_In_Tasks { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Parent> Parents { get; set; }

        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher_payment> Teacher_Payments { get; set; }
        public DbSet<instructor_classescs> instructor_classescs { get; set; }
        public DbSet<Subject_Classes> Subject_Classes { get; set; }
        //public DbSet<Exams_contentcs> Exams_Contentcs { get; set; }
        //public DbSet<Task_content> Task_Contents { get; set; }

        public DbSet<Student_Courses> Student_Courses { get; set; }
        

        //public DbSet<Teacher_payment> Teacher_Payments { get; set; }
        //public DbSet<instructor_classescs> instructor_classescs { get; set; }
        //public DbSet<Subject_Classes> Subject_Classes { get; set; }

        //public DbSet<Student_Courses> Student_Courses { get; set; }









        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            modelBuilder.Entity<Courses>()
    .Property(c => c.CourseId)
    .ValueGeneratedOnAdd();

            modelBuilder.Entity<Users>()
           .HasIndex(u => u.Email)
           .IsUnique();

            modelBuilder.Entity<Users>()
.HasIndex(u => u.nationalID)
.IsUnique();


            // =============================
            // مفاتيح مركّبة
            // =============================

            modelBuilder.Entity<instructor_classescs>()
    .HasKey(ic => new { ic.instructorId, ic.classId });

            modelBuilder.Entity<Subject_Classes>()
.HasKey(ic => new { ic.classId, ic.subjectId });


            modelBuilder.Entity<Student_Courses>()
    .HasKey(sc => new { sc.StudentId, sc.CourseId });

            // =============================
            //           العلاقات
            // =============================

            // علاقة One-to-Many بين Payments و Teacher_payment
            modelBuilder.Entity<Teacher_payment>()
                .HasOne(tp => tp.Payment)
                .WithMany(tp=> tp.Teacher_Payments) // لو عايز كل Payment يعرف الـ Teacher_payments بتاعته، خليها WithMany(tp => tp.TeacherPayments)
                .HasForeignKey(tp => tp.PaymentRefId)
                .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<Instructor>()
            .HasOne(i => i.Subject)
            .WithMany(s => s.Instructors)
            .HasForeignKey(i => i.SubjectID)
            .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Courses>()
           .HasOne(i => i.classes)
           .WithMany(s => s.Courses)
           .HasForeignKey(i => i.classID)
           .OnDelete(DeleteBehavior.NoAction);

            // 2. Instructors -> Users (UserId) - One-to-One/Zero
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.User)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 2. admin -> Users (UserId) - One-to-One/Zero
            modelBuilder.Entity<Admin>()
                .HasOne(i => i.User)
                .WithOne(u => u.admin)
                .HasForeignKey<Admin>(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 3. Parents -> Users (UserId) - One-to-One
            modelBuilder.Entity<Parent>()
                .HasOne(p => p.User)
                .WithOne(u => u.parent)
                .HasForeignKey<Parent>(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 4. Courses -> CourseContents (contentId) - One-to-One
            modelBuilder.Entity<CourseContent>()
                .HasOne(c => c.courses)
                .WithOne(cc => cc.CourseContent)
                .HasForeignKey<Courses>(c => c.CourseId)
                .OnDelete(DeleteBehavior.NoAction);//----------------------------------------------------------------------------

            // 5. Courses -> Instructors (instructorID)
            modelBuilder.Entity<Courses>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.courses)
                .HasForeignKey(c => c.instructorID)
                .OnDelete(DeleteBehavior.NoAction);

            // 6. Courses -> Subjects (SubjectId)
            modelBuilder.Entity<Courses>()
                .HasOne(c => c.Subjects)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // 7. Exams -> Classes (ClassId)
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Classess)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.NoAction);


            // 8. Exams -> Instructors (instructorID)
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Instructor)
                .WithMany(i => i.Exams)
                .HasForeignKey(e => e.instructorID)
                .OnDelete(DeleteBehavior.NoAction);

            // 9. Exams -> Subjects (subjectId)
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Subject)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.subjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // 10. Teacher_payment -> Instructors (instructorID)
            modelBuilder.Entity<Teacher_payment>()
                .HasOne(tp => tp.Instructor)
                .WithMany(i => i.Teacher_Payments)
                .HasForeignKey(tp => tp.instructorID)
                .OnDelete(DeleteBehavior.NoAction);


            // 11. Students -> Classes (classId)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Classes)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.classId)
                .OnDelete(DeleteBehavior.NoAction);

            // 12. Students -> Parents (ParentId)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Parent)
                .WithMany(p => p.Students)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            // 13. Students -> Users (UserId) - One-to-One/Zero
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 14. Payments -> Courses (courseId)
            modelBuilder.Entity<Payments>()
                .HasOne(p => p.Courses)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.courseId)
                .OnDelete(DeleteBehavior.NoAction);

            // 15. Payments -> Students (StudentID)
            modelBuilder.Entity<Payments>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            // 16. Tasks -> Classes (ClassId)
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Classes)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ClassId)
                .OnDelete(DeleteBehavior.NoAction);

            // 17. Teacher_payment -> Admins (AdminID)
            modelBuilder.Entity<Teacher_payment>()
                .HasOne(tp => tp.Admin)
                .WithMany(a => a.Teacher_Payments)
                .HasForeignKey(tp => tp.AdminID)
                .OnDelete(DeleteBehavior.NoAction);

            // 18. Tasks -> Instructors (instructorId)
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Instructor)
                .WithMany(i => i.Tasks)
                .HasForeignKey(t => t.instructorId)
                .OnDelete(DeleteBehavior.NoAction);

            // 19. Questions -> Exams (ExamId)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Exams)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            // 20. Questions -> MCQanswers (MCQAnswerId) - One-to-One
            //modelBuilder.Entity<Question>()
            //    .HasOne(q => q.MCQanswer)
            //    .WithOne(a => a.Question)
            //    .HasForeignKey<Question>(q => q.AnswerId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // 21. Questions -> Tasks (TaskId)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Tasks)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            // 22. StudentAnswers -> Exams (ExamId)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Exams)
                .WithMany(e => e.StudentAnswers)
                .HasForeignKey(sa => sa.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            // 23. StudentAnswers -> Questions (QuestionId)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            // 24. StudentAnswers -> Students (StudentID)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Student)
                .WithMany(s => s.StudentAnswers)
                .HasForeignKey(sa => sa.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            // 25. StudentAnswers -> Tasks (TaskId)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Tasks)
                .WithMany(t => t.StudentAnswers)
                .HasForeignKey(sa => sa.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            // 26. StudentStatistics -> Exams (ExamId)
            modelBuilder.Entity<StudentStatistics>()
                .HasOne(ss => ss.Exams)
                .WithMany(e => e.StudentStatistics)
                .HasForeignKey(ss => ss.ExamId)
                .OnDelete(DeleteBehavior.NoAction);


            // 26. studentstaistics_in_task -> task (taskId)
            modelBuilder.Entity<studentstaistics_in_task>()
                .HasOne(ss => ss.Tasks)
                .WithMany(e => e.Studentstaistics_In_task)
                .HasForeignKey(ss => ss.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            // 27. StudentStatistics -> Students (StudentID)
            modelBuilder.Entity<studentstaistics_in_task>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.Studentstaistics_In_task)
                .HasForeignKey(ss => ss.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            // 27. StudentStatistics -> Students (StudentID)
            modelBuilder.Entity<StudentStatistics>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentStatistics)
                .HasForeignKey(ss => ss.StudentID)
                .OnDelete(DeleteBehavior.NoAction);

            // 28. StudentStatistics -> Tasks (TaskId)
            modelBuilder.Entity<StudentStatistics>()
                .HasOne(ss => ss.Tasks)
                .WithMany(t => t.StudentStatistics)
                .HasForeignKey(ss => ss.TaskId)
                .OnDelete(DeleteBehavior.NoAction);




            // 29. instructor_classescs -> Instructors (instructorID)
            modelBuilder.Entity<instructor_classescs>()
                .HasOne(ic => ic.Instructor)
                .WithMany(i => i.instructor_classescs)
                .HasForeignKey(ic => ic.instructorId)
                .OnDelete(DeleteBehavior.NoAction);

            // 30. instructor_classescs -> Classes (ClassesClassID)
            modelBuilder.Entity<instructor_classescs>()
                .HasOne(ic => ic.classes)
                .WithMany(c => c.instructor_classescs)
                .HasForeignKey(ic => ic.classId)
                .OnDelete(DeleteBehavior.NoAction);



            // 31. Student_Courses -> Students (StudentID)
            modelBuilder.Entity<Student_Courses>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Student_Courses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            // 32. Student_Courses -> Courses (CourseId)
            modelBuilder.Entity<Student_Courses>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.Student_Courses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // 33. Subject_Classes -> Subjects (SubjectID)
            modelBuilder.Entity<Subject_Classes>()
                .HasOne(sc => sc.Subject)
                .WithMany(s => s.Subject_Classes)
                .HasForeignKey(sc => sc.subjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // 34. Subject_Classes -> Classes (ClassesClassID)
            modelBuilder.Entity<Subject_Classes>()
                .HasOne(sc => sc.Classes)
                .WithMany(c => c.Subject_Classes)
                .HasForeignKey(sc => sc.classId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<CourseContent>()
    .HasOne(p => p.Exams)
    .WithOne(u => u.CourseContent)
    .HasForeignKey<CourseContent>(p => p.ExamId)
    .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<CourseContent>()
.HasOne(p => p.Tasks)
.WithOne(u => u.CourseContent)
.HasForeignKey<CourseContent>(p => p.taskId)
.OnDelete(DeleteBehavior.NoAction);

        }
    }
    
    }
