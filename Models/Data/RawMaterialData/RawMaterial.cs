

namespace Models.Data.RawMaterialData
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public string RawMaterialCode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
        public int MeasureUnit { get; set; }
        public int LocationId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public RawMaterial() {
            IsDeleted = false;
        }
    }

    public enum MeasureUnit
    {
        Kg=0,
        L=1,

    }
}
