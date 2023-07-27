using System;
using System.Net;
using System.Net.Mail;
using Sitecore.Data.Items;

namespace sitecorepro3.Project.sitecorepro3.Tasks.Commands
{
    public class SendReminderEmail
    {
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem commandItem,
            Sitecore.Tasks.ScheduleItem scheduleItem)
        {
            try
            {
                string from = "anhluong059@gmail.com";
                string subject = "Reminder mail";
                string message = "This is a test Reminder mail";
                string to = "anhluong059123@gmail.com";

                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(from),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(new MailAddress(to));

                var client = new SmtpClient();
                client.Port = 587;
                client.Credentials = new NetworkCredential("anhluong059@gmail.com", "bqtwyujuwqdfyzdv");
                client.EnableSsl = true;
                client.Send(mailMessage);

                mailMessage.Dispose();
                client.Dispose();


            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Scheduler Exceptions: " + e.Message, this);
            }
        }
    }
}