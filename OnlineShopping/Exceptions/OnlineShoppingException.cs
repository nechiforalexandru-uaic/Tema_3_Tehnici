namespace OnlineShopping.Exceptions;

public class OnlineShoppingException : Exception
{
    public OnlineShoppingException(string message) : base(message)
    {
    }

    public OnlineShoppingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public class ProductNotFoundException : OnlineShoppingException
{
    public ProductNotFoundException(int productId)
        : base($"Product with ID {productId} was not found.")
    {
    }
}

public class InsufficientStockException : OnlineShoppingException
{
    public InsufficientStockException(string productName, int requested, int available)
        : base($"Insufficient stock for {productName}. Requested: {requested}, Available: {available}")
    {
    }
}

public class CartEmptyException : OnlineShoppingException
{
    public CartEmptyException() : base("Cart is empty. Cannot proceed to checkout.")
    {
    }
}