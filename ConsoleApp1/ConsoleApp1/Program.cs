using System;
using System.Collections.Generic;

namespace DesignPatterns
{
    // ----------------- Singleton -----------------
    public class Logger
    {
        private static Logger _instance;
        private Logger() { }

        public static Logger Instance => _instance ??= new Logger();

        public void Log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }
    }

    // ----------------- Factory Method -----------------
    public abstract class Animal
    {
        public abstract void Speak();
    }

    public class Dog : Animal
    {
        public override void Speak() => Console.WriteLine("Woof!");
    }

    public class Cat : Animal
    {
        public override void Speak() => Console.WriteLine("Meow!");
    }

    public class AnimalFactory
    {
        public static Animal CreateAnimal(string type)
        {
            return type.ToLower() switch
            {
                "dog" => new Dog(),
                "cat" => new Cat(),
                _ => throw new ArgumentException("Unknown animal type")
            };
        }
    }

    // ----------------- Observer -----------------
    public interface IObserver
    {
        void Update(string message);
    }

    public class User : IObserver
    {
        private string _name;

        public User(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{_name} отримав повідомлення: {message}");
        }
    }

    public class NotificationService
    {
        private List<IObserver> _observers = new();

        public void Subscribe(IObserver observer) => _observers.Add(observer);

        public void Unsubscribe(IObserver observer) => _observers.Remove(observer);

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }
    
    // ----------------- Decorator -----------------
    public interface INotifier
    {
        void Send(string message);
    }

    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"Відправлено email: {message}");
        }
    }

    public class SmsNotifier : INotifier
    {
        private INotifier _notifier;

        public SmsNotifier(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void Send(string message)
        {
            _notifier.Send(message);
            Console.WriteLine($"Відправлено SMS: {message}");
        }
    }

    // ----------------- Strategy -----------------
    public interface ISortingStrategy
    {
        void Sort(List<int> list);
    }

    public class BubbleSort : ISortingStrategy
    {
        public void Sort(List<int> list)
        {
            Console.WriteLine("BubbleSort виконується...");
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
                    }
                }
            }
        }
    }

    public class QuickSort : ISortingStrategy
    {
        public void Sort(List<int> list)
        {
            Console.WriteLine("QuickSort виконується...");
            list.Sort();
        }
    }

    public class Sorter
    {
        private ISortingStrategy _strategy;

        public Sorter(ISortingStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(ISortingStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Sort(List<int> list)
        {
            _strategy.Sort(list);
        }
    }
    
    // Program
    class Program
    {
        static void Main()
        {
            // Singleton
            Logger.Instance.Log("Це повідомлення від Singleton.");

            // Factory Method
            Animal dog = AnimalFactory.CreateAnimal("dog");
            dog.Speak();

            Animal cat = AnimalFactory.CreateAnimal("cat");
            cat.Speak();

            // Observer
            NotificationService service = new NotificationService();
            User user1 = new User("Іван");
            User user2 = new User("Оксана");

            service.Subscribe(user1);
            service.Subscribe(user2);
            service.Notify("Нова подія на сайті!");

            // Decorator
            INotifier notifier = new EmailNotifier();
            notifier = new SmsNotifier(notifier);
            notifier.Send("Ваше замовлення доставлено.");

            // Strategy
            List<int> numbers = new List<int> { 5, 2, 9, 1, 5, 6 };
            Sorter sorter = new Sorter(new BubbleSort());
            sorter.Sort(numbers);

            sorter.SetStrategy(new QuickSort());
            sorter.Sort(numbers);

            foreach (var num in numbers)
            {
                Console.WriteLine(num);
            }
        }
    }
}
