using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Configuration;
using System.Globalization;


using CarFixed.Core;

namespace CarFixed.Tests.Core
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, string> textSubs = new Dictionary<string, string>();
            textSubs.Add("%SUBJECT%", String.Format("You Quotes {0}", DateTime.Now.ToString("dddd dd MMMM ", CultureInfo.CreateSpecificCulture("en-GB"))));
            textSubs.Add("%NAME%", "Dave");
            textSubs.Add("%MANUFACTURER%", "Honda");
            textSubs.Add("%MODEL%", "Civic");
            textSubs.Add("%VRM%", "RF55FFE");
            textSubs.Add("%SENDERNAME%", "Sean Blackburn");
            textSubs.Add("%SENDERROLE%", "Director");

            EmailSender sender = new EmailSender();

            string emailBody = System.IO.File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["EmailDirectory"], "StandardEmailTemplate_Styled.html"));


            sender.SendMail("a.ananjev@yahoo.com", "d.gill@mycarfixed.co.uk", "Test Subject", emailBody, true, 
                fromName: "Sean Blackburn",
                bcc: "d.gill@mycarfixed.co.uk",
                textSubs: textSubs);

        }
    }
}
