using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecififcation
{
    public class ProductWithFilterForCountSpec : BaseSpecification<Product>
    {
        public ProductWithFilterForCountSpec(ProductSpecParams spec) 
            : base(p=>
                   (!spec.BrandId.HasValue || p.BrandId == spec.BrandId.Value) &&
                  (!spec.CategoryId.HasValue || p.CategoryId == spec.CategoryId.Value)
            )
                   
        {
            
        }
    }
}
