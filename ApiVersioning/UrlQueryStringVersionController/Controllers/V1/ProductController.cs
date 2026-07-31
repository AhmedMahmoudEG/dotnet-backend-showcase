using APIVERSION.ProductDtos.V1;
using Microsoft.AspNetCore.Mvc;

namespace APIVERSION.Controllers.V1;
[ApiController]
[ApiVersion("1.0")]
[Route("api/products")]
public class ProductController(ProductRepository repository)  : ControllerBase
{
    
    [HttpGet("{productId:guid}")]
    public ActionResult<Product> GetProduct(Guid productId)
    {
        var product = repository.GetProductById(productId);
        if(product is null ) return NotFound($"product with this id '{productId}' not found");
        return Ok(ProductDto.FromModel(product));
    }
}