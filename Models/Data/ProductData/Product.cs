using Models.Data.ProductData;

namespace Models.Data.ProductData
{

    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ProductDescription { get; set; }
        public double? ProductPrice { get; set; }
        public string ImageURL { get; set; }
        public int FoodTypeId { get; set; }
        public FoodType? foodType { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSold { get; set; }
        public long BatchId { get; set; }
        public ICollection<BatchProduct> BatchProduct { get; set; }
        public Product() {
            IsDeleted = false;
            IsSold= false;
        }

    }
}