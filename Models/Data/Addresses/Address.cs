

namespace Models.Data.Address
{
    public class Addresses
    {
        public int Id { get; set; }
        public string FullAddress { get; set; }
        public string Street1 { get; set; }
        public string? Street2 { get; set; }
        public string City { get; set; }
        public int Country { get; set; }
        public int PostalCode { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public Addresses()
        {
            IsDeleted = false;
    
        }
    }
}
