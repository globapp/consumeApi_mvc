namespace consumeApi1.Models
{
    public class Customers
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Addby { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
