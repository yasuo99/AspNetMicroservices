﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public int Remain { get; set; }
    }
}
