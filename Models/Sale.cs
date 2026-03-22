public class Sale
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Total { get; set; }

    public List<SaleDetail> Details { get; set; }

    public Sale(int userId, List<SaleDetail> details)
    {
        UserId = userId;
        Details = details;
    }
}