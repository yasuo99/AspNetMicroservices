using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string Username { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public Coupon AppliedCoupon { get; set; } //Only 1 coupon can be use for order
        public ShoppingCart()
        {

        }
        public ShoppingCart(string username)
        {
            Username = username;
        }
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(i => i.Price*i.Quantity);
            }
        }
    }
}
