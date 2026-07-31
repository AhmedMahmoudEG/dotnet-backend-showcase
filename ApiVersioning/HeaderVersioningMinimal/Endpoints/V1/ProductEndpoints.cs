
using APIVERSION.ProductDtos.V1;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace URLPATHVERSIONINGMINIMAL.Endpoints.V1;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndPointsV1(this IEndpointRouteBuilder app,ApiVersionSet apiVersionSet)
    {

        var productApi = app.MapGroup("/api/products")
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(new ApiVersion(1,0));
       
        productApi.MapGet("{productId:guid}",GetProductById).WithName("GetProductByIdV1");

        return productApi;
    }

    private static Results<Ok<ProductDto>,NotFound<string>> GetProductById(Guid productId , ProductRepository repository, HttpResponse response)
    {
        var product = repository.GetProductById(productId);
        if(product is null ) return TypedResults.NotFound($"product with this id '{productId}' not found");
        response.Headers["Deprecation"]="true";
        response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";
        return TypedResults.Ok(ProductDto.FromModel(product));
    }
}