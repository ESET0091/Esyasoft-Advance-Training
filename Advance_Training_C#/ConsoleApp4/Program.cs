namespace SingletonDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Singleton singleton1 = Singleton.getInstance();
            Singleton singleton2 = Singleton.getInstance();
            if (singleton1 == singleton2)
            {
                Console.WriteLine("Both instance are same");
            }

            string greet = singleton1.greetings();
            Console.WriteLine(greet);
        }
    }
}
