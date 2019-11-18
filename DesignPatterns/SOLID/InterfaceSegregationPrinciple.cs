using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SOLID
{
    //
    // Interface Segregation Principle 
    //
    // states that no client code object should be forced to depend on methods it does not use.
    // Basically, each code object should only implement what it needs, 
    // and not be required to implement anything else.
    // the result is to have a lot of small, focused interfaces 
    // that define only what is needed by their implementations.

    public interface IProduct
    {
        int ID { get; set; }
        double Weight { get; set; }
        int Stock { get; set; }
        int Inseam { get; set; }
        int WaistSize { get; set; }
    }
    // any class that implements the interface has to implement it all
    public class Jeans : IProduct
    {
        public int ID { get; set; }
        public double Weight { get; set; }
        public int Stock { get; set; }
        public int Inseam { get; set; }
        public int WaistSize { get; set; }
    }
    public class BaseballCap : IProduct
    {
        public int ID { get; set; }
        public double Weight { get; set; }
        public int Stock { get; set; }
        public int Inseam { get; set; }
        public int WaistSize { get; set; }
        public int HatSize { get; set; }
    }

    // what properties do both BaseballCap and Jeans need? 
    // Those properties can become the new IProduct interface:

    public interface IProductDry
    {
        int ID { get; set; }
        double Weight { get; set; }
        int Stock { get; set; }
    }
    // We currently sell jeans, but Inseam and WaistSize can apply to 
    // any type of pants, so let's create an IPants interface:
    public interface IPantsDry
    {
        public int Inseam { get; set; }
        public int WaistSize { get; set; }
    }
    // We should be willing to bet that baseball caps won't be the only 
    // kinds of hats we'll sell, so we also make a focused IHat interface:
    public interface IHatDry
    {
        public int HatSize { get; set; }
    }
    //Now we can implement both Jeans and BaseballCap:
    public class JeansDry : IProductDry, IPantsDry
    {
        public int ID { get; set; }
        public double Weight { get; set; }
        public int Stock { get; set; }
        public int Inseam { get; set; }
        public int WaistSize { get; set; }
    }

    public class BaseballCapDry : IProductDry, IHatDry
    {
        public int ID { get; set; }
        public double Weight { get; set; }
        public int Stock { get; set; }
        public int HatSize { get; set; }
    }

    // Each class now has only properties that they need. Now we are upholding the Interface Segregation Principle!}
}
