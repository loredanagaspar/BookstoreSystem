using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using static BookstoreSystem.DBLayer;

namespace BookstoreSystem
{
    public class Items
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        // Constructor
        public Items(int itemId, string name, double price, string category, string status, int quantity, string description)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            Category = category;
            Status = status;
            Quantity = quantity;
            Description = description;
        }
    }

    public class Book : Items
    {
        public string Author { get; set; }


        // Constructor
        public Book(int itemId, string name, double price, string category, string status, string author, string description, int quantity)
            : base(itemId, name, price, category, status, quantity, description)
        {
            Author = author;
        }

        // Method to update book details
        public void UpdateBookDetails(string name, double price, string category, string status, string author, string description, int quantity)
        {
            // Update book details
            Name = name;
            Price = price;
            Category = category;
            Status = status;
            Author = author;
            Description = description;
            Quantity = quantity;
        }
    }

    public class Magazine : Items
    {
        public string Publisher { get; set; }

        // Constructor
        public Magazine(int itemId, string name, double price, string category, string status, string publisher, string description, int quantity)
            : base(itemId, name, price, category, status, quantity, description)
        {
            Publisher = publisher;

        }

        // Method to update magazine details
        public void UpdateMagazineDetails(string name, double price, string category, string status, string publisher, string description, int quantity)
        {
            // Update magazine details
            Name = name;
            Price = price;
            Category = category;
            Status = status;
            Publisher = publisher;
            Description = description;
            Quantity = quantity;
        }
    }

    public class ItemManager
    {
        public List<Items> ItemsList { get; } = new List<Items>();

        public ItemManager()
        {
            // Adding books
            ItemsList.Add(new Book(1, "The Great Gatsby", 12.99, "Classic", "Available", "F. Scott Fitzgerald", "A story of the fabulously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan.", 5));
            ItemsList.Add(new Book(2, "To Kill a Mockingbird", 10.49, "Classic", "Available", "Harper Lee", "A gripping portrayal of racial injustice and moral growth in the Deep South.", 5));
            ItemsList.Add(new Book(3, "1984", 15.99, "Dystopian", "Available", "George Orwell", "A chilling dystopian novel depicting a totalitarian regime and the struggle for freedom.", 5));
            ItemsList.Add(new Book(4, "The Catcher in the Rye", 11.99, "Coming-of-age", "Available", "J.D. Salinger", "A classic novel following Holden Caulfield's journey through New York City after being expelled from prep school.", 5));
            ItemsList.Add(new Book(5, "Harry Potter and the Philosopher's Stone", 18.99, "Fantasy", "Available", "J.K. Rowling", "The first book in the Harry Potter series, chronicling the adventures of a young wizard.", 5));

            // Adding magazines
            ItemsList.Add(new Magazine(6, "National Geographic", 5.99, "Science", "Available", "National Geographic Society", "Explore the wonders of the natural world with stunning photography and in-depth articles.", 5));
            ItemsList.Add(new Magazine(7, "Time", 4.99, "Current Affairs", "Available", "Time USA, LLC", "Stay updated on the latest news, politics, and cultural developments from around the globe.", 5));
            ItemsList.Add(new Magazine(8, "The New Yorker", 6.49, "Literary", "Available", "Conde Nast", "Experience thought-provoking journalism, fiction, satire, and poetry from renowned writers.", 5));
            ItemsList.Add(new Magazine(9, "Vogue", 3.99, "Fashion", "Available", "Conde Nast", "Delve into the world of high fashion, beauty, and celebrity culture with cutting-edge photography and features.", 5));
            ItemsList.Add(new Magazine(10, "Scientific American", 6.99, "Science", "Available", "Springer Nature", "Read about groundbreaking discoveries and advancements in science and technology.", 5));
        }


        // Method to add a book
        public void AddBook(int itemId, string name, double price, string category, string status, string author, string description, int quantity)
        {
            Book book = new Book(itemId, name, price, category, status, author, description, quantity);
            ItemsList.Add(book);

            #region Database
            //Connect to the database and add in the book using the DBlayer book class.
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBBook  dbBook = new DBBook(name,Convert.ToDecimal(price),category,status,author,description);
            if (dbBook.InsertBook(conn))
            {
                Console.WriteLine("Book Added");
            }

            #endregion
        }

        // Method to add a magazine
        public void AddMagazine(int itemId, string name, double price, string category, string status, string publisher, string description, int quantity)
        {
            Magazine magazine = new Magazine(itemId, name, price, category, status, publisher, description, quantity);
            ItemsList.Add(magazine);

            //interface with the DBLayer and add in a new magazine.
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBMagazine dBMagazine = new DBMagazine(name,Convert.ToDecimal(price),category,status,publisher,description);
            if(dBMagazine.InsertMagazine(conn))
            {
                Console.WriteLine("Magazine Added");
            }
            
        }

        /// <summary>
        /// Displays a list of all of the books which are currently in the Database.
        /// </summary>
        public void DisplayBooks()
        {
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBBook dbBook = new DBBook();
            List<DBBook> book = dbBook.RetreiveBooks(conn);
            Console.WriteLine("The list of Books are: ");
            foreach (DBBook bookItem in book)
            {
                Console.WriteLine(bookItem.BookID +". " + bookItem.Name );
            }
        }

        /// <summary>
        /// Will display a list of all magazines which are currently in the database.
        /// </summary>
        public void DisplayMagazines()
        {
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBMagazine dBMagazine = new DBMagazine();
            List<DBMagazine> Mags = dBMagazine.RetreiveMagazines(conn);
            Console.WriteLine("The list of Magazines are: ");
            foreach (DBMagazine magItem in Mags)
            {
                Console.WriteLine(magItem.MagazineID + ". " + magItem.Name);
            }
        }

        public bool DisplayMagazine(int MagID)
        {
            bool ret = false;
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBMagazine dBMagazine = new DBMagazine();
            DBMagazine Magazine = dBMagazine.RetrieveMagazine(conn, MagID);
            Console.WriteLine("MagazineID: " + Magazine.MagazineID);
            Console.WriteLine("Name: " + Magazine.Name);
            Console.WriteLine("Price: " + Magazine.Price);
            Console.WriteLine("Category: " + Magazine.Category);
            Console.WriteLine(("Status: " + Magazine.Status));
            Console.WriteLine("Publisher: " + Magazine.Publisher);
            Console.WriteLine("Description: " + Magazine.Description);
            return ret;
        }

        public bool DisplayBook(int BookID)
        {
            bool ret = false;
            DBLayer dBLayer = new DBLayer();
            var conn = dBLayer.CreateConnection();
            DBBook dBBook = new DBBook();
            DBBook Book = dBBook.RetrieveBook(conn, BookID);
            Console.WriteLine("BookID: " + Book.BookID);
            Console.WriteLine("Name: " + Book.Name);    
            Console.WriteLine("Price: " + Book.Price);
            Console.WriteLine("Category: " + Book.Category);
            Console.WriteLine("Status: " + Book.Status);
            Console.WriteLine("Author: " + Book.Author);
            Console.WriteLine("Description: " + Book.Description);
            return ret;
        }

        public void UpdateBook(int BookID)
        {

        }

       

        public bool ItemExistsById(int itemId)
        {
            return ItemsList.Any(item => item.ItemId == itemId);
        }


        // Method to display all items
        public void DisplayItems()
        {
            foreach (var item in ItemsList)
            {
                Console.WriteLine($"Item ID: {item.ItemId}, Name: {item.Name}, Price: {item.Price}, Category: {item.Category}, Status: {item.Status}, Quantity: {item.Quantity}");
                if (item is Book)
                {
                    Book book = (Book)item;
                    Console.WriteLine($"Author: {book.Author}, Description: {book.Description}");
                }
                else if (item is Magazine)
                {
                    Magazine magazine = (Magazine)item;
                    Console.WriteLine($"Publisher: {magazine.Publisher}, Description: {magazine.Description}");
                }
                Console.WriteLine();
            }
        }
    }
}
