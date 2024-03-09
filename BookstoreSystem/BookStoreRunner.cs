using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static BookstoreSystem.Orders;

namespace BookstoreSystem
{
    public class BookStoreRunner
    {
        public static void Main(string[] args)
        {
            // Create an ItemManager object
            ItemManager itemManager = new ItemManager();
            // Create a CustomerManager object
            CustomerManager customerManager = new CustomerManager();
            // Create an OrdersManager object
            OrdersManager ordersManager = new OrdersManager();

            //Interface with the DBLayer class and see if the database exists before we boot up the system
            DBLayer layer = new DBLayer();
            bool exists = layer.DatabaseExists();
            if (exists == false)
            { Console.WriteLine("Could not find the database, would you like to install a fresh copy? Y/N");
                string answer = Console.ReadLine();
                if (answer.ToString().ToLower() == "y")
                {
                    var conn = layer.CreateConnection();
                    layer.CreateTables(conn);
                    layer.PopulateData(conn);
                    layer.PopulateStock(conn);
                }
            }            

            Console.WriteLine("Welcome to The Bookstore");
            Console.WriteLine("----------------------------");
            Console.WriteLine("Please sign in");
            int Logged = -1;
            while (Logged < 0)
            {
                Logged = Login();
                if (Logged < 0)
                { 
                    Console.WriteLine("Username or Password incorrect. Please try again");
                    Console.WriteLine();
                }
                else 
                { 
                    Console.WriteLine("Success");
                    Console.WriteLine();
                    Console.WriteLine("----------------------------");
                }
            }

            #region Loggedin
            //if the logged pointer returns a 1 then an admin has logged into the system
            if (Logged == 1)
            {
                // Initialize AdminOperations
                AdminOperations adminOperations = null;
                try
                {
                    adminOperations = new AdminOperations(itemManager, customerManager, ordersManager);
                    // Display the admin menu
                    adminOperations.DisplayAdminMenu();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing AdminOperations: {ex.Message}");
                    return;
                }               
            }
            //if the logged pointer 
            else if (Logged == 0)
            {
                //CustomerOperations customerOperations = null;
                //customerOperations.

            }
            #endregion
        }

        private static int Login()
        {
            int ret = -1;
            DBLayer layer = new DBLayer();
            var conn1 = layer.CreateConnection();
            Console.WriteLine("Username:");
            string username = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            DBUser dBUser = new DBUser(username, password);
            ret = dBUser.Login(conn1);
            return ret;
        }
    }
}



