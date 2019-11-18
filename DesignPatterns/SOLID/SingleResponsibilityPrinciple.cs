using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace DesignPatterns.SOLID
{
    //
    // Single Responsibility Principle
    //
    // any class must have one, and only one, reason to change.
    // If a class has more than one reason to change, it should be refactored.

    // this class does validation and send email
    public class ValidateSendInvitationService
    {
        public void SendInvite(string email, string firstName, string lastName)
        {
            if (String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName))
            {
                throw new Exception("Name is not valid!");
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new Exception("Email is not valid!!");
            }
            SmtpClient client = new SmtpClient();
            client.Send(new MailMessage("mysite@nowhere.com", email) { Subject = "Please join me at my party!" });
        }
    }
    // so we break the validation in 2
    public class UserNameService
    {
        public void Validate(string firstName, string lastName)
        {
            if (String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName))
            {
                throw new Exception("The name is invalid!");
            }
        }
    }
    public class EmailService
    {
        public void Validate(string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new Exception("Email is not valid!!");
            }
        }
    }

    public class InvitationService
    {
        UserNameService _userNameService;
        EmailService _emailService;

        public InvitationService(UserNameService userNameService, EmailService emailService)
        {
            _userNameService = userNameService;
            _emailService = emailService;
        }
        public void SendInvite(string email, string firstName, string lastName)
        {
            _userNameService.Validate(firstName, lastName);
            _emailService.Validate(email);
            SmtpClient client = new SmtpClient();
            client.Send(new MailMessage("sitename@invites2you.com", email) { Subject = "Please join me at my party!" });
        }
    }

    // Another exemple
    public class Journal
    {
        // just a counter for total # of entries
        private static int count = 0;
        private readonly List<string> entries = new List<string>();

        public void AddEntry(string text)
        {
            entries.Add($"{++count}: {text}");
        }
        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }
        // The journal’s responsibility is to keep journal entries, not to write them to disk.
        // any change in the approach  to persistence 
        // (say, you decide to write to the cloud instead of disk) would 
        // require lots of tiny changes in each of the affected classes.
        public void Save(string filename, bool overwrite = false)
        {
            File.WriteAllText(filename, ToString());
        }
    }
    public class MainJournal
    {
        public void Main()
        {
            var j = new Journal();
            j.AddEntry("I cried today.");
            j.AddEntry("I ate a bug.");
        }
    }

    public class PersistenceManager
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, journal.ToString());
        }
    }
    // This is precisely what we mean by single responsibility: Each class 
    // has only one responsibility, and therefore has only one reason to change. 
    // Journal would need to change only if there is something more that needs 
    // to be done with respect to in-memory storage of entries; for example, 
    // you might want each entry prefixed by a timestamp, so you would 
    // change the Add() method to do exactly that. On the other hand, if you 
    // wanted to change the persistence mechanic, this would be changed in 
    // PersistenceManager.
}

