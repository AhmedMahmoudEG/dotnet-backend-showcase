using Microsoft.Win32;

public class ProductRepository {
private List<Product> _products =
[
    new Product { Id = Guid.Parse("5e67f8a0-9c0d-1e2f-3a4b-cdef12345678"), Name = "Rice", Price = 8.99m },
    new Product { Id = Guid.Parse("6f7a8b9c-0d1e-2f3a-4b5c-def123456789"), Name = "Pasta", Price = 3.49m },
    new Product { Id = Guid.Parse("7a8b9c0d-1e2f-3a4b-5c6d-ef1234567890"), Name = "Apple", Price = 0.79m },
    new Product { Id = Guid.Parse("8b9c0d1e-2f3a-4b5c-6d7e-f1234567890a"), Name = "Banana", Price = 0.59m },
    new Product { Id = Guid.Parse("9c0d1e2f-3a4b-5c6d-7e8f-234567890abc"), Name = "Orange", Price = 0.99m },
    new Product { Id = Guid.Parse("abcdef12-3456-7890-abcd-ef1234567890"), Name = "Grapes", Price = 2.99m }
];
/*

private List<ProductReview> _reviews =
[
    new ProductReview
    {
        Id = Guid.Parse("ddd4e07a-4772-47f7-9cba-6bfc07c26bfe"),
        ProductId = Guid.Parse("5e67f8a0-9c0d-1e2f-3a4b-cdef12345678"),
        Reviewr = "Ahmed",
        Stars = 5
    },
    new ProductReview
    {
        Id = Guid.Parse("c30d9647-1603-4948-8266-88a850547be0"),
        ProductId = Guid.Parse("5e67f8a0-9c0d-1e2f-3a4b-cdef12345678"),
        Reviewr = "Sara",
        Stars = 4
    },
    new ProductReview
    {
        Id = Guid.Parse("b14ef8f3-52d6-4cb5-9a90-0d73dc5d5b4c"),
        ProductId = Guid.Parse("6f7a8b9c-0d1e-2f3a-4b5c-def123456789"),
        Reviewr = "John",
        Stars = 5
    },
    new ProductReview
    {
        Id = Guid.Parse("aa4cfa8d-ef6b-49d9-86f8-caf2bb1c8793"),
        ProductId = Guid.Parse("7a8b9c0d-1e2f-3a4b-5c6d-ef1234567890"),
        Reviewr = "Mona",
        Stars = 3
    },
    new ProductReview
    {
        Id = Guid.Parse("3f65a7f4-7b2e-42c9-b8fd-ec2d96ef89b1"),
        ProductId = Guid.Parse("8b9c0d1e-2f3a-4b5c-6d7e-f1234567890a"),
        Reviewr = "Ali",
        Stars = 4
    },
    new ProductReview
    {
        Id = Guid.Parse("7d2d02c8-bf89-43a6-a5a2-3f4a4c5c66d1"),
        ProductId = Guid.Parse("9c0d1e2f-3a4b-5c6d-7e8f-234567890abc"),
        Reviewr = "Emily",
        Stars = 5
    },
    new ProductReview
    {
        Id = Guid.Parse("98fce4b5-64d5-4a3f-96a6-5c7d0dbeb8c3"),
        ProductId = Guid.Parse("abcdef12-3456-7890-abcd-ef1234567890"),
        Reviewr = "Omar",
        Stars = 4
    }
];
*/

    public List<Product> GetProductsPage(int page =1, int pageSize = 10)
    {
        var products = _products.Skip((page-1)*pageSize).Take(pageSize).ToList();
        return products;
    }

    public Product? GetProductById(Guid productId)
    {
        var product = _products.FirstOrDefault(p=>p.Id==productId);
        if(product is null) 
            return null;
        return product;
    }


    public bool AddProduct(Product product)
    {
        _products.Add(product);
        return true;
    }


    public bool UpdateProduct(Product updatedProduct)
    {
        var existingProduct= _products.FirstOrDefault(p=>p.Id==updatedProduct.Id);
        if(existingProduct==null) return false;
        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return true;
    }
    public bool DeleteProduct(Guid id)
    {
        var product= _products.FirstOrDefault(p=>p.Id ==id);
        if(product is null) return false;
        _products.Remove(product);
        //_reviews.RemoveAll(r=>r.ProductId==id);
        return true;
    }
    public int GetProductCount() => _products.Count();
    public bool ExistsById(Guid id) => _products.Any(p=>p.Id ==id);

    public bool ExistsByName(string? name) =>_products.Any(p=>string.Equals(p.Name ,name, StringComparison.OrdinalIgnoreCase));
}