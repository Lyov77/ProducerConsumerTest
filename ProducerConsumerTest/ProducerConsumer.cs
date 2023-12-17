using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumerTest
{
    internal class ProducerConsumer
    {
        static object objToLock = new object();
        const int QUEUEMAXCOUNT = 50;

        static Queue<int> queue = new Queue<int>(QUEUEMAXCOUNT);

        public void Producer()
        {
            Random rand = new Random();
            lock (objToLock)
            {
                while (true)
                {
                    int item = rand.Next(1, QUEUEMAXCOUNT);
                    for (int i = 0; i < item; i++)
                    {
                        if (queue.Count >= QUEUEMAXCOUNT)
                        {
                            Console.WriteLine("Queue is full. Waiting for consuming");
                            Monitor.Wait(objToLock);
                        }
                        queue.Enqueue(i);
                        Console.WriteLine($"Item {i + 1} produced");
                    }
                    Console.WriteLine($"Queue count is {queue.Count}");
                    Monitor.PulseAll(objToLock);
                }
            }
        }
        public void Consumer()
        {
            Random rand = new Random();
            lock (objToLock)
            {
                while (true)
                {

                    int item = rand.Next(1, QUEUEMAXCOUNT);
                    for (int i = 0; i < item; i++)
                    {
                        if (queue.Count == 0)
                        {
                            Console.WriteLine("Queue is empty. Waiting to produce");
                            Monitor.Wait(objToLock);
                        }
                        queue.Dequeue();
                        Console.WriteLine($"Item {i + 1} consumed");
                    }
                    Console.WriteLine($"Queue count is {queue.Count}");
                    Monitor.PulseAll(objToLock);
                }
            }
        }

        public void CloseProgram()
        {
            lock (objToLock)
            {

                while (queue.Count > 0)
                {
                    int item = queue.Dequeue();
                    Console.WriteLine($"Consumed on close: {item}");
                }
                Console.WriteLine("Program closed.");
                Environment.Exit(0);
            }
        }
    }
}
