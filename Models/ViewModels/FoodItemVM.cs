namespace Models.Data
{
    public class FoodItemVM
    {
        public int Id { get; set; }
        public string FoodCode { get; set; }
        public string FoodName { get; set; }
        public DateTime? AddedDate { get; set; }
        public string FoodDescription { get; set; }
        public double? FoodPrice { get; set; }
        public string ImageURL { get; set; }
    }

    
}
