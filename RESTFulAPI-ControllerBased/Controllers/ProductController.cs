using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpOptions]
    public IActionResult OptionsProducts()
    {
        //add the headers that will be supported 
        Response.Headers.Append("Allow","GET,HEAD, PUT,DELETE,PATCH,OPTIONS");
        return NoContent();
    }

    [HttpHead("{productId:guid}")]
    public IActionResult HeadProducts(Guid productId)
    {
        return repository.ExistsById(productId) ? Ok() : NotFound();
    }

    [HttpGet("{productId:guid}", Name = "GetProductById")]
    public ActionResult<ProductDto> GetProductById (Guid productId,bool includeReviews = false)
    {
        var product = repository.GetProductById(productId);
        if(product is null) return NotFound();
        List<ProductReview>? reviews = null;
        if(includeReviews == true)
        {
            reviews = repository.GetProductReviews(productId);
        }
        return ProductDto.FromModel(product,reviews);
    }
    [HttpGet]
    public IActionResult GetPaged(int page, int pageSize = 10)
    {
        page = Math.Max(1,page);
        pageSize= Math.Clamp(pageSize, 1, 100);
        int totalCount = repository.GetProductCount();
        var products =repository.GetProductsPage(page,pageSize);

        var pagedResult = PagedResult<ProductDto>.Create(
            ProductDto.FromModels(products),
            totalCount,
            page,
            pageSize
        );
        return Ok(pagedResult);
    }

    [HttpPost]
    public IActionResult CreateProduct(CreateProductRequest request)
    {
        if(repository.ExistsByName(request.Name))
        return Conflict($"A Product with the name '{request.Name} already exists");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };
        repository.AddProduct(product);
        return CreatedAtRoute(routeName:nameof(GetProductById),routeValues: new {productId = product.Id},value:ProductDto.FromModel(product));
    }

    [HttpPut("{productId:guid}")]
    public IActionResult Put(Guid productId, UpdateProductRequest request)
    {
        var product = repository.GetProductById(productId);
        if( product is null)
            return NotFound($"Product with id {productId} not found");
        product.Name = request.Name;
        product.Price = request.Price ?? 0 ;

        var succeeded = repository.UpdateProduct(product);
        if(!succeeded)
            return StatusCode(500,"Failed to updated Product");
        return NoContent();

    }
    [HttpPatch("{productId:guid}")]
    public IActionResult Patch(Guid productId , JsonPatchDocument<UpdateProductRequest>? patchDoc)
    {
        if(patchDoc is null)
            return BadRequest("Invalid patch Document.");
        
        var product = repository.GetProductById(productId);
        if(product is null)
            return NotFound($"Product with Id '{productId}'  not found");

        var updatedModel = new UpdateProductRequest
        {
            Name = product.Name,
            Price = product.Price
        };
        patchDoc.ApplyTo(updatedModel);
        product.Name = updatedModel.Name;
        product.Price= updatedModel.Price ?? 0;

        var succeeded = repository.UpdateProduct(product);
        if(!succeeded)
            return StatusCode(500,"Failed to updated Product");
        return NoContent();
    }

    [HttpDelete("{productId:guid}")]
    public IActionResult Delete(Guid productId)
    {
        
        if(!repository.ExistsById(productId))
            return NotFound($"Product with Id '{productId}'  not found");
        
        var succeeded = repository.DeleteProduct(productId);
        if(!succeeded)
            return StatusCode(500,"Failed to Delete Product");
        return NoContent();
    }

    [HttpPost("process")]
    public IActionResult ProcessAsync()
    {
        var jobId = Guid.NewGuid();
        return Accepted (
            $"/api/products/status/{jobId}",
            new {jobId ,status= "Processing"}
        );
    }
    [HttpGet("status/{jobId}")]
    public IActionResult GetProcessinStatus(Guid jobId)
    {
        var isSTillProcessing = false;
        return Ok(new {jobId,status=isSTillProcessing ?"Processing" :"Completed"});
    }

    [HttpGet("csv")]
    public IActionResult GetProductCSV()
    {
        var products =repository.GetProductsPage(1,100);
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("ID,Name,Price");
        foreach( var p in products)
        {
            csvBuilder.AppendLine($"{p.Id},{p.Name},{p.Price}");
        }
        //convert it to byte array
        var fileBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        return File(fileBytes,"text/csv","product-catalog1_100.csv");
    }

    [HttpGet("physical-csv-file")]
    public IActionResult GetPhysicalFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(),"Files","products.csv");
        return PhysicalFile(filePath,"text/csv","products-export.csv");
    }

    //Redirect client to specific endpoint for example redirect from legcy to temp products endpoint

    [HttpGet("products-legacy")]
    public IActionResult GetRedirect()
    {
        return Redirect("/api/products/temp-products");
    }

    [HttpGet("temp-products")]
    public IActionResult TempProducts()
    {
        return Ok(new {message = "You're in the temp endpoint. Chill."});
    }

    //permanent redirection for endpoint
    [HttpGet("products-legacy2")]
    public IActionResult GetPremanentRedirect()
    {
        return RedirectPermanent("/api/products/products-catalog");
    }

    [HttpGet("products-catalog")]
    public IActionResult Catalog()
    {
        return Ok(new {message = "You're in the Catalog endpoint. Chill."});
    }


}