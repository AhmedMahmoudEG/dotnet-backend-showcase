using APIVERSION.ProductDtos.V2;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace URLPATHVERSIONINGMINIMAL.Endpoints.V2;
public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndPointsV2(this IEndpointRouteBuilder app,ApiVersionSet apiVersionSet)
    {
        var productApi = app.MapGroup("/api/products").WithApiVersionSet(apiVersionSet); 

        productApi.MapGet("{productId:guid}",GetProductById)
        .HasApiVersion(new ApiVersion(2))
        .WithName("GetPRoductByIdV2");

        return productApi;
    }

    private static Results<Ok<ProductDto>,NotFound<string>> GetProductById(Guid productId , ProductRepository repository)
    {
        var product = repository.GetProductById(productId); 
        if(product is null ) return TypedResults.NotFound($"product with this id '{productId}' not found");
        return TypedResults.Ok(ProductDto.FromModel(product));
    }
}