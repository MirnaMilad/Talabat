﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.Entities.OrderAggrigation
{
    public class ProductItemOrder
    {
        public ProductItemOrder(string productName, int productId, string pictureUrl)
        {
            ProductName = productName;
            ProductId = productId;
            PictureUrl = pictureUrl;
        }

        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string PictureUrl { get; set; }
    }
}
