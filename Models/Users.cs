using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Users:IdentityUser<int>
    {



   

        [MaxLength(50)]
        public string nationalID { get; set; }

        [Required]
        [MaxLength(50)]
        public string role { get; set; }

<<<<<<< HEAD
        [Required]

        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string email { get; set; }
=======
        public string? Image { get; set; }
>>>>>>> dev

        [Required]

        [MaxLength(100)]
        public string fname { get; set; }


        [Required]
        [MaxLength(100)] 

        public string lastName { get; set; }


        public Student student { get; set; }
        public Admin admin { get; set; }

        public Instructor Instructor { get; set; }
        public Parent parent { get; set; }

     }
}

