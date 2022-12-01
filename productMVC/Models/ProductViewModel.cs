using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace productMVC.Models
{
    public class ProductViewModel
    {
        public List<ProductModel> ProductList { get; set; }
        public List<SellerModel> SellerList { get; set; }

        public static implicit operator List<object>(ProductViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}