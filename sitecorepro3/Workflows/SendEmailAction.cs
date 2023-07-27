using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Workflows.Simple;

namespace sitecorepro3.Project.sitecorepro3.Workflows
{
    public class SendEmailAction
    {
        public void Process(WorkflowPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            ProcessorItem processorItem = args.ProcessorItem;

            if (processorItem == null)
            {
                return;
            }

            Item innerItem = processorItem.InnerItem;
            string fullPath = innerItem.Paths.FullPath;

            List<string> recipents =
                GetItems(innerItem, "rules")
                    .Where(x => ShouldSendEmail(args.DataItem, x))
                    .SelectMany(x => x.Fields["EmailList"].Value.Split(';'))
                    .ToList();

            if (recipents.Any())
            {

                string from = GetText(innerItem, "from", args);
                string mailServer = GetText(innerItem, "mail server", args);
                string subject = GetText(innerItem, "subject", args);
                string message = GetText(innerItem, "message", args);

                Error.Assert(from.Length > 0, "The 'From' field is not specified in the mail action item: " + fullPath);
                Error.Assert(subject.Length > 0,
                    "The 'Subject' field is not specified in the mail action item: " + fullPath);
                Error.Assert(mailServer.Length > 0,
                    "The 'Mail server' field is not specified in the mail action item: " + fullPath);

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject + $" Item name: {args.DataItem.Name}";
                mailMessage.Body = message;

                foreach (string recipent in recipents)
                {
                    mailMessage.To.Add(new MailAddress(recipent));
                }

                SmtpClient client = new SmtpClient(mailServer);
                
                    try
                    {
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("anhluong059@gmail.com", "bqtwyujuwqdfyzdv");
                        client.EnableSsl = true;
                        client.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("EmailExAction Threw An Exception", ex, this);
                    }
                    mailMessage.Dispose();
                    client.Dispose();


            }
        }
  
        ///<summary>
        /// Gets the text.
        /// 
        /// </summary>
 
 
        /// <param name="commandItem">The command item.</param><param name="field">The field.</param><param name="args">The arguments.</param>
        /// <returns/>
        private string GetText(Item commandItem, string field, WorkflowPipelineArgs args)
        {
            string text = commandItem[field];
            return text.Length > 0 ? ReplaceVariables(text, args) : string.Empty;
        }
  
        /// <summary>
        /// Replaces the variables.
        /// 
        /// </summary>
 
 
        /// <param name="text">The text.</param><param name="args">The arguments.</param>
        /// <returns/>
        private string ReplaceVariables(string text, WorkflowPipelineArgs args)
        {
            text = text.Replace("$itemPath$", args.DataItem.Paths.FullPath);
            text = text.Replace("$itemLanguage$", args.DataItem.Language.ToString());
            text = text.Replace("$itemVersion$", args.DataItem.Version.ToString());
            return text;
        }

        private bool ShouldSendEmail(Item dataItem, Item ruleItem)
        {
            return IsItemMatch(dataItem, ruleItem) && IsLanguageMatch(dataItem, ruleItem);
        }

        private bool IsItemMatch(Item dataItem, Item ruleItem)
        {
            Item ruleStartItem = GetStartItem(ruleItem);

            return ruleStartItem == null || IsAncestor(dataItem, ruleStartItem);
        }

        private bool IsLanguageMatch(Item dataItem, Item ruleItem)
        {
            List<Language> languages = GetLanguages(ruleItem).ToList();

            return !languages.Any() || languages.Contains(dataItem.Language);
        }

        private IEnumerable<Language> GetLanguages(Item ruleItem)
        {
            MultilistField selectedLanguages = ruleItem.Fields["Languages"];

            if (selectedLanguages != null && selectedLanguages.TargetIDs.Any())
            {
                return selectedLanguages.GetItems().Select(x => Language.Parse(x.Name));
            }

            return Enumerable.Empty<Language>();
        }
  
        /// <summary>
        /// Gets the items
        /// </summary>
 
 
        /// <param name="commandItem"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private IEnumerable<Item> GetItems(Item commandItem, string field)
        {
            MultilistField rules = commandItem.Fields[field];

            if (rules != null && rules.TargetIDs.Any())
            {
                return rules.GetItems();
            }

            return Enumerable.Empty<Item>();
        }

        private Item GetStartItem(Item ruleItem)
        {
            ReferenceField startPathField = ruleItem.Fields["StartPath"];

            if (startPathField != null && startPathField.TargetItem != null)
            {
                return startPathField.TargetItem;
            }

            return null;

        }

        private bool IsAncestor(Item currentItem, Item ancestor)
        {
            if (currentItem.ID == ancestor.ID)
            {
                return true;
            }

            if (currentItem.Parent != null)
            {
                return IsAncestor(currentItem.Parent, ancestor);
            }

            return false;
        }
    }
}