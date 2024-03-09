
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookstoreSystem
{
    internal class DBLayer
    {
        /// <summary>
        /// will check the system to see if the database actually exists on the system
        /// </summary>
        /// <returns></returns>
        public bool DatabaseExists()
        {
            bool ret = false;
            // Specify the file name you want to check
            string fileName = "Bookstore.db";

            // Get the path to the bin folder
            string binFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");

            // Combine the bin folder path with the file name
            string filePathInBin = Path.Combine(binFolderPath, fileName);

            // Check if the file exists
            if (File.Exists(filePathInBin))
            {
                //Console.WriteLine($"The file {fileName} exists in the bin folder.");
                return true;
            }
            else
            {
                Console.WriteLine($"The file {fileName} does not exist in the bin folder.");
            }
            return ret;
        }

        public SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=Bookstore.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        /// <summary>
        /// Will crete the tables in the database if the are not already present.
        /// </summary>
        /// <param name="conn">Connection to the database,</param>
        public bool CreateTables(SQLiteConnection conn)
        {
            bool ret = false;
            try
            {
                //Create a command instance which we will use to interface with the database.
                SQLiteCommand sqlite_cmd;

                //create a list of strings to create the intial tables in the database.
                string CreateUserTable = "CREATE TABLE User(UserID INTEGER PRIMARY KEY AUTOINCREMENT, UserName VARCHAR(20), Password VarChar(20), AdminUser bit, Email VARCHAR(100), Name VARCHAR(100), Address VARCHAR(100), City VARCHAR(100), Region VARCHAR(100), Country VARCHAR(100))";
                string CreateBookTable = "CREATE TABLE Book(BookID INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(50), Price DECIMAL(18,2), Category VARCHAR(20), Status VarChar(100), Author VARCHAR(100), Description VARCHAR(100))";
                string CreateMagazineTable = "CREATE TABLE Magazine(MagazineID INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(50), Price DECIMAL(18,2), Category VARCHAR(20), Status VARCHAR(100), Publisher VARCHAR(100), Description VARCHAR(100))";
                string CreateTableStock = "CREATE TABLE Stock (StockID INTEGER PRIMARY KEY AUTOINCREMENT, StockType INT, ItemID INT, StockLevel INT)";
                string CreateCustomerTable = "CREATE TABLE Customer(CustomerID INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(50), Username VARCHAR(50),Password VARCHAR(100),Email VARCHAR(50), Address VARCHAR(200), City VARCHAR(50), Region VARCHAR(50), PostCode VARCHAR(20), Country VARCHAR(100))";               
                string CreateOrderTable = "CREATE TABLE Orders (OrderID INTEGER PRIMARY KEY AUTOINCREMENT, CustomerID INT, ItemID INT, Status INT)";

                //once we have all of our tables to create then create them in our database.
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = CreateUserTable;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = CreateBookTable;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = CreateMagazineTable;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = CreateCustomerTable;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = CreateTableStock;
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = CreateOrderTable;
                sqlite_cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return ret;
        }


        /// <summary>
        /// Populates the data into the tables from the database.
        /// </summary>
        /// <param name="conn">connection to the database</param>
        public void PopulateData(SQLiteConnection conn)
        {
           
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            //Strings to create users in the database.
            #region user
            sqlite_cmd.CommandText = "INSERT INTO User (Username, Password,AdminUser,Email,Name,Address,City,Region,Country) VALUES('Admin', 'Admin', 1, 'Admin@Bookstore.com', 'Administrator', '1 admin way', 'London', 'The South', 'England'); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO User (Username, Password,AdminUser,Email,Name,Address,City,Region,Country) VALUES('Adam', 'Ads', 0, 'User@Bookstore.com', 'User', '1 user lane', 'Portsmouth', 'The South', 'England'); ";
            sqlite_cmd.ExecuteNonQuery();
            #endregion
            //strings to create books in the database
            #region Book
            sqlite_cmd.CommandText = "INSERT INTO Book(Name, Price, Category,Status,Author,Description) VALUES(\"The Great Gatsby\", 12.99, \"Classic\", \"Available\", \"F. Scott Fitzgerald\", \"A story of the fabulously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan.\"); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Book(Name, Price, Category,Status,Author,Description) VALUES(\"To Kill a Mockingbird\", 10.49, \"Classic\", \"Available\", \"Harper Lee\", \"A gripping portrayal of racial injustice and moral growth in the Deep South.\"); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Book(Name, Price, Category,Status,Author,Description) VALUES(\"1984\", 15.99, \"Dystopian\", \"Available\", \"George Orwell\", \"A chilling dystopian novel depicting a totalitarian regime and the struggle for freedom.\"); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Book(Name, Price, Category,Status,Author,Description) VALUES(\"The Catcher in the Rye\", 11.99, \"Coming-of-age\", \"Available\", \"J.D. Salinger\", \"A classic novel following Holden Caulfield's journey through New York City after being expelled from prep school.\")";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Book(Name, Price, Category,Status,Author,Description) VALUES(\"Harry Potter and the Philosopher's Stone\", 18.99, \"Fantasy\", \"Available\", \"J.K. Rowling\", \"The first book in the Harry Potter series, chronicling the adventures of a young wizard.\")";
            sqlite_cmd.ExecuteNonQuery();
            #endregion
            //string to create magazines in the database.
            #region Magazine
            sqlite_cmd.CommandText = "INSERT INTO Magazine(Name, Price, Category,Status,Publisher,Description) VALUES(\"National Geographic\", 5.99, \"Science\", \"Available\", \"National Geographic Society\", \"Explore the wonders of the natural world with stunning photography and in-depth articles.\") ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Magazine(Name, Price, Category,Status,Publisher,Description) VALUES(\"Time\", 4.99, \"Current Affairs\", \"Available\", \"Time USA, LLC\", \"Stay updated on the latest news, politics, and cultural developments from around the globe.\") ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Magazine(Name, Price, Category,Status,Publisher,Description) VALUES(\"The New Yorker\", 6.49, \"Literary\", \"Available\", \"Conde Nast\", \"Experience thought-provoking journalism, fiction, satire, and poetry from renowned writers.\") ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Magazine(Name, Price, Category,Status,Publisher,Description) VALUES(\"Vogue\", 3.99, \"Fashion\", \"Available\", \"Conde Nast\", \"Delve into the world of high fashion, beauty, and celebrity culture with cutting-edge photography and features.\") ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Magazine(Name, Price, Category,Status,Publisher,Description) VALUES(\"Scientific American\", 6.99, \"Science\", \"Available\", \"Springer Nature\", \"Read about groundbreaking discoveries and advancements in science and technology.\")";
            sqlite_cmd.ExecuteNonQuery();
            #endregion

            #region Customer
            //strings to create customers in the database.
            sqlite_cmd.CommandText = "INSERT INTO Customer(Name, Username, Password, Email,Address,City,Region,Postcode, Country) VALUES(\"Tom Smith\", \"user6\", \"password6\", \"user6@bookstore.com\", \"49 The Vale\", \"London\", \"London\", \"N1 5AH\", \"UK\")";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Customer(Name, Username, Password, Email,Address,City,Region,Postcode, Country) VALUES(\"Mike Thorne\", \"user7\", \"password7\", \"user7@bookstore.com\", \"150 Ozxford Street\", \"London\", \"London\", \"E1 3GB\", \"UK\")";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Customer(Name, Username, Password, Email,Address,City,Region,Postcode, Country) VALUES(\"Anna Maria\", \"user8\", \"password8\", \"user8@bookstore.com\", \"30 Piccadilly Road\", \"London\", \"London\", \"NW10 3GB\", \"UK\")";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Customer(Name, Username, Password, Email,Address,City,Region,Postcode, Country) VALUES(\"Laura Stoica\", \"user9\", \"password9\", \"user9@bookstore.com\", \"01 Downing Street\", \"London\", \"London\", \"W3 3QE\", \"UK\")";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO Customer(Name, Username, Password, Email,Address,City,Region,Postcode, Country) VALUES(\"George Michael\", \"user10\", \"password10\", \"user10@bookstore.com\", \"13 Primrose Road\", \"London\", \"London\", \"E17 2DF\", \"UK\")";
            sqlite_cmd.ExecuteNonQuery();
            #endregion          
        }

        /// <summary>
        /// Populates the stock tables with all neccassary details.
        /// </summary>
        /// <param name="conn">connection to the database.</param>
        public void PopulateStock(SQLiteConnection conn)
        {
            //Create an instance of the magazine and book classes
            DBMagazine mag = new DBMagazine();
            DBBook book = new DBBook();
            //retrieve a list of all magazines and books which we have stored in the database.
            var Magazines = mag.RetreiveMagazines(conn);
            var Books = book.RetreiveBooks(conn);
            //iterate through each of the books and add in the stock to the database.
            foreach (var Book in Books)
            {
                DBStock dBStock = new DBStock();
                dBStock.StockType = (int)DBStock.StockTypes.Book;
                dBStock.StockLevel = 5;
                dBStock.ItemID = Book.BookID;
                dBStock.InsertStock(conn);
            }
            //iterate through each of the magazines and add in the stock to the database.
            foreach (var mags in Magazines)
            {
                DBStock dBStock = new DBStock();
                dBStock.StockType = (int)DBStock.StockTypes.Magazine;
                dBStock.StockLevel = 5;
                dBStock.ItemID = mags.MagazineID;
                dBStock.InsertStock(conn);
            }
        }

       
    }
    /// <summary>
    /// Class to interface with the user table in the database
    /// </summary>
    public class DBUser
    {
        private string Username;
        private string Password;

        public DBUser(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Method to login to the system
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int Login(SQLiteConnection conn)
        {
            //set a return value to -1
            //if we return over 0 or 1 then the user has logged in
            //1 is an admin
            //0 is a normal user.
            int ret = -1;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT AdminUser FROM User Where Username = '" + Username + "' AND Password = '" + Password + "'";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                int myreader = sqlite_datareader.GetInt32(0);
                ret = myreader;
            }

            return ret;
        }
    }

    /// <summary>
    /// Class to interface with the book table in the database
    /// </summary>
    public class DBBook
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public Decimal Price
        {
            get; set;
        }
        public String Category { get; set; }
        public String Status { get; set; }
        public string Author { get; set; }
        public String Description { get; set; }

        public DBBook()
        {
            BookID = 0;
            Name = "";
            Price = new Decimal(0);
            Category = "";
            Status = "Avaliable";
            Author = "";
            Description = "";
        }

        /// <summary>
        /// Constuctor for the database book class
        /// </summary>
        /// <param name="name">Book Name</param>
        /// <param name="price">Price of book in £</param>
        /// <param name="catgegory">Book category</param>
        /// <param name="status"></param>
        /// <param name="author">Book Author</param>
        /// <param name="description">Decription of the book.</param>
        public DBBook(string name, decimal price, string catgegory, string status, string author, string description)
        {
            Name = name;
            Price = price;
            Category = catgegory;
            Status = status;
            Author = author;
            Description = description;
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="conn">SQLite connection</param>
        /// <param name="BookID">Unique ID for the book.</param>
        /// <returns></returns>
        public bool DeleteBook(SQLiteConnection conn, int BookID)
        {
            bool ret = false;
            string query = "Delete From Book where BookID = " + BookID.ToString();
            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// Inserts a book into the database.
        /// </summary>
        /// <param name="conn">SQLite Connection</param>
        /// <returns></returns>
        public bool InsertBook(SQLiteConnection conn)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Book(");
            sb.Append("Name, ");
            sb.Append("Price, ");
            sb.Append("Category, ");
            sb.Append("Status, ");
            sb.Append("Author, ");
            sb.Append("Description, ");
            sb.Append("Quantity) ");
            sb.Append("VALUES (");
            sb.Append("'" + Name + "', ");
            sb.Append(Price + ", ");
            sb.Append("'" + Category + "', ");
            sb.Append("'" + Status + "', ");
            sb.Append("'" + Author + "', ");
            sb.Append("'" + Description + "') ");

            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// Retrieves a list of Books from the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public List<DBBook> RetreiveBooks(SQLiteConnection conn)
        {
            List<DBBook> books = new List<DBBook>();
            // Your SELECT * FROM query
            string query = "SELECT * FROM Book";

            // Create a data adapter
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                // Create a DataSet to hold the result
                DataSet dataSet = new DataSet();

                // Fill the DataSet with the result of the query
                adapter.Fill(dataSet);

                // You can access the result in tables within the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Process the data as needed
                foreach (DataRow row in dataTable.Rows)
                {
                    DBBook bookToAdd = new DBBook();
                    bookToAdd.BookID = Convert.ToInt32(row["BookID"]);
                    bookToAdd.Name = Convert.ToString(row["Name"].ToString());
                    bookToAdd.Price = Convert.ToDecimal(row["Price"]);
                    bookToAdd.Category = row["Category"].ToString();
                    bookToAdd.Status = row["Status"].ToString();
                    bookToAdd.Author = row["Author"].ToString();
                    bookToAdd.Description = row["Description"].ToString();
                    books.Add(bookToAdd);
                }
            }

            return books;
        }

        /// <summary>
        /// will update a book based on the book object which is passed through
        /// </summary>
        /// <param name="conn">Connection to the Database</param>
        /// <param name="book">Book object to update</param>
        /// <returns></returns>
        public bool UpdateBook(SQLiteConnection conn, DBBook book)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE Book SET ");
            sb.Append("Name = '" + book.Name + "', ");
            sb.Append("Price = " + book.Price + ", ");
            sb.Append("Category = '" + book.Category + "', ");
            sb.Append("Status = '" + book.Status + "', ");
            sb.Append("Author = '" + book.Author + "', ");
            sb.Append("Description = '" + book.Description + "', ");
            sb.Append("WHERE BookId = " + book.BookID);

            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }

        public DBBook RetrieveBook(SQLiteConnection conn, int bookID)
        {
            DBBook book = null;
            string query = "SELECT * FROM Book WHERE BookID = " + bookID;

            // Create a data adapter
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                // Create a DataSet to hold the result
                DataSet dataSet = new DataSet();

                // Fill the DataSet with the result of the query
                adapter.Fill(dataSet);

                // You can access the result in tables within the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Process the data as needed
                foreach (DataRow row in dataTable.Rows)
                {
                    DBBook bookToAdd = new DBBook();
                    bookToAdd.BookID = Convert.ToInt32(row["BookID"]);
                    bookToAdd.Name = Convert.ToString(row["Name"].ToString());
                    bookToAdd.Price = Convert.ToDecimal(row["Price"]);
                    bookToAdd.Category = row["Category"].ToString();
                    bookToAdd.Status = row["Status"].ToString();
                    bookToAdd.Author = row["Author"].ToString();
                    bookToAdd.Description = row["Description"].ToString();
                    return (bookToAdd);
                }
            }
            return book;
        }


    }

    /// <summary>
    /// Class to interface with the magazine table in the database.
    /// </summary>
    public class DBMagazine
    {
        public int MagazineID { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public String Category { get; set; }
        public String Status { get; set; }
        public string Publisher { get; set; }
        public String Description { get; set; }

        public DBMagazine(string name, decimal price, string catgegory, string status, string publisher, string description)
        {
            Name = name;
            Price = price;
            Category = catgegory;
            Status = status;
            Publisher = publisher;
            Description = description;
        }

        public DBMagazine()
        {
            MagazineID = 0;
            Name = string.Empty;
            Price = 0;
            Category = string.Empty;
            Status = string.Empty;
            Publisher = string.Empty;
            Description = string.Empty;
        }


        /// <summary>
        /// Inserts a magazine into the database.
        /// </summary>
        /// <param name="conn">Connection to the database</param>
        /// <returns>True if a magazine was inserted correctly.</returns>
        public bool InsertMagazine(SQLiteConnection conn)
        {
            //Create an insert command string for the database. 
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Magazine(");
            sb.Append("Name, ");
            sb.Append("Price, ");
            sb.Append("Category, ");
            sb.Append("Status, ");
            sb.Append("Publisher, ");
            sb.Append("Description, ");
            sb.Append("Quantity) ");
            sb.Append("VALUES (");
            sb.Append("'" + Name + "', ");
            sb.Append(Price + ", ");
            sb.Append("'" + Category + "', ");
            sb.Append("'" + Status + "', ");
            sb.Append("'" + Publisher + "', ");
            sb.Append("'" + Description + "') ");

            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }


        public DBMagazine RetrieveMagazine(SQLiteConnection conn, int magazineID)
        {
            DBMagazine magazine = null;
            string query = "SELECT * FROM Magazine WHERE MagazineID = " + magazineID;

            // Create a data adapter
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                // Create a DataSet to hold the result
                DataSet dataSet = new DataSet();

                // Fill the DataSet with the result of the query
                adapter.Fill(dataSet);

                // You can access the result in tables within the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Process the data as needed
                foreach (DataRow row in dataTable.Rows)
                {
                    DBMagazine magazineToAdd = new DBMagazine();
                    magazineToAdd.MagazineID = Convert.ToInt32(row["MagazineID"]);
                    magazineToAdd.Name = Convert.ToString(row["Name"].ToString());
                    magazineToAdd.Price = Convert.ToDecimal(row["Price"]);
                    magazineToAdd.Category = row["Category"].ToString();
                    magazineToAdd.Status = row["Status"].ToString();
                    magazineToAdd.Publisher = row["Publisher"].ToString();
                    magazineToAdd.Description = row["Description"].ToString();
                    return (magazineToAdd);
                }
            }
            return magazine;
        }

        /// <summary>
        /// will update a book based on the book object which is passed through
        /// </summary>
        /// <param name="conn">Connection to the Database</param>
        /// <param name="book">Book object to update</param>
        /// <returns></returns>
        public bool UpdateMagazine(SQLiteConnection conn, DBMagazine Mag)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE Magazine SET ");
            sb.Append("Name = '" + Mag.Name + "', ");
            sb.Append("Price = " + Mag.Price + ", ");
            sb.Append("Category = '" + Mag.Category + "', ");
            sb.Append("Status = '" + Mag.Status + "', ");
            sb.Append("Publisher = '" + Mag.Publisher + "', ");
            sb.Append("Description = '" + Mag.Description + "' ");
            sb.Append("WHERE MagazineID = " + Mag.MagazineID);

            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }

        public List<DBMagazine> RetreiveMagazines(SQLiteConnection conn)
        {
            List<DBMagazine> mags = new List<DBMagazine>();
            // Your SELECT * FROM query
            string query = "SELECT * FROM Magazine";

            // Create a data adapter
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                // Create a DataSet to hold the result
                DataSet dataSet = new DataSet();

                // Fill the DataSet with the result of the query
                adapter.Fill(dataSet);

                // You can access the result in tables within the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Process the data as needed
                foreach (DataRow row in dataTable.Rows)
                {
                    DBMagazine magToAdd = new DBMagazine();
                    magToAdd.MagazineID = Convert.ToInt32(row["MagazineID"]);
                    magToAdd.Name = Convert.ToString(row["Name"].ToString());
                    magToAdd.Price = Convert.ToDecimal(row["Price"]);
                    magToAdd.Category = row["Category"].ToString();
                    magToAdd.Status = row["Status"].ToString();
                    magToAdd.Publisher = row["Publisher"].ToString();
                    magToAdd.Description = row["Description"].ToString();
                    mags.Add(magToAdd);
                }
            }

            return mags;
        }

        public bool DeleteMagazine(SQLiteConnection conn, int MagazineID)
        {
            bool ret = false;
            string query = "Delete From Magazine where MagazineID = " + MagazineID.ToString();
            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }
    }

    /// <summary>
    /// Class to interface with the Customer table in the database.
    /// </summary>
    public class DBCustomer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        public DBCustomer() 
        { 
            CustomerID = 0;
            Name = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            Region = string.Empty;
            PostCode = string.Empty;
            Country = string.Empty;
        }

        public DBCustomer (string name, string username, string password, string email, string address, string city, string region, string postcode, string country)
        {
            this.Name = name;
            this.Username = username;
            this.Email = email;
            this.Address = address;
            this.City = city;
            this.Region = region;
            this.PostCode = postcode;
            this.Country = country;
            this.Password = password;
        }

        public List<DBCustomer> RetreiveCustomers(SQLiteConnection conn)
        {
            List<DBCustomer> customers = new List<DBCustomer>();
            // Your SELECT * FROM query
            string query = "SELECT * FROM Customer";

            // Create a data adapter
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                // Create a DataSet to hold the result
                DataSet dataSet = new DataSet();

                // Fill the DataSet with the result of the query
                adapter.Fill(dataSet);

                // You can access the result in tables within the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Process the data as needed
                foreach (DataRow row in dataTable.Rows)
                {
                    DBCustomer customerToAdd = new DBCustomer();
                    customerToAdd.CustomerID = Convert.ToInt32(row["CustomerID"]);
                    customerToAdd.Name = Convert.ToString(row["Name"].ToString());
                    customerToAdd.Username = Convert.ToString(row["Username"]);
                    customerToAdd.Password = row["Password"].ToString();
                    customerToAdd.Email = row["Email"].ToString();
                    customerToAdd.Address = row["Address"].ToString();
                    customerToAdd.City = row["city"].ToString();
                    customerToAdd.Region = row["Region"].ToString();
                    customerToAdd.PostCode = row["Postcode"].ToString();
                    customerToAdd.Country = row["Country"].ToString();
                    customers.Add(customerToAdd);
                }
            }

            return customers;
        }

        public DBCustomer RetrieveCustomer(SQLiteConnection conn, int customerID)
        {
            DBCustomer book = null;
            string query = "SELECT * FROM Customer WHERE CustomerID = " + customerID;

            // Create a data adapter
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                // Create a DataSet to hold the result
                DataSet dataSet = new DataSet();

                // Fill the DataSet with the result of the query
                adapter.Fill(dataSet);

                // You can access the result in tables within the DataSet
                DataTable dataTable = dataSet.Tables[0];

                // Process the data as needed
                foreach (DataRow row in dataTable.Rows)
                {
                    DBCustomer customerToAdd = new DBCustomer();
                    customerToAdd.CustomerID = Convert.ToInt32(row["CustomerID"]);
                    customerToAdd.Name = Convert.ToString(row["Name"].ToString());
                    customerToAdd.Username = Convert.ToString(row["Username"]);
                    customerToAdd.Password = row["Password"].ToString();
                    customerToAdd.Email = row["Email"].ToString();
                    customerToAdd.Address = row["Address"].ToString();
                    customerToAdd.City = row["city"].ToString();
                    customerToAdd.Region = row["Region"].ToString();
                    customerToAdd.PostCode = row["Postcode"].ToString();
                    customerToAdd.Country = row["Country"].ToString();
                    return (customerToAdd);
                }
            }
            return book;
        }

        /// <summary>
        /// will update a Customer based on the book object which is passed through
        /// </summary>
        /// <param name="conn">Connection to the Database</param>
        /// <param name="book">Customer object to update</param>
        /// <returns></returns>
        public bool UpdateCustomer(SQLiteConnection conn, DBCustomer customer)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE Customer SET ");
            sb.Append("Name = '" + customer.Name + "', ");
            sb.Append("Username = '" + customer.Username + "', ");
            sb.Append("Password = '" + customer.Password + "', ");
            sb.Append("Email = '" + customer.Email + "', ");
            sb.Append("Address = '" + customer.Address + "', ");
            sb.Append("City = '" + customer.City + "', ");
            sb.Append("Region = '" + customer.Region + "', ");
            sb.Append("Postcode = '" + customer.PostCode + "', ");
            sb.Append("Country = '" + customer.Country + "' ");
            sb.Append("WHERE CustomerID = " + customer.CustomerID);

            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }
    }

    public class DBStock
    {
        public enum StockTypes
        {
            Book = 1,
            Magazine = 2
        }

        public int StockID { get; set; }
        public int StockType { get; set; }
        public int ItemID { get; set; }
        public int StockLevel { get; set; }

        public DBStock()
        {
            this.StockID = 0;
            this.StockType = 0;
            this.ItemID = 0;
            this.StockLevel = 0;
        }

        public DBStock(int stockID, int stockType, int itemID, int stockLevel)
        {
            StockID = stockID;
            StockType = stockType;
            ItemID = itemID;
            StockLevel = stockLevel;
        }

        public bool InsertStock(SQLiteConnection conn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO Stock(");
            sb.Append("StockType, ");
            sb.Append("ItemID, ");
            sb.Append("StockLevel )");
            sb.Append("VALUES (");
            sb.Append(StockType + ", ");
            sb.Append(ItemID + ", ");
            sb.Append(StockLevel + ") ");

            SQLiteCommand cmd = null;
            cmd = conn.CreateCommand();
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
            return true;
        }


    }
}

