using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Models
{
    public class PaymentResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PurchasedAt { get; set; }
    }
}
