namespace MyFinalProject.Models
{
    public class OrderModel
    {
        public int UserId { get; set; }
        public string CarModel { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int TypeOfBodyId { get; set; }
        public int MinMileage { get; set; }
        public int MaxMileage { get; set; }
        public double? MinEngineСapacity { get; set; }
        public double? MaxEngineСapacity { get; set; }
        public int GearboxId { get; set; }
        public int TypeOfFuelId { get; set; }
        public string? Coments { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreaded { get; set; }
        public DateTime? DateAnswered { get; set; }
        public string? Answer { get; set; }
        public int? AdminId { get; set; }
    }
}
