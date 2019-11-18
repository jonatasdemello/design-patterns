using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns
{
    // Dependency Injection

    // Normal way, separated classes
    public class GoogleOAuthService_v1
    {
        public void RegisterUser(string emailAddress, string password)
        {
            Console.WriteLine("Register a new Google user");
        }
    }
    public class GoogleEmailService_v1
    {
        public void SendMail(string emailAddress, string message)
        {
            Console.WriteLine("Send an email using google");
        }
    }
    // UserLogic Creates its dependencies with "new"
    public class UserLogic_v1
    {
        private GoogleOAuthService_v1 _authService;
        private GoogleEmailService_v1 _emailService;

        public UserLogic_v1()
        {
            _authService = new GoogleOAuthService_v1();
            _emailService = new GoogleEmailService_v1();
        }

        public void Register(string emailAddress, string password)
        {
            _authService.RegisterUser(emailAddress, password);
            _emailService.SendMail(emailAddress, "ConfirmationMessage");
        }
    }

    // ----------------------------------------------
    // Dependency Injection
    // start with a generic interface
    public interface IEmailService
    {
        void SendMail(string emailAddress, string message);
    }
    // each concrete class implement IEmailService_v2
    public class GoogleEmailService_v2 : IEmailService
    {
        public void SendMail(string emailAddress, string message)
        {
            Console.WriteLine("Send an email using google");
        }
    }
    public class OutlookEmailService_v2 : IEmailService
    {
        public void SendMail(string emailAddress, string message)
        {
            Console.WriteLine("Send an email using google");
        }
    }

    // then:
    public class ConstructorInjection
    {
        public void ConstructorInjectionTest()
        {
            // the object is created somewhere else
            GoogleEmailService_v2 googleEmailService = new GoogleEmailService_v2();

            // then the dependency is injected through the constructor:
            UserLogic_Constructor userLogic = new UserLogic_Constructor(googleEmailService);
        }
        public class UserLogic_Constructor
        {
            private IEmailService _emailService;
            public UserLogic_Constructor(IEmailService emailService)
            {
                _emailService = emailService;
            }
        }
    }
    
    public class SetterInjection
    {
        public void SetterInjectionTest()
        {
            // the object is created somewhere else
            OutlookEmailService_v2 outlookEmailService = new OutlookEmailService_v2();

            // then the dependency is injected through the setter property:
            UserLogic_Setter userLogic = new UserLogic_Setter()
            {
                EmailService = outlookEmailService
            };
        }
        public class UserLogic_Setter
        {
            private IEmailService _emailService;
            public IEmailService EmailService
            {
                get { return _emailService; }
                set { _emailService = value; }
            }
        }
    }

    public class MethodInjection
    {
        public void MethodInjectionTest()
        {
            // the object is created somewhere else
            OutlookEmailService_v2 outlookEmailService = new OutlookEmailService_v2();

            UserLogic_Method userLogic = new UserLogic_Method();

            // then the dependency is injected using the class method:
            userLogic.Register("email", "password", outlookEmailService);
        }

        public class UserLogic_Method
        {
            private GoogleOAuthService_v1 _authService;
            private OutlookEmailService_v2 _emailService;

            public UserLogic_Method()
            {
                _authService = new GoogleOAuthService_v1();
                _emailService = new OutlookEmailService_v2();
            }

            public void Register(string emailAddress, string password, IEmailService emailService)
            {
                _authService.RegisterUser(emailAddress, password);
                emailService.SendMail(emailAddress, "ConfirmationMessage");
            }
        }
    }
}

