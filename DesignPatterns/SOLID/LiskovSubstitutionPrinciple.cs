using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SOLID
{
    //
    // Liskov Substitution Principle(LSP)
    //
    // states that we should be able to treat a child class as though it were the parent class.
    // Essentially this means that all derived classes should retain the functionality 
    // of their parent class and cannot replace any functionality the parent provides. 

    public class Ellipse_Lsp
    {
        public double MajorAxis { get; set; }
        public double MinorAxis { get; set; }
        public virtual void SetMajorAxis(double majorAxis)
        {
            MajorAxis = majorAxis;
        }
        public virtual void SetMinorAxis(double minorAxis)
        {
            MinorAxis = minorAxis;
        }
        public virtual double Area()
        {
            return MajorAxis * MinorAxis * Math.PI;
        }
    }

    public class Circle_Lsp : Ellipse_Lsp
    {
        public override void SetMajorAxis(double majorAxis)
        {
            base.SetMajorAxis(majorAxis);
            this.MinorAxis = majorAxis; // In a cirle, each axis is identical
        }
    }

    public class TestCircle_Lsp
    {
        public void methodTest()
        {
            // See the problem now? 
            // If we set both axes, attempting to calculate the area gives the wrong result.

            Circle_Lsp circle = new Circle_Lsp();
            circle.SetMajorAxis(5);
            circle.SetMinorAxis(4);
            var area = circle.Area(); //5*4 = 20, but we expected 5*5 = 25
            Console.WriteLine(area);
        }
    }
    // One solution might be to have Circle implement SetMinorAxis as well:
    public class Circle_v1_Lsp : Ellipse_Lsp
    {
        public override void SetMajorAxis(double majorAxis)
        {
            base.SetMajorAxis(majorAxis);
            this.MinorAxis = majorAxis; //In a cirle, each axis is identical
        }
        public override void SetMinorAxis(double minorAxis)
        {
            base.SetMinorAxis(minorAxis);
            this.MajorAxis = minorAxis;
        }
        public override double Area()
        {
            return base.Area();
        }
    }
    // Another solution, one with less code overall, might be to treat Circle as an entirely separate class:
    public class Circle_v2_Lsp
    {
        public double Radius { get; set; }
        public void SetRadius(double radius)
        {
            this.Radius = radius;
        }
        public double Area()
        {
            return this.Radius * this.Radius * Math.PI;
        }
    }

}

