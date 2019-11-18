using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Creational
{
    // The Template Method design pattern defines the outline or skeleton of an operation, but leaves the specific steps involved to be defined by subclasses.
    // In other words, the Template Method pattern defines in what order certain steps should occur, 
    // but can optionally leave the specific details of those steps to be implemented by other classes. 
    // Whereas Factory Method did something similar with creating objects, Template Method does this with the behavior of those objects.

    class TemplateMethod
    {

        // The AbstractClass participant which contains the template method.
        abstract class Bread
        {
            public abstract void MixIngredients();

            public abstract void Bake();

            public virtual void Slice()
            {
                Console.WriteLine("Slicing the " + GetType().Name + " bread!");
            }

            // The template method
            public void Make()
            {
                MixIngredients();
                Bake();
                Slice();
            }
        }

        class TwelveGrain : Bread
        {
            public override void MixIngredients()
            {
                Console.WriteLine("Gathering Ingredients for 12-Grain Bread.");
            }

            public override void Bake()
            {
                Console.WriteLine("Baking the 12-Grain Bread. (25 minutes)");
            }
        }

        class Sourdough : Bread
        {
            public override void MixIngredients()
            {
                Console.WriteLine("Gathering Ingredients for Sourdough Bread.");
            }

            public override void Bake()
            {
                Console.WriteLine("Baking the Sourdough Bread. (20 minutes)");
            }
        }

        class WholeWheat : Bread
        {
            public override void MixIngredients()
            {
                Console.WriteLine("Gathering Ingredients for Whole Wheat Bread.");
            }

            public override void Bake()
            {
                Console.WriteLine("Baking the Whole Wheat Bread. (15 minutes)");
            }
        }

        public void TestMain()
        {
            Sourdough sourdough = new Sourdough();
            sourdough.Make();

            TwelveGrain twelveGrain = new TwelveGrain();
            twelveGrain.Make();

            WholeWheat wholeWheat = new WholeWheat();
            wholeWheat.Make();

        }
    }
}
