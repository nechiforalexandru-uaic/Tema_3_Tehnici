using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShopping.Data;
using OnlineShopping.Repositories;
using OnlineShopping.Services;

namespace OnlineShopping;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }

        var productService = host.Services.GetRequiredService<IProductService>();
        var shoppingCartService = host.Services.GetRequiredService<IShoppingCartService>();
        var checkoutService = host.Services.GetRequiredService<ICheckoutService>();
        var orderService = host.Services.GetRequiredService<IOrderService>();

        await RunDemoAsync(productService, shoppingCartService, checkoutService, orderService);

        await host.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OnlineShoppingDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IProductRepository, ProductRepository>();
                services.AddScoped<ICartRepository, CartRepository>();
                services.AddScoped<IOrderRepository, OrderRepository>();

                services.AddScoped<IProductService, ProductService>();
                services.AddScoped<IShoppingCartService, ShoppingCartService>();
                services.AddScoped<ICheckoutService, CheckoutService>();
                services.AddScoped<IOrderService, OrderService>();
            });

    static async Task RunDemoAsync(
        IProductService productService,
        IShoppingCartService shoppingCartService,
        ICheckoutService checkoutService,
        IOrderService orderService)
    {
        Console.WriteLine("=== Online Shopping Application Demo ===\n");

        Console.WriteLine("1. All Products:");
        var products = await productService.GetAllProductsAsync();
        foreach (var product in products)
        {
            Console.WriteLine($"   ID: {product.Id}, Name: {product.Name}, Price: {product.Price:C}, Stock: {product.StockQuantity}");
        }

        Console.WriteLine("\n2. Adding products to cart (User ID: 1):");
        await shoppingCartService.AddToCartAsync(1, 1, 1);
        await shoppingCartService.AddToCartAsync(1, 2, 2);
        Console.WriteLine("   Added: 1 Laptop, 2 Mice");

        Console.WriteLine("\n3. Shopping Cart:");
        var cart = await shoppingCartService.GetCartByUserIdAsync(1);
        if (cart != null && cart.CartItems.Any())
        {
            foreach (var item in cart.CartItems)
            {
                Console.WriteLine($"   {item.Product.Name} x{item.Quantity} = {item.Quantity * item.Product.Price:C}");
            }
            var total = await shoppingCartService.GetCartTotalAsync(1);
            Console.WriteLine($"   Total: {total:C}");
        }

        Console.WriteLine("\n4. Checkout:");
        try
        {
            var order = await checkoutService.CheckoutAsync(1, "123 Main St, Bucharest, Romania");
            Console.WriteLine($"   Order created successfully!");
            Console.WriteLine($"   Order ID: {order.Id}");
            Console.WriteLine($"   Total Amount: {order.TotalAmount:C}");
            Console.WriteLine($"   Status: {order.Status}");

            Console.WriteLine("\n   Order Items:");
            foreach (var item in order.OrderItems)
            {
                Console.WriteLine($"   - {item.Product.Name} x{item.Quantity} = {item.Subtotal:C}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   Checkout failed: {ex.Message}");
        }

        Console.WriteLine("\n=== Demo Completed ===");
    }
}