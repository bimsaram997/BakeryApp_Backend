﻿

namespace Models.Data.RawMaterialData
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public string RawMaterialCode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
        public RawMaterialQuantityType RawMaterialQuantityType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public RawMaterial() {
            IsDeleted = false;
        }
    }

    public enum RawMaterialQuantityType
    {
        Kg=0,
        L=1,

    }
}
