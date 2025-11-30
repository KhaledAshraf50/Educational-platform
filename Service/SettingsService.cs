using Luno_platform.Repository;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public class SettingsService
    {
        private readonly IUserRepo _userRepo;
        private readonly IstudentRepo _studentRepo;
        private readonly IWebHostEnvironment _env;
        private readonly IClassRepo _classRepo;

        public SettingsService(IUserRepo userRepo, IstudentRepo studentRepo, IWebHostEnvironment env, IClassRepo classRepo)
        {
            _userRepo = userRepo;
            _studentRepo = studentRepo;
            _env = env;
            _classRepo = classRepo;
        }

        public UserSettingsVM GetSettings(int userId)
        {
            var user = _userRepo.GetById(userId);
            var student = _studentRepo.GetByUserId(userId);
            if (student == null)
            {
                throw new Exception("هذا المستخدم ليس طالباً.");
            }


            return new UserSettingsVM
            {
                UserId = user.Id,
                Fname = user.fname,
                LastName = user.lastName,
                PhoneNumber = user.PhoneNumber,
                ClassId = student.classId,
                CurrentImage = user.Image,
                ClassName = student.Classes != null ? student.Classes.ClassName : ""
                ,
                AllClasses = _classRepo.GetAll().ToList()
            };
        }

        public void UpdateSettings(UserSettingsVM model)
        {
            var user = _userRepo.GetById(model.UserId);
            var student = _studentRepo.GetByUserId(model.UserId);

            // تعديل البيانات
            user.fname = model.Fname;
            user.lastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            student.classId = model.ClassId;

            // رفع الصورة
            if (model.ImageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "ProfileImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                user.Image = "/ProfileImages/" + fileName; // URL
            }

            _userRepo.Update(user);
            _studentRepo.Update(student);

            _userRepo.Save();
            _studentRepo.Save();
        }
    }

}
