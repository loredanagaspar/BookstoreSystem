using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreSystem
{
    public class Admin : User
    {
        // Properties 
        public int AdminID { get; set; }
        public string AdminAccess { get; set; } // Example: "Full Access", "Limited Access", etc.
        public string AdminStatus { get; set; } // Example: "Active", "Inactive", etc.

        // Constructor
        public Admin(int adminID, string adminAccess, string adminStatus, string username, string password, string email, string name, string address, string city, string region, string postCode, string country)
            : base(username, password, email, name, address, city, region, postCode, country)
        {
            AdminID = adminID;
            AdminAccess = adminAccess;
            AdminStatus = adminStatus;
        }
    }

    public class AdminRepository
    {
        public List<Admin> Admins { get; set; } = new List<Admin>();

        public AdminRepository()
        {
            // Adding admin users to the list
            Admins.Add(new Admin(1, "Full Access", "Active", "user1", "password1", "user1@bookstore.com", "Charlie James", "35 Abbey Road", "London", "London", "E1 2ER", "UK"));
            Admins.Add(new Admin(2, "Full Access", "Active", "user2", "password2", "user2@bookstore.com", "John Smith", "136 Wards Wharf", "London", "London", "E16 5AB", "UK"));
            Admins.Add(new Admin(3, "Full Access", "Active", "user3", "password3", "user3@bookstore.com", "Olga Conish", "12 Southey Mews", "London", "London", "E16 2EX", "UK"));
            Admins.Add(new Admin(4, "Limited Access", "Active", "user4", "password4", "user4@bookstore.com", "Jessica Alba", "125 Russell Flint", "London", "London", "NW10 2ER", "UK"));
            Admins.Add(new Admin(5, "Full Access", "Inactive", "user5", "password5", "user5@bookstore.com", "Luca Ciancheta", "13 Standard Road", "London", "London", "W1C 2AB", "UK"));
        }
    }

}
