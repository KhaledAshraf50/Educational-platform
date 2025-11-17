using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Users
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string phoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(50)]
        public string nationalID { get; set; }

        [Required]
        [MaxLength(50)]
        public string role { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string email { get; set; }

        [Required]
        [MaxLength(100)]
        public string fname { get; set; }

        [MaxLength(100)]
        public string secondName { get; set; }

        [MaxLength(100)]
        public string ThirdName { get; set; }

        [Required]
        [MaxLength(100)]
        public string lastName { get; set; }


        public Student student { get; set; }
        public Instructor Instructor { get; set; }
        public Parent parent { get; set; }

     }
}

