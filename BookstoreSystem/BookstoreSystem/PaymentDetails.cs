using System;

namespace BookstoreSystem
{
    public class PaymentDetails
    {
        // Properties
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; } // Added CustomerId property
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        // Constructor
        public PaymentDetails(int paymentId, int orderId, int customerId, decimal amount, DateTime paymentDate)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            CustomerId = customerId; // Initialize CustomerId
            Amount = amount;
            PaymentDate = paymentDate;
        }

        // Method to display payment details
        public void DisplayPaymentDetails()
        {
            Console.WriteLine($"Payment ID: {PaymentId}");
            Console.WriteLine($"Order ID: {OrderId}");
            Console.WriteLine($"Customer ID: {CustomerId}"); // Display CustomerId
            Console.WriteLine($"Amount: {Amount:C}");
            Console.WriteLine($"Payment Date: {PaymentDate}");
        }
    }
}
