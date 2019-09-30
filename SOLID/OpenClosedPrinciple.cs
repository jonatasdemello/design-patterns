//
// Open/Closed Principle
//
// a given software entity should be open for extension, but closed for modification
// Essentially, any given class (or module, or function, etc) 
// should allow for its functionality to be extended, 
// but not allow for modification to its own source code.

// we are given several Rectangles and need to calculate the total combined area of all of them
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
}
public class CombinedAreaCalculator
{
    public double Area(object[] shapes)
    {
        double area = 0;
        foreach (var shape in shapes)
        {
            if (shape is Rectangle)
            {
                Rectangle rectangle = (Rectangle)shape;
                area += rectangle.Width * rectangle.Height;
            }
        }
        return area;
    }
}

// if we have another shape
public class Circle
{
    public double Radius { get; set; }
}
// the class must change
public class CombinedAreaCalculator
{
    public double Area(object[] shapes)
    {
        double area = 0;
        foreach (var shape in shapes)
        {
            if (shape is Rectangle)
            {
                Rectangle rectangle = (Rectangle)shape;
                area += rectangle.Width * rectangle.Height;
            }
            if (shape is Circle)
            {
                Circle circle = (Circle)shape;
                area += (circle.Radius * circle.Radius) * Math.PI;
            }
        }
        return area;
    }
}

//Let's create an abstract class that all the shapes can inherit from:

public abstract class Shape
{
    public abstract double Area();
}

// our abstract Shape class has a method for Area. 
// We're moving the dependency for calculating the area from one centralized class (CombinedAreaCalculator) to the individual shapes.

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
public class Product
{
  public string Name;
  public Color Color;
  public Size Size;
  public Product(string name, Color color, Size size)
  {
    // obvious things here
  }
}
// Now, we want to provide certain filtering capabilities for a given set of products.
public class ProductFilter
{
	// this is using list
	public IEnumerable<Product> FilterByColorList (IEnumerable<Product> products, Color color)
	{
		var list = new List<Product>();
		foreach (var p in products)
			if (p.Color == color)
				list.Add(p);	  
		return list;
	}
	// this is the same using yeld:
	public IEnumerable<Product> FilterByColor (IEnumerable<Product> products, Color color)
	{
		foreach (var p in products)
			if (p.Color == color)
				yield return p;
	}
	public IEnumerable<Product> FilterBySize (IEnumerable<Product> products, Size size)
	{
		foreach (var p in products)
			if (p.Size == size)
				yield return p;
	}
	public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
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
// This is done by defining, you guessed it, an IFilter<T>:
public interface IFilter<T>
{
	IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}
// Again, all we are doing is specifying the signature for a method called 
// Filter() that takes all the items and a specification, and returns only 
// those items that conform to the specification.

public class BetterFilter : IFilter<Product>
{
  public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
  {
    foreach (var i in items)
      if (spec.IsSatisfied(i))
        yield return i;
  }
}

// To make a color filter, you make a ColorSpecification:
public class ColorSpecification : ISpecification<Product>
{
  private Color color;
  public ColorSpecification(Color color)
  {
    this.color = color;
  }
  public bool IsSatisfied(Product p)
  {
    return p.Color == color;
  }
}

// Armed with this specification, and given a list of products, we can now filter them as follows:
public void main()
{
	var apple = new Product("Apple", Color.Green, Size.Small);
	var tree = new Product("Tree", Color.Green, Size.Large);
	var house = new Product("House", Color.Blue, Size.Large);

	Product[] products = {apple, tree, house};

	var pf = new ProductFilter();

	WriteLine("Green products:");
	foreach (var p in pf.FilterByColor(products, Color.Green))
	  WriteLine($" - {p.Name} is green");
}  
// This code gets us “Apple” and “Tree” because they are both green. Now, 
// the only thing we have not implemented so far is searching for size and color 
// (or, indeed, explaining how you would search for size or color, or mix different 
// criteria). The answer is that you simply make a composite specification (or a 
// combinator). For example, for the logical AND, you can make it as follows:

public class AndSpecification<T> : ISpecification<T>
{
  private readonly ISpecification<T> first, second;
  
  public AndSpecification(ISpecification<T> first, ISpecification<T> second)
  {
    this.first = first;
    this.second = second;
  }
  public override bool IsSatisfied(T t)
  {
    return first.IsSatisfied(t) && second.IsSatisfied(t);
  }
}

// Now, you are free to create composite conditions on the basis of 
// simpler ISpecifications. Reusing the green specification we made earier, 
// finding something green and big is now as simple as this:

foreach (var p in bf.Filter(products, 
	new AndSpecification<Product>(new ColorSpecification(Color.Green), new SizeSpecification(Size.Large)) ))
{
  WriteLine($"{p.Name} is large");
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

public abstract class ISpecification<T>
{
  public abstract bool IsSatisfied(T p);
  
  public static ISpecification<T> operator &(ISpecification<T> first, ISpecification<T> second)
  {
    return new AndSpecification<T>(first, second);
  }
}

// If you now avoid making extra variables for size and color 
// specifications, the composite specification can be reduced to a single line:

var largeGreenSpec = new ColorSpecification(Color.Green)
                   & new SizeSpecification(Size.Large);

// Naturally, you can take this approach to extreme by defining extension 
// methods on all pairs of possible specifications:

public static class CriteriaExtensions
{
   public static AndSpecification<Product> And(this Color color, Size size)
  {
    return new AndSpecification<Product>(
      new ColorSpecification(color),
      new SizeSpecification(size));
  }
}
// with the subsequent use:
var largeGreenSpec = Color.Green.And(Size.Large);


