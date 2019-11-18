using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SOLID
{
    //
    // The DRY principle
    //
    // The DRY principle states that every piece of knowledge must have 
    // a single, unambiguous, authoritative representation within a system.

    public class ProductNotDry
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class CustomerNotDry
    {
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

    public class ProductDry : NamedEntity
    {
        /* Other members */
    }

    public class CustomerDry : NamedEntity
    {
        /* Other members */
    }
}

