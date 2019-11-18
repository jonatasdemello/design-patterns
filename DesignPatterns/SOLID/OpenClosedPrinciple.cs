using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SOLID
{
    //
    // Open/Closed Principle
    //
    // a given software entity should be open for extension, but closed for modification
    // Essentially, any given class (or module, or function, etc) 
    // should allow for its functionality to be extended, 
    // but not allow for modification to its own source code.

    // we are given several Rectangles and need to calculate the total combined area of all of them
    public class Rectangle_0
    {
        public double width { get; set; }
        public double height { get; set; }
    }
    public class CombinedAreaCalculator_0
    {
        public double Area(object[] shapes)
        {
            double area = 0;
            foreach (var shape in shapes)
            {
                if (shape is Rectangle_0)
                {
                    Rectangle_0 rectangle = (Rectangle_0)shape;
                    area += rectangle.width * rectangle.height;
                }
            }
            return area;
        }
    }
    // if we have another shape
    public class Circle_0
    {
        public double radius { get; set; }
    }
    // the class must change
    public class CombinedAreaCalculatorChange
    {
        public double Area(object[] shapes)
        {
            double area = 0;
            foreach (var shape in shapes)
            {
                if (shape is Rectangle_0)
                {
                    Rectangle_0 rectangle = (Rectangle_0)shape;
                    area += rectangle.width * rectangle.height;
                }
                if (shape is Circle_0)
                {
                    Circle_0 circle = (Circle_0)shape;
                    area += (circle.radius * circle.radius) * Math.PI;
                }
            }
            return area;
        }
    }

    // Now let's create an abstract class that all the shapes can inherit from:
    public abstract class Shape
    {
        public abstract double Area();
    }

    // Our abstract Shape class has a method for Area. 
    // We're moving the dependency for calculating the area from 
    // one centralized class (CombinedAreaCalculator) to the individual shapes.
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public override double Area()
        {
            return Width * Height;
        }
    }
    public class Circle : Shape
    {
        public double Radius { get; set; }
        public override double Area()
        {
            return Radius * Radius * Math.PI;
        }
    }
    public class Triangle : Shape
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public override double Area()
        {
            return Height * Width * 0.5;
        }
    }
    public class CombinedAreaCalculator
    {
        public double Area(Shape[] shapes)
        {
            double area = 0;
            foreach (var shape in shapes)
            {
                area += shape.Area();
            }
            return area;
        }
    }

    // Another exemple
    public enum Color
    {
        Red, Green, Blue
    }
    public enum Size
    {
        Small, Medium, Large, Yuge
    }
    public class Product_oc
    {
        public string Name;
        public Color Color;
        public Size Size;
        public Product_oc(string name, Color color, Size size)
        {
            Name = name;
            Color = color;
            Size = size;
        }
    }
    // Now, we want to provide certain filtering capabilities for a given set of products.
    public class ProductFilter_oc
    {
        // this is using list
        public IEnumerable<Product_oc> FilterByColorList(IEnumerable<Product_oc> products, Color color)
        {
            var list = new List<Product_oc>();
            foreach (var p in products)
                if (p.Color == color)
                    list.Add(p);
            return list;
        }
        // this is the same using yeld:
        public IEnumerable<Product_oc> FilterByColor(IEnumerable<Product_oc> products, Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }
        public IEnumerable<Product_oc> FilterBySize(IEnumerable<Product_oc> products, Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }
        public IEnumerable<Product_oc> FilterBySizeAndColor(IEnumerable<Product_oc> products, Size size, Color color)
        {
            foreach (var p in products)
                if (p.Size == size && p.Color == color)
                    yield return p;
        }
    }
    // our filtering process into two parts: 
    // a filter (a construct that takes all items and only returns some) 
    // and a specification (a predicate to apply to a data element).

    public interface ISpecification<T>
    {
        bool IsSatisfied(T item);
    }
    // Next up, we need a way of filtering based on ISpecification<T>: 
    // This is done by defining an IFilter<T>:
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }
    // Again, all we are doing is specifying the signature for a method called 
    // Filter() that takes all the items and a specification, and returns only 
    // those items that conform to the specification.

    public class BetterFilter : IFilter<Product_oc>
    {
        public IEnumerable<Product_oc> Filter(IEnumerable<Product_oc> items, ISpecification<Product_oc> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }
    }

    // To make a color filter, you make a ColorSpecification:
    public class ColorSpecification : ISpecification<Product_oc>
    {
        private Color color;
        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product_oc p)
        {
            return p.Color == color;
        }
    }

    public class Test
    {    
        // Armed with this specification, and given a list of products, we can now filter them as follows:
        public void TestMain()
        {
            var apple = new Product_oc("Apple", Color.Green, Size.Small);
            var tree = new Product_oc("Tree", Color.Green, Size.Large);
            var house = new Product_oc("House", Color.Blue, Size.Large);

            Product_oc[] products = { apple, tree, house };

            var pf = new ProductFilter_oc();

            Console.WriteLine("Green products:");
            foreach (var p in pf.FilterByColor(products, Color.Green))
                Console.WriteLine($" - {p.Name} is green");

            // This code gets us “Apple” and “Tree” because they are both green. Now, 
            // the only thing we have not implemented so far is searching for size and color 
            // (or, indeed, explaining how you would search for size or color, or mix different criteria). 
            // The answer is that you simply make a composite specification (or a 
            // combinator). For example, for the logical AND, you can make it as follows:

            // Now, you are free to create composite conditions on the basis of 
            // simpler ISpecifications. Reusing the green specification we made earier, 
            // finding something green and big is now as simple as this:

            //foreach (var p in pf.Filter(products,
            //    new AndSpecification<Product_oc>(new ColorSpecification(Color.Green), new SizeSpecification(Size.Large))))
            //{
            //    Console.WriteLine($"{p.Name} is large");
            //}

            //// with the subsequent use:
            //var largeGreenSpec = Color.Green.And(Size.Large);
        }
    }

    public abstract class ISpecification1<T>
    {
        public abstract bool IsSatisfied(T p);
        public static ISpecification1<T> operator &(ISpecification1<T> first, ISpecification1<T> second)
        {
            return new AndSpecification<T>(first, second);
        }
    }
    public class AndSpecification<T> : ISpecification1<T>
    {
        private readonly ISpecification1<T> first, second;
        public AndSpecification(ISpecification1<T> first, ISpecification1<T> second)
        {
            this.first = first;
            this.second = second;
        }
        public override bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }
    // Tree is large and green

    // This was a lot of code to do something seemingly simple, but the benefits 
    // are well worth it. The only really annoying part is having to specify the 
    // generic argument to AndSpecification—remember, unlike the color and 
    // size specifications, the combinator is not constrained to the Product type.
    // Keep in mind that, thanks to the power of C#, you can simply introduce 
    // an operator & (important: note the single ampersand here; && is a by- 
    // product) for two ISpecification<T> objects, thereby making the process 
    // of filtering by two (or more) criteria somewhat simpler. The only problem 
    // is that we need to change from an interface to an abstract class 
    // (feel free to remove the leading I from the name).



    // If you now avoid making extra variables for size and color 
    // specifications, the composite specification can be reduced to a single line:

    //var largeGreenSpec = new ColorSpecification(Color.Green)
    //                   & new SizeSpecification(Size.Large);

    // Naturally, you can take this approach to extreme by defining extension 
    // methods on all pairs of possible specifications:

    //public static class CriteriaExtensions
    //{
    //    public static AndSpecification<Product_oc> And(this Color color, Size size)
    //    {
    //        return new AndSpecification<Product_oc>(
    //          new ColorSpecification(color),
    //          new SizeSpecification(size));
    //    }
    //}

}
