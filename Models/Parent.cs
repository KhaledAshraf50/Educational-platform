using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Parent
    {
        [Key]
       
        public int ID { get; set; }
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public virtual ICollection<Student> Students { get; set; }  

    }
}
