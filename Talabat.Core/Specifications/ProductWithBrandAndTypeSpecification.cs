using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecifications<Product>
    {

        public ProductWithBrandAndTypeSpecification(ProductSpecParams Params) 
            : base(
                 P=> (!Params.BrandId.HasValue || P.ProductBrandId ==Params.BrandId )
                 &&
                 (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
                  &&
                 (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search) ) )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAcs":
                        AddOrderBy(P => P.Price);

                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
             
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);

        }
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {

            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }

    }
}
