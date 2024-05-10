using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Orderitem:BaseEntity
    {
        public Orderitem(ProductItemOrder product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }
        public Orderitem()
        {
            
        }
        public ProductItemOrder Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }



    }
}
