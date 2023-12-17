namespace ProducerConsumerTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProducerConsumer prod = new ProducerConsumer();

            Thread producer = new Thread(prod.Producer);

            Thread consumer = new Thread(prod.Consumer);

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                prod.CloseProgram();
            };

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join(100);
        }
    }
}