namespace Luno_platform.ViewModels
{
    public class InvoiceVM
    {
        public int InvoiceId { get; set; }
        public string CourseName { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
