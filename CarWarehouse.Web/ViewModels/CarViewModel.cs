namespace CarWarehouse.Web.ViewModels
{
    public class CarViewModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
