using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;

namespace BookstoreSystem
{
    public class CustomerOperations
    {
        private CustomerManager customerManager;
        public CustomerOperations(CustomerManager customerManager)
        {
            this.customerManager = customerManager;
        }

        public List<Orders> Orders { get; set; } = new List<Orders>();

        }

    }
}

