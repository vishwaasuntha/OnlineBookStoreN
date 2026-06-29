namespace OnlineBookStore.Models
{
    public class CardPayment : Payment
    {
        public override void Pay()
        {
            Console.WriteLine("Card payment processed");
        }
    }
}
