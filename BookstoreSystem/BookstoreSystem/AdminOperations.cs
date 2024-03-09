using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using static BookstoreSystem.Orders;
using System.Runtime.InteropServices;
using System.IO.Pipes;

namespace BookstoreSystem
{
    public class AdminOperations
    {
        private ItemManager itemManager;
        private CustomerManager customerManager;
        private OrdersManager ordersManager;

        private string historyFilePath = "admin_history.txt"; // Path to the history file
        public AdminOperations(ItemManager itemManager, CustomerManager customerManager, OrdersManager ordersManager)
        {
            this.itemManager = itemManager;
            this.customerManager = customerManager;
            this.ordersManager = ordersManager;

            // Initialize Customers and Orders lists
            Customers = new List<Customer>();
            Orders = new List<Orders>();
        }
        public List<Customer> Customers { get; set; }
        public List<Orders> Orders{ get; set; }


        /// <summary>
        /// The below sections cover methods and classes related to managing operations and logging history within the Stock Management Menu.
        /// </summary>

        // Defining data structure to represent an operation
        private class Operation
        {
            public DateTime Timestamp { get; set; }
            public string Description { get; set; }
        }

        // Method to log an operation to a file
        private void LogOperation(string description)
        {
            // Create a new operation object
            Operation operation = new Operation
            {
                Timestamp = DateTime.Now,
                Description = description
            };

            // Serialize the operation object
            string serializedOperation = $"{operation.Timestamp}: {operation.Description}";

            // Append the serialized operation to the history file
            try
            {
                File.AppendAllLines(historyFilePath, new[] { serializedOperation });
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing to history file: {ex.Message}");
            }
        }

        // Method to initialize the history file
        private void InitializeHistoryFile()
        {
            try
            {
                if (!File.Exists(historyFilePath))
                {
                    File.Create(historyFilePath).Close();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error initializing history file: {ex.Message}");
            }
        }

        /// <summary>
        /// The below sections covers the methods for the StockManagemenetMenu
        /// </summary>
        private bool ItemExistsById(int itemId)
        {
            DBLayer layer = new DBLayer();
            var conn = layer.CreateConnection();
            DBBook dBBook = new DBBook();
            var returnedBook = dBBook.RetrieveBook(conn, itemId);
            conn.Close();
            if (returnedBook.BookID >0) {
                return true; }
            return itemManager.ItemsList.Exists(item => item.ItemId == itemId);
        }


        // Method to search for an item by its ID
        private void SearchItemById(int itemId)
        {
            // Check if the item exists in the list
            if (ItemExistsById(itemId))
            {
                // Find the item in the list by its ID
                Items foundItem = itemManager.ItemsList.Find(item => item.ItemId == itemId);

                if (foundItem != null)
                {
                    Console.WriteLine("Item found:");
                    Console.WriteLine(foundItem.ToString());

                    LogOperation($"Searched for item by ID: {itemId}");
                }
                else
                {
                    Console.WriteLine("Item not found.");
                }
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }
        // Method to add stock book
        public void CreateBook()
        {
            bool AddBook = true;
            int nextAvailableId = itemManager.ItemsList.Count + 1; // Calculate the next available ID

            while (AddBook)
            {
                try
                {
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Please enter the book details:");
                    // Automatically assign the next available ID
                    int itemId = nextAvailableId;

                    Console.WriteLine($"Item ID: {itemId}");
                    Console.Write("Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Price: ");
                    double price = double.Parse(Console.ReadLine());

                    Console.Write("Category: ");
                    string category = Console.ReadLine();

                    Console.Write("Status: ");
                    string status = Console.ReadLine();

                    Console.Write("Author: ");
                    string author = Console.ReadLine();

                    Console.Write("Description: ");
                    string description = Console.ReadLine();

                    Console.Write("Quantity: ");
                    int quantity = int.Parse(Console.ReadLine());

                    // Call the AddBook method from the ItemManager
                    itemManager.AddBook(itemId, name, price, category, status, author, description, quantity);

                    Console.WriteLine("The new updated list of books is:");
                    itemManager.DisplayBooks();

                    LogOperation($"Created book: {itemId}");

                    // Increment the next available ID for the next book
                    nextAvailableId++;

                    // Ask the user if they want to continue adding books
                    Console.WriteLine("Do you want to add another book? (Y/N)");
                    string continueInput = Console.ReadLine();
                    if (continueInput.Trim().ToUpper() != "Y")
                    {
                        // If user does not want to continue, exit the loop
                        AddBook = false; 
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter a valid value.");
                    // Loop will continue as isValidInput is still false
                }
            }
        }

        // Method to update book details

        public void UpdateBookDetails(int itemId, string name, double price, string category, string status, string author, string description, int quantity)
        {
            // Check if the book with the specified ID exists in the list
            //if (!ItemExistsById(itemId))
            //{
            //    Console.WriteLine("Book not found.");
            //    return; // Exit the method if the book is not found
            //}



            // Find the book in the list and update details
            //Book foundBook = (Book)itemManager.ItemsList.Find(item => item is Book && item.ItemId == itemId);
            //if (foundBook != null)
            //{
            //    foundBook.Name = name;
            //    foundBook.Price = price;
            //    foundBook.Category = category;
            //    foundBook.Status = status;
            //    foundBook.Author = author;
            //    foundBook.Description = description;
            //    foundBook.Quantity = quantity;
            //    Console.WriteLine("Book details updated successfully.");
            //}
            //else
            //{
            //    Console.WriteLine("Book not found.");
            //}

            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBBook dBBooK = new DBBook();
            var book = dBBooK.RetrieveBook(conn, itemId);
            book.Name = name;
            book.Price = Convert.ToDecimal(price);
            book.Category = category;
            book.Status = status;
            book.Author = author;
            book.Description = description;
            bool bookUpdated = dBBooK.UpdateBook(conn, book);
            Console.WriteLine("The new updated list of books is:");
            itemManager.DisplayBooks();
            conn.Close();
            LogOperation($"Updated book: {itemId}");
        }

        public void CreateMagazine()
        {
            bool isValidInput = false;
            int nextAvailableId = itemManager.ItemsList.Count + 1; // Calculate the next available ID

            while (!isValidInput)
            {
                try
                {
                    Console.WriteLine("Please enter the item details:");

                    // Automatically assign the next available ID
                    int itemId = nextAvailableId;

                    Console.WriteLine($"Item ID: {itemId}");

                    Console.Write("Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Price: ");
                    double price = double.Parse(Console.ReadLine());

                    Console.Write("Category: ");
                    string category = Console.ReadLine();

                    Console.Write("Status: ");
                    string status = Console.ReadLine();

                    Console.Write("Publisher: ");
                    string publisher = Console.ReadLine();

                    Console.Write("Description: ");
                    string description = Console.ReadLine();

                    Console.Write("Quantity: ");
                    int quantity = int.Parse(Console.ReadLine());

                    // Call the AddMagazine method from the ItemManager
                    itemManager.AddMagazine(itemId, name, price, category, status, publisher, description, quantity);
                    LogOperation($"Created magazine: {itemId}");

                    isValidInput = true; // Set to true if all inputs are valid

                    // Increment the next available ID for the next magazine
                    nextAvailableId++;

                    // Ask the user if they want to continue adding magazines
                    Console.WriteLine("Do you want to add another magazine? (Y/N)");
                    string continueInput = Console.ReadLine().ToUpper();
                    if (continueInput != "Y")
                    {
                        // If user does not want to continue, exit the loop
                        isValidInput = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter a valid value.");
                    // Loop will continue as isValidInput is still false
                }
            }
        }

        // Method to update magazine details
        public void UpdateMagazineDetails(int itemId, string name, double price, string category, string status, string publisher, string description, int quantity)
        {
            // Check if the magazine with the specified ID exists in the list
            //if (!ItemExistsById(itemId))
            //{
            //    Console.WriteLine("Magazine not found.");
            //    return; // Exit the method if the magazine is not found
            //}

            //// Find the magazine in the list and update details
            //Magazine foundMagazine = (Magazine)itemManager.ItemsList.Find(item => item is Magazine && item.ItemId == itemId);
            //if (foundMagazine != null)
            //{
            //    foundMagazine.Name = name;
            //    foundMagazine.Price = price;
            //    foundMagazine.Category = category;
            //    foundMagazine.Status = status;
            //    foundMagazine.Publisher = publisher;
            //    foundMagazine.Description = description;
            //    foundMagazine.Quantity = quantity;
            //    Console.WriteLine("Magazine details updated successfully.");
            //}
            //else
            //{
            //    Console.WriteLine("Magazine not found.");
            //}

            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBMagazine dBMagazine = new DBMagazine();
            var Mag = dBMagazine.RetrieveMagazine(conn, itemId);
            Mag.Name = name;
            Mag.Price = Convert.ToDecimal(price);
            Mag.Category = category;
            Mag.Status = status;
            Mag.Publisher = publisher;
            Mag.Description = description;
            bool bookUpdated = dBMagazine.UpdateMagazine(conn, Mag);
            Console.WriteLine("The new updated list of Magazines is:");
            itemManager.DisplayMagazines();
            conn.Close();
            LogOperation($"Updated book: {itemId}");

        
        }

        // Method to remove stock items by ItemId
        public void RemoveStock(List<Items> itemsList, int itemId)
        {
            Items itemToRemove = itemsList.FirstOrDefault(item => item.ItemId == itemId);
            if (itemToRemove != null)
            {
                itemsList.Remove(itemToRemove);
                Console.WriteLine($"Item with ID {itemId} removed successfully.");
                LogOperation($"Removed item with ID {itemId}");
            }
            else
            {
                Console.WriteLine($"Item with ID {itemId} not found.");
            }
            LogOperation($"Attempted to remove item with ID {itemId}, but item not found");
        }

        /// <summary>
        /// Removes a book from the database;
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool RemoveBook(int itemId)
        {
            bool ret = false;
            if (ItemExistsById(itemId))
            {
                Console.WriteLine("Are you sure you would like to delete the book? Y/N");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    DBLayer dBLayer = new DBLayer();
                    var conn = dBLayer.CreateConnection();
                    DBBook dBBook = new DBBook();
                    dBBook.DeleteBook(conn, itemId);
                    conn.Close();
                    return true;
                }
            }
            return ret;
        }

        /// <summary>
        /// Removes a book from the database;
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool RemoveMagazine(int itemId)
        {
            bool ret = false;
            if (ItemExistsById(itemId))
            {
                Console.WriteLine("Are you sure you would like to delete the Magazine? Y/N");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    DBLayer dBLayer = new DBLayer();
                    var conn = dBLayer.CreateConnection();
                    DBMagazine dBMag = new DBMagazine();
                    dBMag.DeleteMagazine(conn, itemId);
                    conn.Close();
                    return true;
                }
            }
            return ret;
        }

        /// <summary>
        /// The below sections covers the methods for the customerManagemenet Menu
        /// </summary>


        // Method to update customer details
        public void UpdateCustomerDetails(int customerID, string newEmail, string newAddress, string newCity, string newRegion, string newPostCode, string newCountry)
        {            

            // Find the customer in the list and update their details
            Customer foundCustomer = Customers.Find(c => c.CustomerID == customerID);
            if (foundCustomer != null)
            {
                // Update customer details
                foundCustomer.UpdateAccountDetails(foundCustomer.Name, newEmail, newAddress, newCity, newRegion, newPostCode, newCountry);
                Console.WriteLine("Customer details updated successfully.");

                // Display updated details
                Console.WriteLine("Updated Customer Details:");
                //foundCustomer.();

                LogOperation($"Updated customer details: {foundCustomer.Name}");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        // Method to view orders by order ID
        private void ViewProcessedOrders()
        {
            Console.WriteLine("Please enter the ID of the order to view:");
            int orderId;
            while (!int.TryParse(Console.ReadLine(), out orderId))
            {
                Console.WriteLine("Invalid input. Please enter a valid order ID:");
            }

            // Find the order by ID
            var order = ordersManager.AllOrders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                Console.WriteLine($"Order with ID {orderId} not found.");
                return;
            }

            // Display the order
            ordersManager.DisplayOrdersByOrderID(orderId);
        }

        // Method to view payments
        public void ViewPayments(List<PaymentDetails> payments, List<Customer> customers)
        {
            foreach (var payment in payments)
            {
                // Find the customer associated with the payment
                Customer customer = customers.Find(c => c.CustomerID == payment.CustomerId);

                // Check if the customer was found
                if (customer != null)
                {
                    Console.WriteLine($"Payment ID: {payment.PaymentId}, Date: {payment.PaymentDate}, Amount: {payment.Amount}, Customer: {customer.Name}");
                }
                else
                {
                    Console.WriteLine($"Payment ID: {payment.PaymentId}, Date: {payment.PaymentDate}, Amount: {payment.Amount}, Customer not found");
                }
            }
            LogOperation("Viewed payments");
        }

        // Method to display admin menu
        public void DisplayAdminMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Welcome Admin! Please select an operation:");
                Console.WriteLine("1. Stock Management");
                Console.WriteLine("2. Customer Management");
                Console.WriteLine("3. Exit");
                Console.WriteLine("----------------------------");
                // Read user input
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayStockManagementMenu();
                        break;
                    case "2":
                        DisplayCustomerManagementMenu();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        exit = true; // Set exit flag to true to break the loop
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                LogOperation("Exited admin menu");
            }
        }

        /// <summary>
        /// method to check whether the user has quite the program or not.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool CheckForQuit(string input)
        {
            return input.Trim().Equals("quit", StringComparison.OrdinalIgnoreCase);
        }

        // Method to display stock management menu
        private void DisplayStockManagementMenu()
        {
            bool backToMainMenu = false;

            while (!backToMainMenu)
            {
                //Display the stock management menu
                Console.WriteLine("----------------------------");
                Console.WriteLine("Stock Management Menu:");
                Console.WriteLine("1. Create Book");
                Console.WriteLine("2. Update Book Details");
                Console.WriteLine("3. Delete Book");
                Console.WriteLine("4. Create Magazine");
                Console.WriteLine("5. Update Magazine Details");
                Console.WriteLine("6. Delete Magazine");
                Console.WriteLine("7. Search item by its ID");
                Console.WriteLine("8. Display list of items.");
                Console.WriteLine("9. Back to Main Menu");
                Console.WriteLine("----------------------------");

                string choice = Console.ReadLine();

                if (CheckForQuit(choice))
                {
                    Console.WriteLine("Exiting stock management menu...");
                    LogOperation("Exited stock management menu");
                    return;
                }
                try
                {
                    switch (choice)
                    {
                        case "1":
                            CreateBook();
                            break;
                        case "2":
                            bool validBookId = false;
                            int bookId = 0;

                            while (!validBookId)
                            {
                                
                                itemManager.DisplayBooks();
                                Console.WriteLine("Please enter the ID of the book you want to update:");
                                while (!int.TryParse(Console.ReadLine(), out bookId))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid book ID:");
                                }

                                // Check if the item exists
                                if (!ItemExistsById(bookId))
                                {
                                    Console.WriteLine("Book not found.");
                                    Console.WriteLine("Do you want to try another book ID? (Y/N)");
                                    string tryAgain = Console.ReadLine().ToUpper();
                                    if (tryAgain != "Y")
                                    {
                                        // If user does not want to continue, exit the loop
                                        break;
                                    }
                                }
                                else
                                {
                                    validBookId = true; // Set to true if a valid book ID is entered
                                }
                            }

                            if (validBookId)
                            {
                                // Prompt the user for new details
                                Console.Write("New Name: ");
                                string name = Console.ReadLine();

                                Console.Write("New Price: ");
                                double price;
                                while (!double.TryParse(Console.ReadLine(), out price))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid price:");
                                }

                                Console.Write("New Category: ");
                                string category = Console.ReadLine();

                                Console.Write("New Status: ");
                                string status = Console.ReadLine();

                                Console.Write("New Author: ");
                                string author = Console.ReadLine();

                                Console.Write("New Description: ");
                                string description = Console.ReadLine();

                                Console.Write("New Quantity: ");
                                int quantity;
                                while (!int.TryParse(Console.ReadLine(), out quantity))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid quantity:");
                                }

                                // Call the UpdateBookDetails method
                                UpdateBookDetails(bookId, name, price, category, status, author, description, quantity);
                            }
                            break;
                        case "3":
                            itemManager.DisplayBooks();
                            Console.WriteLine("Please enter the book Id of the book you want to delete:");
                            int bookIdToRemove;
                            if (int.TryParse(Console.ReadLine(), out bookIdToRemove))
                            {
                                //RemoveStock(itemManager.ItemsList, bookIdToRemove);
                                if(RemoveBook(bookIdToRemove))
                                {
                                    Console.WriteLine("Success!");
                                    itemManager.DisplayBooks();
                                    Console.WriteLine("-----------------------------------------");
                                }
                            }
                            else
                            {
                                throw new FormatException("Invalid book ID format. Please enter a valid number.");
                            }
                            break;
                        case "4":
                            Console.WriteLine("Please enter the new magazine details:");
                            CreateMagazine();
                            break;
                        case "5":
                            itemManager.DisplayMagazines();
                            Console.WriteLine("Please enter the magazine ID of the magazine you want to update:");
                            int magazineId;
                            if (int.TryParse(Console.ReadLine(), out magazineId))
                            {
                                Console.Write("New Name: ");
                                string magazineName = Console.ReadLine();
                                Console.Write("New Price: ");
                                double magazinePrice;
                                if (!double.TryParse(Console.ReadLine(), out magazinePrice))
                                {
                                    throw new FormatException("Invalid price format. Please enter a valid number.");
                                }
                                Console.Write("New Category: ");
                                string magazineCategory = Console.ReadLine();
                                Console.Write("New Status: ");
                                string magazineStatus = Console.ReadLine();
                                Console.Write("New Publisher: ");
                                string magazinePublisher = Console.ReadLine();
                                Console.Write("New Description: ");
                                string magazineDescription = Console.ReadLine();
                                Console.Write("New Quantity: ");
                                int magazineQuantity;
                                if (!int.TryParse(Console.ReadLine(), out magazineQuantity))
                                {
                                    throw new FormatException("Invalid quantity format. Please enter a valid number.");
                                }

                                // Call the UpdateMagazineDetails method
                                UpdateMagazineDetails(magazineId, magazineName, magazinePrice, magazineCategory, magazineStatus, magazinePublisher, magazineDescription, magazineQuantity);
                            }
                            else
                            {
                                throw new FormatException("Invalid magazine ID format. Please enter a valid number.");
                            }
                            break;
                        case "6":
                            itemManager.DisplayMagazines();
                            Console.WriteLine("Please enter the book Id of the book you want to delete:");
                            int magazineIdToRemove;
                            if (int.TryParse(Console.ReadLine(), out magazineIdToRemove))
                            {
                                //RemoveStock(itemManager.ItemsList, magazineIdToRemove);
                                if (RemoveMagazine(magazineIdToRemove))
                                {
                                    Console.WriteLine("Success!");
                                    itemManager.DisplayMagazines();
                                    Console.WriteLine("-----------------------------------------");
                                }
                            }
                            else
                            {
                                throw new FormatException("Invalid magazine ID format. Please enter a valid number.");
                            }
                            break;
                        case "7":
                            Console.WriteLine("Would you like to see books or Magazines?");
                            Console.WriteLine("Please select a category");
                            Console.WriteLine("1.Magazines");
                            Console.WriteLine("2.Books");

                            int choice1;
                            if (int.TryParse(Console.ReadLine(), out choice1))
                            {
                                int choice2;
                                if (choice1 == 1)
                                {
                                    itemManager.DisplayMagazines();
                                    Console.WriteLine("Please select a magazine to view details on:");
                                    string input = Console.ReadLine();
                                    itemManager.DisplayMagazine(Convert.ToInt32(input));                              
                                  
                                }
                                else if (choice1 == 2)
                                {
                                    itemManager.DisplayBooks();
                                    Console.WriteLine("Please select a magazine to view details on:");
                                    string input = Console.ReadLine();
                                    itemManager.DisplayBook(Convert.ToInt32(input));
                                }
                                // Call the SearchItemById method to search for the item
                                
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid ID.");
                            }
                            break;
                        case "8":
                            Console.WriteLine("The current list of books are:");
                            itemManager.DisplayBooks();
                            Console.WriteLine("--------------------------------");
                            Console.WriteLine("The current list of Magazines are:");
                            itemManager.DisplayMagazines();
                            Console.WriteLine("---------------------------------");
                            break;
                        case "9":
                            backToMainMenu = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            LogOperation("Exited stock management menu");
        }


        // Method to display customer management menu
        private void DisplayCustomerManagementMenu()
        {
            bool backToMainMenu = false;
            while (!backToMainMenu)
            {
                try
                {
                    //display the customer management menu
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Customer Management Menu:");
                    Console.WriteLine("1. Update Customer Details");
                    Console.WriteLine("2. View Payments");
                    Console.WriteLine("3. View Processed Orders");
                    Console.WriteLine("4. View Customer list.");
                    Console.WriteLine("5. Back to Main Menu");
                    Console.WriteLine("----------------------------");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            bool validCustomerId = false;
                            int customerId = 0;

                            while (!validCustomerId)
                            {
                                //first display a list of all customers which are in the database to update.
                                CustomerManager cus = new CustomerManager();
                                cus.DisplayAllCustomers();
                                Console.WriteLine("Please enter the ID of the customer you want to update:");
                                while (!int.TryParse(Console.ReadLine(), out customerId))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid customer ID:");
                                }

                                // Check if the customer exists
                                if (!customerManager.CustomerExists(customerId))
                                {
                                    Console.WriteLine("Customer not found.");
                                    Console.WriteLine("Do you want to try another customer ID? (Y/N)");
                                    string tryAgain = Console.ReadLine().ToUpper();
                                    if (tryAgain != "Y")
                                    {
                                        // If the user does not want to continue, exit the loop
                                        break;
                                    }
                                }
                                else
                                {
                                    validCustomerId = true; // Set to true if a valid customer ID is entered
                                }
                            }

                            if (validCustomerId)
                            {
                                // Prompt the user for new details
                                Console.WriteLine("Enter new email:");
                                string newEmail = Console.ReadLine();
                                Console.WriteLine("Enter new address:");
                                string newAddress = Console.ReadLine();
                                Console.WriteLine("Enter new city:");
                                string newCity = Console.ReadLine();
                                Console.WriteLine("Enter new region:");
                                string newRegion = Console.ReadLine();
                                Console.WriteLine("Enter new postal code:");
                                string newPostCode = Console.ReadLine();
                                Console.WriteLine("Enter new country:");
                                string newCountry = Console.ReadLine();
                                CustomerManager manager = new CustomerManager();                                  
                                // Call the UpdateCustomerDetails method
                                if (manager.UpdateCustomer(customerId, newEmail, newAddress, newCity, newRegion, newPostCode, newCountry))
                                {                                    
                                    Console.WriteLine("Customer was successfully updated!");
                                    Console.WriteLine("New customer details are");
                                    manager.DisplayCustomer(customerId);
                                }
                            }
                            break;
                        case "2":
                            // Add logic for viewing payments
                            break;
                        case "3":
                            Console.WriteLine("Enter the customer ID:");
                            int customerIdForOrders;
                            while (!int.TryParse(Console.ReadLine(), out customerIdForOrders))
                            {
                                Console.WriteLine("Invalid input. Please enter a valid customer ID:");
                            }

                            // Find the customer by ID
                            var customerForOrders = customerManager.Customers.Find(c => c.CustomerID == customerIdForOrders);
                            if (customerForOrders != null)
                            {
                                // Display orders for the specified customer ID
                                
                            }
                            else
                            {
                                Console.WriteLine("Customer not found.");
                            }
                            break;
                        case "4":
                            customerManager.DisplayAllCustomers();
                            break;
                        case "5":
                            backToMainMenu = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            LogOperation("Exited customer management menu");
        }

    }
}
