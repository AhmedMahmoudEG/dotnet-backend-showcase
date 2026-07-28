public class ProductReviewDto
{
    public Guid ReviewId { get; set; }
    public Guid ProductId { get; set; }
    public string? Reviewr { get; set; }
    public int Stars { get; set; }

    private ProductReviewDto(){}
    public static ProductReviewDto FromModel(ProductReview? review)
    {
        if(review ==null)
            throw new ArgumentNullException(nameof(review),"Cannot Create a response from a null review");
        
        return new ProductReviewDto
        {
            ReviewId =review.Id,
            ProductId = review.ProductId,
            Reviewr = review.Reviewr,
            Stars = review.Stars
        };
    }
    public static IEnumerable<ProductReviewDto> FromModels(IEnumerable<ProductReview> reviews)
    {
        if(reviews ==null)
            throw new ArgumentNullException(nameof(reviews),"Cannot Create a response from a null review");

            return reviews.Select(FromModel);
    }
}