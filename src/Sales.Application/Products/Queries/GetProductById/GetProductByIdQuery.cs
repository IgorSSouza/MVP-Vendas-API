using MediatR;
using Sales.Application.Products.Common;

namespace Sales.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQuery : IRequest<ProductResponse?>
{
    public Guid Id { get; set; }
}
