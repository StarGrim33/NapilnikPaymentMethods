using System.Security.Cryptography;
using System.Text;

namespace Payment
{
    class Program
    {
        static void Main(string[] args)
        {
            MirPayment mirPayment = new MirPayment();
            VisaPayment visaPayment = new VisaPayment();
            MasterCardPayment masterCardPayment = new MasterCardPayment();
            Order order = new Order(1, 1000);

            Console.WriteLine(mirPayment.GetPayingLink(order));
            Console.WriteLine(visaPayment.GetPayingLink(order));
            Console.WriteLine(masterCardPayment.GetPayingLink(order));
        }
    }

    class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    interface IPaymentSystem
    {
        string URLLink { get; }

        public string GetPayingLink(Order order);
    }

    class MirPayment : IPaymentSystem
    {
        public string URLLink => "pay.system1.ru/order?amount=12000RUB&hash=";

        public string GetPayingLink(Order order)
        {
            var hashid = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString()));
            return URLLink + Convert.ToBase64String(hashid);
        }
    }

    class VisaPayment : IPaymentSystem
    {
        public string URLLink => "order.system2.ru/pay?hash =";

        public string GetPayingLink(Order order)
        {
            var hashid = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString()));
            return URLLink + Convert.ToBase64String(hashid) + order.Amount.ToString();
        }
    }

    class MasterCardPayment : IPaymentSystem
    {
        public string URLLink => "system3.com/pay?amount=12000&curency=RUB&hash=";

        private readonly string _secretKey = "HfdHKJSLDlewds23251";

        public string GetPayingLink(Order order)
        {
            var hashid = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString()));
            return URLLink + Convert.ToBase64String(hashid) + order.Amount.ToString() + _secretKey;
        }
    }
}