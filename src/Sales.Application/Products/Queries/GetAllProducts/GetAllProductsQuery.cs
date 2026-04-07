using MediatR;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQuery : IRequest<IReadOnlyList<ProductResponse>>
{
}
