using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {

        #region prop
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        #endregion

        #region ForignKey
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        #endregion

        #region Navigation prop
        public ProductBrand Brand { get; set; }        //Navigation prop [one]
        public ProductCategory Category { get; set; }   //Navigation prop [one]

        #endregion

    }
}
