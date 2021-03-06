using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SOLID
{
    //
    // Dependency Inversion Principle
    //
    // is comprised of two rules:
    // - High-level modules should not depend on low-level modules.  Both should depend on abstractions.
    // - Abstractions should not depend on details.  Details should depend on abstractions.

    // This principle is primarily concerned with reducing dependencies among the code modules.  
    // We can think of it as needing the low-level objects to define contracts that the high-level objects can use, 
    // without the high-level objects needing to care about the specific implementation the low-level objects provide.

    // We are building an notifications client. 
    // We want to be able send both email and SMS text notifications

    public class Email_1
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public void SendEmail()
        {
            Console.WriteLine("Send email");
        }
    }

    public class SMS_1
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public void SendSMS()
        {
            Console.WriteLine("Send sms");
        }
    }

    public class Notification_1
    {
        private Email_1 _email;
        private SMS_1 _sms;

        public Notification_1()
        {
            _email = new Email_1();
            _sms = new SMS_1();
        }

        public void Send()
        {
            _email.SendEmail();
            _sms.SendSMS();
        }
    }

    // Notice that the Notification class, a higher-level class, has a dependency on both the Email class and the SMS class, which are lower-level classes.  
    // In other words, Notification is depending on the concrete implementation of both Email and SMS, not an abstraction of said implementation. 

    // (One trick you can use to determine how tightly coupled your code is is to look for the "new" keyword. 
    // Generally speaking, the more instances of new keyword you have, the more tightly coupled your code is.)

    // We need to introduce an abstraction, one that Notification can rely on and that Email and SMS can implement

    public interface IMessage
    {
        void SendMessage();
    }
    // Next, Email and SMS can implement the IMessage interface:
    public class Email : IMessage
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public void SendMessage()
        {
            Console.WriteLine("Send email");
        }
    }

    public class SMS : IMessage
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public void SendMessage()
        {
            Console.WriteLine("Send sms");
        }
    }
    // And, finally, we can make Notification depend on the abstraction IMessage rather than its concrete implementations:
    public class Notification
    {
        private ICollection<IMessage> _messages;

        public Notification(ICollection<IMessage> messages)
        {
            this._messages = messages;
        }

        public void Send()
        {
            foreach (var message in _messages)
            {
                message.SendMessage();
            }
        }
    }

    // **********************************************************
    // https://www.tutorialsteacher.com/ioc/dependency-injection
    // **********************************************************

    /*
    public class CustomerBusinessLogic
    {
        public CustomerBusinessLogic()
        {
        }
        public string GetCustomerName(int id)
        {
            DataAccess _dataAccess = DataAccessFactory.GetDataAccessObj();
            return _dataAccess.GetCustomerName(id);
        }
    }
    public class DataAccessFactory
    {
        public static DataAccess GetDataAccessObj()
        {
            return new DataAccess();
        }
    }
    public class DataAccess
    {
        public DataAccess()
        {
        }
        public string GetCustomerName(int id)
        {
            return "Dummy Customer Name"; // get it from DB in real app
        }
    }
    */
    // -------------------------------------------
    // high-level module (CustomerBusinessLogic)
    // low-level module (CustomerDataAccess)
    // dependent on an abstraction (ICustomerDataAccess)
    // the abstraction (ICustomerDataAccess) does not depend on details (CustomerDataAccess), but the details depend on the abstraction.

    public interface ICustomerDataAccess
    {
        public string GetCustomerName(int id);
    }
    public class CustomerDataAccess : ICustomerDataAccess
    {
        public CustomerDataAccess() { }
        public string GetCustomerName(int id)
        {
            return "Dummy Customer Name"; // get it from DB in real app
        }
    }
    public class DataAccessFactory
    {
        public static ICustomerDataAccess GetCustomerDataAccessObj()
        {
            return new CustomerDataAccess();
        }
    }
    public class CustomerBusinessLogic
    {
        private ICustomerDataAccess _custDataAccess;

        public CustomerBusinessLogic()
        {
            _custDataAccess = DataAccessFactory.GetCustomerDataAccessObj();
        }

        public string GetCustomerName(int id)
        {
            return _custDataAccess.GetCustomerName(id);
        }
    }

    public class ConstructorInjection
    {
        public class CustomerBusinessLogic
        {
            ICustomerDataAccess _dataAccess;
            public CustomerBusinessLogic(ICustomerDataAccess custDataAccess)
            {
                _dataAccess = custDataAccess;
            }
            public CustomerBusinessLogic()
            {
                _dataAccess = new CustomerDataAccess();
            }
            public string GetCustomerName(int id)
            {
                return _dataAccess.GetCustomerName(id);
            }
        }
        public class CustomerDataAccess : ICustomerDataAccess
        {
            public CustomerDataAccess() { }
            public string GetCustomerName(int id)
            {
                return "Dummy Customer Name";
            }
        }
        // Inject Dependency
        public class CustomerService
        {
            CustomerBusinessLogic _customerBL;
            // constructor
            public CustomerService()
            {
                // new object is created here
                _customerBL = new CustomerBusinessLogic(new CustomerDataAccess());
            }
            public string GetCustomerName(int id)
            {
                return _customerBL.GetCustomerName(id);
            }
        }
    }

    public class PropertyInjection
    {
        public class CustomerBusinessLogic
        {
            public CustomerBusinessLogic() { }
            // Property
            public string GetCustomerName(int id)
            {
                return DataAccess.GetCustomerName(id);
            }
            public ICustomerDataAccess DataAccess { get; set; }
        }
        public class CustomerService
        {
            CustomerBusinessLogic _customerBL;
            public CustomerService()
            {
                _customerBL = new CustomerBusinessLogic();
                _customerBL.DataAccess = new CustomerDataAccess();
            }
            public string GetCustomerName(int id)
            {
                return _customerBL.GetCustomerName(id);
            }
        }
    }

    public class MethodInjection
    {
        interface IDataAccessDependency
        {
            void SetDependency(ICustomerDataAccess customerDataAccess);
        }
        public class CustomerBusinessLogic : IDataAccessDependency
        {
            ICustomerDataAccess _dataAccess;
            public CustomerBusinessLogic()  { }
            public string GetCustomerName(int id)
            {
                return _dataAccess.GetCustomerName(id);
            }
            public void SetDependency(ICustomerDataAccess customerDataAccess)
            {
                _dataAccess = customerDataAccess;
            }
        }
        public class CustomerService
        {
            CustomerBusinessLogic _customerBL;
            public CustomerService()
            {
                _customerBL = new CustomerBusinessLogic();
                // method
                ((IDataAccessDependency)_customerBL).SetDependency(new CustomerDataAccess());
            }
            public string GetCustomerName(int id)
            {
                return _customerBL.GetCustomerName(id);
            }
        }
    }
}

