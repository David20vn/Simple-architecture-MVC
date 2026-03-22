public class SaleDetail
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }

    public SaleDetail(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}