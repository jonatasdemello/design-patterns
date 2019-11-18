using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Creational
{
    // The Factory Method pattern provides a manner in which we can instantiate objects, 
    // but the details of the creation of those instance are left to be defined by the instance classes themselves.

    abstract class Ingredient { }

    class Bread : Ingredient { }
    class Turkey : Ingredient { }
    class Lettuce : Ingredient { }
    class Mayonnaise : Ingredient { }
    
    abstract class Sandwich
    {
        private List<Ingredient> _ingredients = new List<Ingredient>();

        public Sandwich()
        {
            CreateIngredients();
        }

        // Factory method
        public abstract void CreateIngredients();

        public List<Ingredient> Ingredients
        {
            get { return _ingredients; }
        }
    }

    class TurkeySandwich : Sandwich
    {
        // the Concrete Creator class implements it
        public override void CreateIngredients()
        {
            Ingredients.Add(new Bread());
            Ingredients.Add(new Mayonnaise());
            Ingredients.Add(new Lettuce());
            Ingredients.Add(new Turkey());
            Ingredients.Add(new Turkey());
            Ingredients.Add(new Bread());
        }
    }
    class Veggie : Sandwich
    {
        public override void CreateIngredients()
        {
            Ingredients.Add(new Bread());
            Ingredients.Add(new Lettuce());
            Ingredients.Add(new Mayonnaise());
            Ingredients.Add(new Bread());
        }
    }
    public class TestRecipeFactory
    {
        public void TestMain()
        {
            //var turkeySandwich = new TurkeySandwich();
            //var veggie = new Veggie();

            List<Sandwich> sandwiches = new List<Sandwich>();
            sandwiches.Add(new TurkeySandwich());
            sandwiches.Add(new Veggie());

            foreach (var sandwich in sandwiches)
            {
                Console.WriteLine("\nSandwich: " + sandwich.GetType().Name + " ");
                foreach (var ingredient in sandwich.Ingredients)
                {
                    Console.WriteLine("Ingredient: " + ingredient.GetType().Name);
                }
            }
        }
    }
}
