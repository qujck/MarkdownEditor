_In this post I will outline some of the techniques I employ to manage my Commands and Queries. If you are unfamiliar with these two patterns then please see these posts [here] and [here] for a great introduction. For the remainder of this post I will assume you have fully subscribed to the idea of [parameter objects](http://refactoring.com/catalog/introduceParameterObject.html) and are comfortable with a variation of the following two abstractions._

#### Command

> Actions that change something and return nothing 

```csharp
public interface ICommand { }

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    void Run(TCommand command);
}
```

#### Query

> Actions that return something and change nothing  

```csharp
public interface IQuery<TResult> { }

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    TResult Execute(TQuery query);
}
```

## Descriptive

_Parameter objects should be descriptive; it should be easy to discover what the object is for, how to set it up, and what it will return without the need for short cut keys such as `F12`, `Alt+F12`, etc._

Given a query parameter named `GetProductDetails` we can probably have a good guess as to what it does, but the probably should give us pause for thought. Probably is a word that indicates a potential uncertainty or ambiguity. We can provide a much better insight into what the parameter object is for by adding some commenting to the definition of the class

```csharp
/// <summary>
/// Get Product Details for all the current active portals.
/// Returns IEnumerable&lt;ProductDetail&gt;
/// </summary>
public class GetProductDetails : IQuery<IEnumerable<ProductDetail>> { }
```

Which (in Visual Studio) gives us this neat little tooltip

![image](../images/GetProductDetails.png)

In a similar way we can provide more detailed information at the point the developer comes to instantiating an instance of the parameter 

```csharp
/// <summary>
/// Get Product Details for all the current active portals.
/// Returns IEnumerable&lt;ProductDetail&gt;
/// </summary>
public class GetProductDetails : IQuery<IEnumerable<ProductDetail>> 
{ 
    /// <summary>
    /// Get Product Details for the supplied type
    /// </summary>
    /// <param name="type">The type of product being queried</param>
    public GetProductDetails(ProductType productType) { }
}
```

Which will be displayed like this at the point the developer beings to `new` up an instance

![image](../images/GetProductDetails2.png)

This level of detail can be especially useful when a parameter object supports multiple sets of input criteria, i.e. when it has more than method of construction

```csharp
/// <summary>
/// Get Product Details for all the current active portals.
/// Returns IEnumerable&lt;ProductDetail&gt;
/// </summary>
public class GetProductDetails : IQuery<IEnumerable<ProductDetail>> 
{ 
    /// <summary>
    /// Get Product Details for the supplied type
    /// </summary>
    /// <param name="type">The type of product being queried</param>
    public GetProductDetails(ProductType productType) { }

    /// <summary>
    /// Get Product Details for the supplied sub type
    /// </summary>
    /// <param name="productType">The type of product being queried</param>
    /// <param name="productSubType">The subtype of the product being queried</param>
    public GetProductDetails(ProductType productType, ProductSubType productSubType) { }
}
```

![image](../images/GetProductDetails3.png)

## Discoverable

_Parameter objects should be discoverable: we should do all we can to make our commands and queries easy to find._

By default our parameters will be mixed up amongst all the other classes available from the namspaces we are `using`. You need to know the first character or two of the parameter you are searching for before you can spot it amongst the intellisense noise. 

A simple way to organise parameter objects is by their functional type (e.g. Command or Query). 

```csharp
/// ...
public static partial class Query
{
	/// ...
	public class ProductDetails : IQuery<IEnumerable<ProductDetail>> 
	{ 
	    /// ...
	    public ProductDetails(ProductType productType) { }
	}
}
```

> Using a static class in preference to a namespace offers a couple of advantages that we will cover later

After you have typed `IQueryHandler<Query` and you hit the `.` you are given a view of all the query parameter object types.

This simple grouping is good for small code bases but as the code grows into a full enterprise sized application we can very quickly find ourselves sifting through 50, 100 and more parameter objects. In anticipation of this I advise organising parameters by an additional identifying marker such as a schema or an aggregate root. 

```csharp
public static partial class Query
{
    public static partial class Product
    {
		/// ...
		public class Details : IQuery<IEnumerable<ProductDetails>> 
		{ 
		    /// ...
		    public Details(ProductType productType) { }
		}
	}
}
```

This technique of organising parameters does not need to be a long winded, in the above example we have gone from this

```csharp
IQueryHandler<GetProductDetails, IEnumerable<ProductDetail>> getProductDetails;
```

to this

```csharp
IQueryHandler<Query.ProductDetails, IEnumerable<ProductDetail>> getProductDetails;
```
to this
 
```csharp
IQueryHandler<Query.Product.Details, IEnumerable<ProductDetail>> getProductDetails;
```

#### Reducing noise

We have already done a lot to remove noise from the screen but there is one other tip I have. In addition to organising parameter objects it can also be beneficial to to tuck all of the handler implementations out of sight and we can easily do this by introducing one more static class

```csharp
public static partial class Query
{
    public static partial class Product
    {
		/// ...
		public class Details : IQuery<IEnumerable<ProductDetails>> 
		{ 
		    /// ...
		    public Details(ProductType productType) { }
		}

        internal static partial class Handlers
        {
            internal sealed class GetProductDetailsHandler : 
                IQueryHandler<GetProductDetails, IEnumerable<ProductDetail>>
            {
            }
        }
	}
}
```

All the handlers are now squirrelled away within the static `Handlers` class.

#### Organised

It is also good practice to organise the source code into folder structures that match the heirarchy of organising class names

![image](../images/GetProductDetails4.png)

## Reducing code

_We should do all we can to reduce the amount of typing required for the consumer of the API. This means more code for us up front but again I feel it is ultimately worth it._

I find this code unnecessarily verbose

```csharp
this.getProductDetails.Execute(new Query.Product.Details(ProductType.Investment));
```

But with our static `Query` class we have the perfect place to define an extension method (in the same file as the parameter object and the handler) to reduce the above to 

```csharp
this.getProductDetails.Execute(ProductType.Investment);
```

which would look something like this

```csharp
public static partial class Query
{
    /// <summary>
    /// Get Product Details for the supplied type
    /// </summary>
    /// <param name="productType">The type of product being queried</param>
    public IEnumerable<ProductDetail> Execute(
        this IQueryHandler<Product.Details, IEnumerable<ProductDetail>> handler,
        ProductType productType)
    {
        return handler.Execute(new Product.Details(productType));
    }

    public static partial class Product
    {
		/// <summary>
		/// Get Product Details for all the current active portals.
		/// Returns IEnumerable&lt;ProductDetail&gt;
		/// </summary>
        public class Details : IQuery<IEnumerable<ProductDetail>>
        {
            internal Details(ProductType productType) 
            {
                this.ProductType = productType;
            }

            public ProductType ProductType { get; set; }
        }

        internal static partial class Handlers
        {
            // ....
``` 

> NOTE: Jumping ahead slightly we should also move the comment from the parameter objects constructor to the new extension method and we can optionally make the parameter objects constructor `internal`

> WARNING: do not automatically change the parameter objects `getters` and `setters` to `internal`. Reducing property accessibility from `public` prevents these objects from being easily serlialized, which can affect things such as instance caching.  

## Summary

If you break down the guidelines in this post and shine the cold light of day on to them you can see extra code in each and every handler. 

From this

```csharp
public class GetProductDetails : IQuery<IEnumerable<ProductDetail>> 
{
    public GetProductDetails(ProductType productType) 
    {
        this.ProductType = productType;
    }

    public ProductType ProductType { get; set; }
}

public sealed class GetProductDetailsHandler :
    IQueryHandler<GetProductDetails, IEnumerable<ProductDetail>>
{
    public IEnumerable<ProductDetail> Execute(GetProductDetails query)
    {
        return Enumerable.Empty<ProductDetail>();
    }
}
```

To this

```csharp
public static partial class Query
{
    /// <summary>
    /// Get Product Details for the supplied type
    /// </summary>
    /// <param name="productType">The type of product being queried</param>
    public IEnumerable<ProductDetail> Execute(
        this IQueryHandler<Product.Details, IEnumerable<ProductDetail>> handler,
        ProductType productType)
    {
        return handler.Execute(new Product.Details(productType));
    }

    public static partial class Product
    {
		/// <summary>
		/// Get Product Details for all the current active portals.
		/// Returns IEnumerable&lt;ProductDetail&gt;
		/// </summary>
        public class Details : IQuery<IEnumerable<ProductDetail>>
        {
            internal Details(ProductType productType) 
            {
                this.ProductType = productType;
            }

            public ProductType ProductType { get; set; }
        }

        internal static partial class Handlers
        {
            internal sealed class GetProductDetailsHandler : 
                IQueryHandler<GetProductDetails, IEnumerable<ProductDetail>>
            {
                public IEnumerable<ProductDetail> Execute(GetProductDetails query)
                {
                    return Enumerable.Empty<ProductDetail>();
                }
            }
        }
    }
}
```

But we've gained a well formed and organised API in the process. It's extra code but an easier to use and neatly packaged API. IMO it's not a major overhead as I like to write code. And an API built this way can be be a real pleasure to work with.