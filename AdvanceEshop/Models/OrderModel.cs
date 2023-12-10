namespace AdvanceEshop.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }

    public class CapturedOrderModel : OrderModel
    {
        public string ReferenceId { get; set; }
        public decimal AmountCaptured { get; set; }
        public string CurrencyCode { get; set; }
    }
}
