using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using System;
using System.Collections.Generic;

namespace BookstoreSystem
{
    public class User
    {
        // Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        // Constructor
        public User(string username, string password, string email, string name, string address, string city, string region, string postCode, string country)
        {
            Username = username;
            Password = password;
            Email = email;
            Name = name;
            Address = address;
            City = city;
            Region = region;
            PostCode = postCode;
            Country = country;
        }

        // Method to register a new user
        public static bool RegisterUser(List<User> users, string username, string password, string email, string name, string address, string city, string region, string postCode, string country)
        {
            // Check if the username is already taken
            if (IsUsernameTaken(users, username))
            {
                return false;
            }

            // Create a new user object
            User newUser = new User(username, password, email, name, address, city, region, postCode, country);

            // Add the new user to the list of users
            users.Add(newUser);

            return true;
        }

        // Method to authenticate a user
        public static bool AuthenticateUser(List<User> users, string username, string password)
        {
            // Find the user with the given username
            User user = users.Find(u => u.Username == username);

            // Check if the user exists and if the password matches
            return user != null && user.Password == password;
        }

        // Method to check if a username is already taken
        private static bool IsUsernameTaken(List<User> users, string username)
        {
            // Find if any user in the list has the same username
            return users.Exists(u => u.Username == username);
        }
    }
}


