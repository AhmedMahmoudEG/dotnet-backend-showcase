
namespace APIVERSION.ProductDtos.V2;
public sealed class ProductDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public PriceDto Price {get;set;} = null;
    private ProductDto() { }

    public static ProductDto FromModel(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var response = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = new PriceDto
            {
                Amount = product.Price,
                Currency = "USD"
            }
        };
        return response;
    }
    //mapping Product Dto to Product Model
    public static IEnumerable<ProductDto> FromModels(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Select(p=>FromModel(p));
    }
}


