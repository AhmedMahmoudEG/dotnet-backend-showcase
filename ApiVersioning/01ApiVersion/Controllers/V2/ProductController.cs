using APIVERSION.ProductDtos.V2;
using Microsoft.AspNetCore.Mvc;

namespace APIVERSION.Controllers.V2;
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/products")]
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