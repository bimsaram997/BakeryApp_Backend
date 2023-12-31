namespace Models.ViewModels.FoodType
{
    public class FoodTypeVM
    {
       

         public int id { get; set; }
        public string foodTypeCode { get; set; }
        public string foodTypeName { get; set; }
        public DateTime? addedDate { get; set; }
        public string imageURL { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool isDeleted { get; set; }


    }


}
