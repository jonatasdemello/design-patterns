//
// The DRY principle
//
// The DRY principle states that every piece of knowledge must have 
// a single, unambiguous, authoritative representation within a system.

public class Product
{
    /* Other members */
    public string Name { get; set; }
 
    public override string ToString()
    {
        return Name;
    }
}
 
public class Customer
{
    /* Other members */
    public string Name { get; set; }
 
    public override string ToString()
    {
        return Name;
    }
}

// can be replaced by

public class NamedEntity
{
    public string Name { get; set; }
 
    public override string ToString()
    {
        return Name;
    }
}
 
public class Product : NamedEntity
{
    /* Other members */
}
 
public class Customer : NamedEntity
{
    /* Other members */
}
