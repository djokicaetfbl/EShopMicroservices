using BuildingBlocks.Exceptions;

namespace Catalog.API.Excepions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
        }
    }
}
