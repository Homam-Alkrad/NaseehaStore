namespace NaseehaStore.Models.ViewModels
{
    public class OrderInfoViewModel
    {
        public int OrderId { get; set; }
        public string StudentName { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusMessage { get; internal set; }
    }
}
