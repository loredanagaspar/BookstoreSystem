using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;

namespace BookstoreSystem
{
    public class Customer : User
    {
        // Properties
        public int CustomerID { get; }
        public List<Orders> Orders { get; }

        // Constructor
        public Customer(int customerId, string name, string username, string password, string email, string address, string city, string region, string postCode, string country)
            : base(username, password, email, name, address, city, region, postCode, country)
        {
            CustomerID = customerId;
            Orders = new List<Orders>();
        }

        // Method to update account details
        public void UpdateAccountDetails(string newName, string newEmail, string newAddress, string newCity, string newRegion, string newPostCode, string newCountry)
        {
            Name = newName;
            Email = newEmail;
            Address = newAddress;
            City = newCity;
            Region = newRegion;
            PostCode = newPostCode;
            Country = newCountry;
        }

      
        // Method to place an order
        public void PlaceOrder(Orders order)
        {
            if (order != null)
            {
                Orders.Add(order);
            }
            else
            {
                Console.WriteLine("Cannot place a null order.");
            }
        }
    }

    public class CustomerManager
    {
        public List<Customer> Customers { get; } = new List<Customer>();
        // Constructor to add customers to the list
        public CustomerManager()
        {
            InitializeCustomers();
        }

        private void InitializeCustomers()
        {
            Customers.Add(new Customer(6, "Tom Smith", "user6", "password6", "user6@bookstore.com", "49 The Vale", "London", "London", "N1 5AH", "UK"));
            Customers.Add(new Customer(7, "Mike Thorne", "user7", "password7", "user7@bookstore.com", "150 Ozxford Street", "London", "London", "E1 3GB", "UK"));
            Customers.Add(new Customer(8, "Anna Maria", "user8", "password8", "user8@bookstore.com", "30 Piccadilly Road", "London", "London", "NW10 3GB", "UK"));
            Customers.Add(new Customer(9, "Laura Stoica", "user9", "password9", "user9@bookstore.com", "01 Downing Street", "London", "London", "W3 3QE", "UK"));
            Customers.Add(new Customer(10, "George Michael", "user10", "password10", "user10@bookstore.com", "13 Primrose Road", "London", "London", "E17 2DF", "UK"));
        }

        // Method to register a new customer
        public bool RegisterCustomer(int customerId, string name, string username, string password, string email, string address, string city, string region, string postCode, string country)
        {
            if (IsUsernameTaken(username))
            {
                Console.WriteLine("Username already exists. Please choose a different username.");
                return false;
            }

            Customer newCustomer = new Customer(customerId, name, username, password, email, address, city, region, postCode, country);
            Customers.Add(newCustomer);

            Console.WriteLine("Registration successful. You can now log in as a customer.");
            return true;
        }

        // Method to authenticate a customer
        public bool AuthenticateCustomer(string username, string password)
        {
            Customer customer = Customers.Find(c => c.Username == username);

            if (customer != null && customer.Password == password)
            {
                Console.WriteLine("Customer login successful. Welcome, " + customer.Username + "!");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                return false;
            }
        }

        // Method to check if a username is already taken
        private bool IsUsernameTaken(string username)
        {
            return Customers.Exists(c => c.Username == username);
        }

        public bool CustomerExists(int customerId)
        {
            bool ret = false;
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBCustomer dBCustomer = new DBCustomer();
            var customer = dBCustomer.RetrieveCustomer(conn, customerId);
            conn.Close();
            if (customer != null)
            {
                return true;
            }
            return Customers.Any(c => c.CustomerID == customerId);
        }

        // Method to display all customers
        public bool DisplayAllCustomers()
        {
            bool ret = false;
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBCustomer dBCustomer = new DBCustomer();
            var customers = dBCustomer.RetreiveCustomers(conn);
            conn.Close();
            if (Customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return true;
            }
            else
            {
                Console.WriteLine("List of Customers:");
                foreach (DBCustomer cus in customers)
                {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"Customer ID: {cus.CustomerID}");
                    Console.WriteLine($"Name: {cus.Name}");
                    Console.WriteLine($"Username: {cus.Username}");
                    Console.WriteLine($"Email: {cus.Email}");
                    Console.WriteLine($"Address: {cus.Address}");
                    Console.WriteLine($"City: {cus.City}");
                    Console.WriteLine($"Region: {cus.Region}");
                    Console.WriteLine($"Post Code: {cus.PostCode}");
                    Console.WriteLine($"Country: {cus.Country}");
                    Console.WriteLine("---------------------------------");
                }
                ret = true;
            }
            return ret;
        }        

        /// <summary>
        /// Displays one customer by their id number
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DisplayCustomer(int customerId)
        {
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBCustomer dBCustomer = new DBCustomer();
            var cus = dBCustomer.RetrieveCustomer(conn, customerId);
            conn.Close();
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"Customer ID: {cus.CustomerID}");
            Console.WriteLine($"Name: {cus.Name}");
            Console.WriteLine($"Username: {cus.Username}");
            Console.WriteLine($"Email: {cus.Email}");
            Console.WriteLine($"Address: {cus.Address}");
            Console.WriteLine($"City: {cus.City}");
            Console.WriteLine($"Region: {cus.Region}");
            Console.WriteLine($"Post Code: {cus.PostCode}");
            Console.WriteLine($"Country: {cus.Country}");
            Console.WriteLine("---------------------------------");
            return true;
        }
    
        // Method to add a new order for a customer
        public void AddOrderForCustomer(int customerId, Orders order)
        {
            var customer = Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer != null)
            {
                customer.PlaceOrder(order);
            }
            else
            {
                Console.WriteLine($"Customer with ID {customerId} not found.");
            }
        }

        public bool UpdateCustomer(int customerID, string email, string address, string city, string region, string postcode, string country)
        {
            bool ret = false;
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBCustomer dBCustomer = new DBCustomer();
            var customer = dBCustomer.RetrieveCustomer(conn,customerID);
            customer.Email = email;
            customer.Address = address;
            customer.City = city;
            customer.Region = region;
            customer.PostCode = postcode;
            customer.Country = country;
            ret = dBCustomer.UpdateCustomer(conn, customer);
            conn.Close();
            return ret;
        }


    }
}


