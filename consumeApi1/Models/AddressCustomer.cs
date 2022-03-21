namespace consumeApi1.Models
{
    public class AddressCustomer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Idcustomer { get; set; }
        public Int16 AddressType { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? CountryCode { get; set; }
        public string? Address { get; set; }
        public string? Adby { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
