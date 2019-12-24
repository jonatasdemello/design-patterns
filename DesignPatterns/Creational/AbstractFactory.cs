using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Creational
{
    //The Abstract Factory Pattern is a Creational pattern in which interfaces are defined for creating
    // families of related objects without specifying their actual implementations.
    // Abstract Factory allows us to generically define families of related objects,
    // leaving the actual concretions for those objects to be implemented as needed.

    abstract class SandwichA { }
    abstract class DessertA { }


    // The AbstractFactory class, which defines methods for creating abstract objects.
    abstract class RecipeFactory
    {
        public abstract SandwichA CreateSandwich();
        public abstract DessertA CreateDessert();
    }

    // A ConcreteProduct
    class BLT : SandwichA { }
    class CremeBrulee : DessertA { }
    class GrilledCheese : SandwichA { }
    class IceCreamSundae : DessertA { }

    // A ConcreteFactory which creates concrete objects by implementing the abstract factory's methods.
    class AdultCuisineFactory : RecipeFactory
    {
        public override SandwichA CreateSandwich()
        {
            return new BLT();
        }
        public override DessertA CreateDessert()
        {
            return new CremeBrulee();
        }
    }
    class KidCuisineFactory : RecipeFactory
    {
        public override SandwichA CreateSandwich()
        {
            return new GrilledCheese();
        }

        public override DessertA CreateDessert()
        {
            return new IceCreamSundae();
        }
    }


    public class AbstractFactory
    {
        public void TestMain()
        {
            TestFactory('A');
            TestFactory('C');
        }
        public void TestFactory(char input)

        {
            RecipeFactory factory;
            switch (input)
            {
                case 'A':
                    factory = new AdultCuisineFactory();
                    break;

                case 'C':
                    factory = new KidCuisineFactory();
                    break;

                default:
                    throw new NotImplementedException();

            }

            var sandwich = factory.CreateSandwich();
            var dessert = factory.CreateDessert();

            Console.WriteLine("\nSandwich: " + sandwich.GetType().Name);
            Console.WriteLine("Dessert: " + dessert.GetType().Name);
        }
    }
}
