using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class ProductCategory : BaseEntity
    {

        #region prop
        public string Name { get; set; }
        #endregion

        #region Navigation prop
        //public ICollection<Product> Products { get; set; } = new HashSet<Product>(); 
        #endregion

    }
}
