namespace MyFinalProject.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public int TypeOfBodyId { get; set; }
        public int Mileage { get; set; }
        public double EngineСapacity { get; set; }
        public int GearboxId { get; set; }
        public int TypeOfFuelId { get; set; }
        public string Description { get; set; }
        public string Vin { get; set; }
        //public byte[] Images { get; set; }
        public IList<IFormFile> photos { get; set; }
    }
}
