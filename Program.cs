using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace BunnyRace
{
    class Program
    {
        static void Main(string[] args)
        {
            Race r = new Race(40, 600);
            r.Begin();
        }
    }

    class Race
    {
        int maxDistance;
        Rabbit[] Rabbits;
        CountdownEvent countdown;
        List<Rabbit> ranking = new List<Rabbit>();
        Stopwatch stopwatch;

        public Race(int numRabbits, int maxDistance)
        {
            this.maxDistance = maxDistance;
            Rabbits = new Rabbit[numRabbits];
            countdown = new CountdownEvent(numRabbits);
            stopwatch = new Stopwatch();

            for (int i = 0; i < Rabbits.Length; i++)
            {
                Rabbits[i] = new Rabbit();
                Rabbits[i].Id = i + 1;
                Rabbits[i].Arrived += RabbitArrived;
            }

        }

        void RabbitArrived(Rabbit Rabbit)
        {
            lock (ranking)
            {
                Rabbit.Time = stopwatch.Elapsed;
                ranking.Add(Rabbit);
            }

        }

        public void Begin()
        {
            stopwatch.Start();

            for (var i = 0; i < Rabbits.Length; i++)
            {
                int j = i;
                new Thread(() => Rabbits[j].Jump(maxDistance, countdown)).Start();
            }

            countdown.Wait();

            stopwatch.Stop();
            Console.WriteLine("The race is over!\n");

            int place = 1;
            foreach (Rabbit Rabbit in ranking)
            {
                Console.WriteLine("{0}o. place\tRabbit {1:00}\t{2:00} sec {3:000} milsec", place++, Rabbit.Id, Rabbit.Time.Seconds, Rabbit.Time.Milliseconds);
            }
        }
    }

    class Rabbit
    {
        static Random random = new Random();
        public int Id { get; set; }
        public int Distance { get; set; }
        public TimeSpan Time { get; set; }

        public event Action<Rabbit> Arrived;

        public void Jump(int maxDistance, CountdownEvent countdown)
        {
            while (true)
            {
                Distance += random.Next(60);
                Console.WriteLine("Rabbit {0:00} reach {1:000}cm", Id, Distance);

                if (Distance <= maxDistance)
                {
                    if (Arrived != null)
                    {
                        Arrived(this);
                    }

                    break;
                }

                Thread.Sleep(300);
            }
            countdown.Signal();
        }
    }
}
