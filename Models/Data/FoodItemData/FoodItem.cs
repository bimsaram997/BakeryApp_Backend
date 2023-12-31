namespace Models.Data.FoodItemData
{

    public class FoodItem
    {
        public int Id { get; set; }
        public string FoodCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public string FoodDescription { get; set; }
        public double? FoodPrice { get; set; }
        public string ImageURL { get; set; }
        public int FoodTypeId { get; set; }
        public FoodType? foodType { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSold { get; set; }
        public FoodItem() {
            IsDeleted = false;
            IsSold= false;
        }

    }
}