using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Creational
{
    // Singleton is a Creational design pattern in which a class is guaranteed to only ever have exactly one instance, 
    // with that instance being globally accessible.
    public class Singleton
    {

        public void testSingleton()
        {
            var bell = TheBell.GetInstance;
            var otherBell = TheBell.GetInstance;

            Console.WriteLine(bell.GetHashCode());
            Console.WriteLine(otherBell.GetHashCode());
        }
    }

    public sealed class TheBell
    {
        private static TheBell bellConnection;
        private static object syncRoot = new Object();
        private TheBell()
        {
            // this is private, so we avoid creating new objects
        }

        // We implement this method to ensure thread safety for our singleton.
        public static TheBell GetInstance
        {
            get
            {
                lock (syncRoot)
                {
                    if (bellConnection == null)
                    {
                        bellConnection = new TheBell();
                    }
                }
                return bellConnection;
            }
        }

        public void Ring()
        {
            Console.WriteLine("Ding! Order up!");
        }
    }
}
