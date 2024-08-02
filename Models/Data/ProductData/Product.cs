using Models.Data.ProductData;

namespace Models.Data.ProductData
{

    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int Unit { get; set; }
        public int CostCode { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public int RecipeId { get; set; }
        public string ProductDescription { get; set; }
        public string ImageURL { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSold { get; set; }
        public double Weight { get; set; }
        public int Status { get; set; }
        public int DaysToExpires { get; set; }   
        public int ReOrderLevel { get; set; }   
        public Product() {
            IsDeleted = false;
            IsSold= false;
        }

    }
}