using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string Username { get; set; }
        public List<BasketItemExendedModel> Items { get; set; } = new List<BasketItemExendedModel>();
        public decimal TotalPrice { get; set; }
    }
}
