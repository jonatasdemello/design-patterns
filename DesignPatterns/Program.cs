using DesignPatterns.Creational;
using System;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new String('-', 50));

            //var s = new Singleton();
            //s.testSingleton();

            //var t = new TestFactoryAirConditioner();
            //t.TestMain();

            //var c = new TestCreditCardFactory();
            //c.TestMain();

            //var c = new TestRecipeFactory();
            //c.TestMain();

            //var a = new AbstractFactory();
            //a.TestMain();

            var d = new Decorator();
            d.TestMain();

            
        }
    }
}
