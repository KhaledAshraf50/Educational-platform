using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Teacher_payment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int paymentID { get; set; }
        [Required]
        public int PaymentRefId { get; set; }

        [ForeignKey("PaymentRefId")]
        public virtual Payments Payment { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public int instructorID { get; set; }

        [ForeignKey("instructorID")]
        public virtual Instructor Instructor { get; set; }

        public int AdminID { get; set; }

        [ForeignKey("AdminID")]
        public virtual Admin Admin { get; set; }





    }
}