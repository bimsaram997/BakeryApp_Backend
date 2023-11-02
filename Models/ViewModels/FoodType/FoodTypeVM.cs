﻿namespace Models.ViewModels.FoodType
{
    public class FoodTypeVM
    {
       

         public int Id { get; set; }
        public string FoodTypeCode { get; set; }
        public string FoodTypeName { get; set; }
        public DateTime? AddedDate { get; set; }
        public int FoodTypeCount { get; set; }
        public string ImageURL { get; set; }
        public List<int> RawMaterialIds { get; set; }


    }


}
