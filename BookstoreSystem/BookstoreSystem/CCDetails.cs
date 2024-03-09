using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreSystem
{
    public class CCDetails
    {
        // Properties
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CVV { get; set; }

        // Constructor
        public CCDetails(string cardNumber, string cardHolderName, DateTime expiryDate, int cvv)
        {
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpiryDate = expiryDate;
            CVV = cvv;
        }

        // Method to display credit card details
        public void DisplayCCDetails()
        {
            Console.WriteLine($"Card Number: {CardNumber}");
            Console.WriteLine($"Card Holder Name: {CardHolderName}");
            Console.WriteLine($"Expiry Date: {ExpiryDate}");
            Console.WriteLine($"CVV: {CVV}");
        }
    }
}
