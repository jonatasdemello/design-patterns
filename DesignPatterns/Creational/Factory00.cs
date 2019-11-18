using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Creational
{
    public interface IAirConditioner
    {
        void Operate();
    }
    //  concrete classes to implement this interface
    public class Cooling : IAirConditioner
    {
        private readonly double _temperature;

        public Cooling(double temperature)
        {
            _temperature = temperature;
        }

        public void Operate()
        {
            Console.WriteLine($"Cooling the room to the required temperature of {_temperature} degrees");
        }
    }
    public class Warming : IAirConditioner
    {
        private readonly double _temperature;

        public Warming(double temperature)
        {
            _temperature = temperature;
        }

        public void Operate()
        {
            Console.WriteLine($"Warming the room to the required temperature of {_temperature} degrees.");
        }
    }

    // Factory Classes
    public abstract class AirConditionerFactory
    {
        public abstract IAirConditioner Create(double temperature);
    }
    // concrete creator classes:
    public class CoolingFactory : AirConditionerFactory
    {
        public override IAirConditioner Create(double temperature) => new Cooling(temperature);
    }
    public class WarmingFactory : AirConditionerFactory
    {
        public override IAirConditioner Create(double temperature) => new Warming(temperature);
    }

    // Factory Execution
    public enum Actions
    {
        Cooling,
        Warming
    }
    public class AirConditioner
    {
        private readonly Dictionary<Actions, AirConditionerFactory> _factories;

        public AirConditioner()
        {
            _factories = new Dictionary<Actions, AirConditionerFactory>
                {
                    { Actions.Cooling, new CoolingFactory() },
                    { Actions.Warming, new WarmingFactory() }
                };
        }
        public static AirConditioner InitializeFactories() => new AirConditioner();
        public IAirConditioner ExecuteCreation(Actions action, double temperature) => _factories[action].Create(temperature);
    }

    // or, using reflection:
    public class AirConditionerRef
    {
        private readonly Dictionary<Actions, AirConditionerFactory> _factories;

        public AirConditionerRef()
        {
            _factories = new Dictionary<Actions, AirConditionerFactory>();

            foreach (Actions action in Enum.GetValues(typeof(Actions)))
            {
                var factory = (AirConditionerFactory)Activator.CreateInstance(Type.GetType("FactoryMethod." + Enum.GetName(typeof(Actions), action) + "Factory"));
                _factories.Add(action, factory);
            }
        }
        public static AirConditioner InitializeFactories() => new AirConditioner();
        public IAirConditioner ExecuteCreation(Actions action, double temperature) => _factories[action].Create(temperature);
    }

    public class TestFactoryAirConditioner
    {
        public void TestMain()
        {
            var factory1 = new AirConditioner().ExecuteCreation(Actions.Cooling, 22.5);
            factory1.Operate();

            var factory2 = new AirConditioner().ExecuteCreation(Actions.Warming, 28);
            factory2.Operate();

            AirConditioner
            .InitializeFactories()
            .ExecuteCreation(Actions.Cooling, 23)
            .Operate();
        }
    }
}
