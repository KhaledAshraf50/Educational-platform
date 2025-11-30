

    namespace Luno_platform.Viewmodel
    {
        public class EditExamVM
        {
            public int ExamID { get; set; }          // معرف الامتحان
            public string ExamName { get; set; }     // اسم الامتحان
            public int ClassId { get; set; }         // الصف الدراسي
            public int subjectId { get; set; }       // المادة
            public int Time { get; set; }            // مدة الامتحان بالدقائق
            public int TotalQuestions { get; set; }  // عدد الأسئلة
            public int TotalMarks { get; set; }      // درجة الامتحان الكاملة
        }
    }

