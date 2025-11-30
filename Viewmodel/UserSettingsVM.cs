using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class UserSettingsVM
    {
        public int UserId { get; set; }

        public string Fname { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public string? CurrentImage { get; set; }
        public IFormFile? ImageFile { get; set; }

        public List<Classes> AllClasses { get; set; }
        //public ChangePasswordVM PasswordModel { get; set; } = new ChangePasswordVM();
    }

}
