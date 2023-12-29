namespace API.Domain
{
    public class Thing
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateOnly CreationDate { get; set; }
        public Category Category { get; set; }
    }
}
