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
        Response.Headers["Deprecation"]="true";
        Response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        Response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";
        return Ok(ProductDto.FromModel(product));
    }
}