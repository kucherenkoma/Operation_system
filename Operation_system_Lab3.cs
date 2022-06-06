using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pract3
{
    class Program
    {
        static readonly int nQueue = 200;   
        static Random randomNum;
        static Queue<int> queue;
        static Thread[] manufacturers;
        static Thread[] consumers;
        static Mutex mutex;
        static bool manufacturersIsOver;

        static void Manufacturer()
        {
            int value;
            while (!manufacturersIsOver)
            {
                mutex.WaitOne();
                if (queue.Count < nQueue)
                {
                    value = randomNum.Next(1, 100);
                    queue.Enqueue(value);
                    Console.WriteLine(Thread.CurrentThread.Name + "В конец очереди добавленно число " + value + "\nДлина очереди: " + queue.Count + "\n");
                }
                mutex.ReleaseMutex();
            }
            Console.WriteLine(Thread.CurrentThread.Name + "Закончил работу\n");
        }

        static void Dequeue()
        {
            mutex.WaitOne();
            if (queue.Count > 0)
            {
                int value = queue.Dequeue();
                Console.WriteLine(Thread.CurrentThread.Name + "Из начала очереди удаленно число " + value + "\nДлина очереди: " + queue.Count + "\n");
            }
            mutex.ReleaseMutex();
        }


        static void Consumer()
        {
            while (true)
            {
                if (!manufacturersIsOver)
                {
                    if (queue.Count >= 100 || queue.Count == 0) {Thread.Sleep(1000);}
                    else if (queue.Count <= 80) {Dequeue();}
                }
                else if (queue.Count == 0) {break;}
                else {Dequeue();}
            }
            Console.WriteLine(Thread.CurrentThread.Name + "Закончил работу\n");
        }


        static void Main(string[] args)
        {
            int nGenerator = 3, nConsumer = 2;
            manufacturersIsOver = false;
            queue = new Queue<int>();
            mutex = new Mutex();
            randomNum = new Random();
            manufacturers = new Thread[nGenerator];
            consumers = new Thread[nConsumer];

            Console.WriteLine("Нажмите 'q', чтобы остановить потоки производителей.");

            for (int i = 0; i < nGenerator; i++)
            {
                manufacturers[i] = new Thread(new ThreadStart(Manufacturer));
                manufacturers[i].Name = "Производитель #" + (i + 1) + "\n";
                manufacturers[i].Start();
            }
            for (int i = 0; i < nConsumer; i++)
            {
                consumers[i] = new Thread(new ThreadStart(Consumer));
                consumers[i].Name = "Потребитель #" + (i + 1) + "\n";
                consumers[i].Start();
            }

            char key = ' ';
            while (key != 'q')
            {
                key = (char)Console.Read();
            }
            manufacturersIsOver = true;

            Console.ReadKey();
        }
    }
}
