using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productApi = app.MapGroup("/api/products");
        productApi.MapMethods("",["OPTIONS"],OptionsProducts);
        productApi.MapMethods("{productId:guid}",["HEAD"],HeadProducts);
        productApi.MapGet("",GetPaged);
        productApi.MapGet("{productId:guid}",GetProductById).WithName(nameof(GetProductById));
        productApi.MapPost("",CreateProduct);
        productApi.MapPut("{productId:guid}",Put);
        productApi.MapPatch("{productId:guid}",Patch);
        productApi.MapDelete("{productId:guid}",Delete);
        productApi.MapPost("process",ProcessAsync);
        productApi.MapGet("status/{jobId:guid}",GetProcessinStatus); 
        productApi.MapGet("products-csv", GetProductCSV);
        productApi.MapGet("physical-file", GetPhysicalFile);

        productApi.MapGet("redirect", GetRedirect);
        productApi.MapGet("temp-products", TempProducts);

        productApi.MapGet("permanent-redirect", GetPermanentRedirect);
        productApi.MapGet("products-catalog", Catalog);
        return productApi;
    }

    private static IResult OptionsProducts(HttpResponse response)
    {
        //add the headers that will be supported 
        response.Headers.Append("Allow","GET,HEAD, PUT,DELETE,PATCH,OPTIONS");
        return Results.NoContent();
    }

    private static IResult HeadProducts(Guid productId,ProductRepository repository)
    {
        return repository.ExistsById(productId) ? Results.Ok() : Results.NotFound();
    }


    private static Results<Ok<ProductDto>, NotFound<string>> GetProductById (Guid productId,ProductRepository repository, bool includeReviews = false)
    {
        var product = repository.GetProductById(productId);
        if(product is null) return TypedResults.NotFound($"PRoduct with Id '{productId}' is not found");
        List<ProductReview>? reviews = null;
        if(includeReviews == true)
        {
            reviews = repository.GetProductReviews(productId);
        }
        return TypedResults.Ok(ProductDto.FromModel(product,reviews));
    }

    private static IResult GetPaged(ProductRepository repository, int page, int pageSize = 10)
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
        return Results.Ok(pagedResult);
    }


    private static IResult CreateProduct(CreateProductRequest request,ProductRepository repository)
    {
        if(repository.ExistsByName(request.Name))
        return Results.Conflict($"A Product with the name '{request.Name} already exists");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };
        repository.AddProduct(product);
        return Results.CreatedAtRoute(routeName:nameof(GetProductById),routeValues: new {productId = product.Id},value:ProductDto.FromModel(product));
    }


    private static IResult Put(Guid productId, UpdateProductRequest request, ProductRepository repository)
    {
        var product = repository.GetProductById(productId);
        if( product is null)
            return Results.NotFound($"Product with id {productId} not found");
        product.Name = request.Name;
        product.Price = request.Price ?? 0 ;

        var succeeded = repository.UpdateProduct(product);
        if(!succeeded)
            return Results.StatusCode(500);
        return Results.NoContent();

    }

    private static async Task<IResult> Patch(Guid productId , ProductRepository repository,HttpRequest httpRequest)
    {
        using var reader = new StreamReader(httpRequest.Body);
        var json = await reader.ReadToEndAsync();
        var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<UpdateProductRequest>>(json);
        if(patchDoc is null)
            return Results.BadRequest("Invalid patch Document.");
        
        var product = repository.GetProductById(productId);
        if(product is null)
            return Results.NotFound($"Product with Id '{productId}'  not found");

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
            return Results.StatusCode(500);
        return Results.NoContent();
    }

    private static IResult Delete(Guid productId,ProductRepository repository)
    {
        
        if(!repository.ExistsById(productId))
            return Results.NotFound($"Product with Id '{productId}'  not found");
        
        var succeeded = repository.DeleteProduct(productId);
        if(!succeeded)
            return Results.StatusCode(500);
        return Results.NoContent();
    }


    private static IResult ProcessAsync()
    {
        var jobId = Guid.NewGuid();
        return Results.Accepted (
            $"/api/products/status/{jobId}",
            new {jobId ,status= "Processing"}
        );
    }

    private static IResult GetProcessinStatus(Guid jobId)
    {
        var isSTillProcessing = false;
        return Results.Ok(new {jobId,status=isSTillProcessing ?"Processing" :"Completed"});
    }

    private static IResult GetProductCSV(ProductRepository repository)
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
        return Results.File(fileBytes,"text/csv","product-catalog1_100.csv");
    }

    private static IResult GetPhysicalFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(),"Files","products.csv");
        return TypedResults.PhysicalFile(filePath,"text/csv","products-export.csv");
    }

  
    private static IResult GetRedirect()
    {
        return Results.Redirect("/api/products/temp-products");
    }

    private static IResult TempProducts()
    {
        return Results.Ok(new {message = "You're in the temp endpoint. Chill."});
    }


    private static IResult GetPermanentRedirect()
    {
        return Results.Redirect("/api/products/products-catalog",permanent:true);
    }

    private static IResult Catalog()
    {
        return Results.Ok(new {message = "You're in the Catalog endpoint. Chill."});
    }
}