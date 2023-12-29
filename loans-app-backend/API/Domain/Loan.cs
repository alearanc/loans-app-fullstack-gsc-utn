using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain
{
    public class Loan
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string Status { get; set; }
        public Thing Thing { get; set; }
        public Person Person { get; set; }
    }
}
