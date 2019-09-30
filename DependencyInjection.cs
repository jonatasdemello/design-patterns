// Dependency Injection

// An example of the traditional way:

public class UserLogic
{
    private GoogleOAuthService _authService;
    private GoogleEmailService _emailService;

    public UserLogic()
    {
        _authService = new GoogleOAuthService();
        _emailService = new GoogleEmailService();
    }

    public void Register(string emailAddress, string password)
    {
        var authResult = _authService.RegisterUser(emailAddress,password);
        _emailService.SendMail(emailAddress, authResult.ConfirmationMessage);
    }
}
public class GoogleOAuthService
{
    public GoogleOAuthResult RegisterUser(string emailAddress, string password)
    {
        //Register a new user
    }
}
public class GoogleEmailService
{
    public SendMail(string emailAddress, string message)
    {
        //Send an email using google
    }
}

// generic interface
public interface IEmailService
{
    void SendMail(string emailAddress, string message);
}
public class GoogleEmailService: IEmailService
{
    public SendMail(string emailAddress, string message)
    {
        //Send an email using google
    }
}
public class OutlookEmailService: IEmailService
{
    public void SendMail(string emailAddress, string message)
    {
        //Send an email using outlook
    }
}
// then

// Constructor Injection
...
    GoogleEmailService googleEmailService = new GoogleEmailService();
    UserLogic userLogic = new UserLogic(googleEmailService);
...
public class UserLogic
{
    private GoogleOAuthService _authService;
    private IEmailService _emailService;
	
    public UserLogic(IEmailSevice emailService)
    {
        _authService = new GoogleOAuthService();
        _emailService = emailService;
    }
}

// Setter Injection
...
    OutlookEmailService outlookEmailService = new OutlookEmailService();
    UserLogic userLogic = new UserLogic()
        {
            EmailService = outlookEmailService
        };
...
public class UserLogic
{
    private GoogleOAuthService _authService;
    private IEmailService _emailService;

    public IEmailService EmailService 
    {
        get { return _emailService; } 
        set { _emailService = value; }
    }

    public UserLogic()
    {
        _authService = new GoogleOAuthService();
    }
    ...
}

// Method Injection
...
    OutlookEmailService outlookEmailService = new OutlookEmailService();
    UserLogic userLogic = new UserLogic();
    userLogic.Register(email, password, outlookEmailService);
...
public class UserLogic
{
    private GoogleOAuthService _authService;

    public UserLogic()
    {
        _authService = new GoogleOAuthService();
        _emailService = new OutlookEmailService() // or Google;
    }

    public void Register(string emailAddress, string password, IEmailService emailService)
    {
        var authResult = _authService.RegisterUser(emailAddress,password);
        emailService.SendMail(emailAddress, authResult.ConfirmationMessage);
    }
}
