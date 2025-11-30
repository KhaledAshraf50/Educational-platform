using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        public DateTime date { get; set; }

        [Required]
        [MaxLength(50)]
        public string status { get; set; }

        public decimal amountPayment { get; set; }

        [Required]
        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        [Required]
        public int courseId { get; set; }

        [ForeignKey("courseId")]
        public virtual Courses Courses { get; set; }
    }
}
