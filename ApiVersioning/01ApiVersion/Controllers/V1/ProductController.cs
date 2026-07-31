using APIVERSION.ProductDtos.V1;
using Microsoft.AspNetCore.Mvc;

namespace APIVERSION.Controllers.V1;
[ApiController]
[ApiVersion("1.0")]
[Route("api/products")]
[Route("api/v{version:apiVersion}/products")] //tell this endpoint that you support another version
public class ProductController(ProductRepository repository)  : ControllerBase
{
    
    [HttpGet("{productId:guid}")]
    public ActionResult<Product> GetProduct(Guid productId)
    {
        Response.Headers["Deprecation"]="true";
        Response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        Response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";
        var product = repository.GetProductById(productId);
        if(product is null ) return NotFound($"product with this id '{productId}' not found");
        return Ok(ProductDto.FromModel(product));
    }
}