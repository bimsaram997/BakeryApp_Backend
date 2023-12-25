using Models.Data.RawMaterialData;


namespace Models.Data.RecipeData
{
    public class Recipe
    {
        public int Id { get; set; }
        public string RecipeCode { get; set; }
        public int FoodTypeId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Recipe()
        {
            IsDeleted = false;
        }
    }

}
