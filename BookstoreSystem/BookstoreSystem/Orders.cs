using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreSystem
{
    public class Orders
    {

        public int OrderID { get; set; }
        public List<Items> Items { get; set; }
        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public double TotalPrice { get; set; }

        // Constructor
        public Orders(int orderId, int customerId, List<Items> items, DateTime orderDate, string status, double totalPrice)
        {
            OrderID = orderId;
            CustomerID = customerId;
            Items = items;
            OrderDate = orderDate;
            Status = status;
            TotalPrice = totalPrice;
        }

        public class OrdersManager
        {

            public List<Orders> AllOrders { get; } = new List<Orders>(); // List to store all orders
            // Method to initialize orders
            public static void InitializeOrders(Customer customer)
            {
                customer.Orders.Add(new Orders(1, customer.CustomerID, new List<Items>(), DateTime.Now, "Pending", 0));
                customer.Orders.Add(new Orders(2, customer.CustomerID, new List<Items>(), DateTime.Now, "Pending", 0));
                customer.Orders.Add(new Orders(3, customer.CustomerID, new List<Items>(), DateTime.Now, "Pending", 0));
                customer.Orders.Add(new Orders(4, customer.CustomerID, new List<Items>(), DateTime.Now, "Pending", 0));
                customer.Orders.Add(new Orders(5, customer.CustomerID, new List<Items>(), DateTime.Now, "Pending", 0));
            }


            // Method to display orders by order ID
            public void DisplayOrdersByOrderID(int orderId)
            {
                var order = AllOrders.FirstOrDefault(o => o.OrderID == orderId);

                if (order != null)
                {
                    Console.WriteLine($"Order ID: {order.OrderID}");
                    Console.WriteLine($"Customer ID: {order.CustomerID}");
                    Console.WriteLine($"Order Date: {order.OrderDate}");
                    Console.WriteLine($"Status: {order.Status}");
                    Console.WriteLine($"Total Price: {order.TotalPrice:C2}");
                    // Display other order details as needed
                }
                else
                {
                    Console.WriteLine($"Order with ID {orderId} not found.");
                }

            }


        }
    }
}
