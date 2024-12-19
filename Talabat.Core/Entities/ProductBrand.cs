using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class ProductBrand : BaseEntity
    {

        #region Prop
        public string Name { get; set; }
        #endregion

        #region Navigation prop
        public ICollection<Product> Prducts { get; set; } = new HashSet<Product>(); 
        #endregion

    }
}
