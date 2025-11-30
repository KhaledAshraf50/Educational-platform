using System.ComponentModel.DataAnnotations;

namespace Luno_platform.Viewmodel
{
    public class ChildViewModel
    {
        public int ParentID { get; set; }
        [Required(ErrorMessage ="Required NationalID")]
        [StringLength(14,MinimumLength =14,ErrorMessage ="NationalID Must Be 14 Digit")]
        public string NationalID { get; set; }
        public string ClassName { get; set; }
        public string ImageUrl { get; set; }

    }
}
