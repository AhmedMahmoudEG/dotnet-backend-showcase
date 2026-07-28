public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Reviewr { get; set; }
    public int Stars { get; set; }
}