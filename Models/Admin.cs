using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string SecondName { get; set; }

        [MaxLength(100)]
        public string ThirdName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string email { get; set; }

        [Required]
        public string password { get; set; } // يُخزن هنا الـ Hash لكلمة المرور

        [MaxLength(50)]
        public string NationalID { get; set; }

        public DateTime? birthdate { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string FullNumber { get; set; }


        public string Image { get; set; }


        public virtual  ICollection<Teacher_payment> Teacher_Payments { get; set; } 

    }
}