using System.Collections;

namespace APIVERSION.ProductDtos.V1;

public sealed class ProductDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public decimal Price { get; set; }

    private ProductDto (){}

    public static ProductDto FromModel(Product product)
    {
        if(product==null)
            throw new ArgumentNullException(nameof(product),"Cannot create a response from a null Product");

        var response = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
        };
        return response;
    }
    //mapping Product Dto to Product Model
    public static IEnumerable<ProductDto> FromModels(IEnumerable<Product> products)
    {
        if(products == null)
            throw new ArgumentNullException(nameof(products),"Cannot Create a response from a null Collection");

        return products.Select(p => FromModel(p));
    }
}


