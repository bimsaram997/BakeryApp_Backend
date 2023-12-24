

namespace Models.Data.RawMaterialData
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public string RawMaterialCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
        public RawMaterialQuantityType RawMaterialQuantityType { get; set; }
    }

    public enum RawMaterialQuantityType
    {
        Kg=0,
        L=0,

    }
}
