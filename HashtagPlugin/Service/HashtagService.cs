using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using HashtagPlugin.Storage;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace HashtagPlugin.Service
{
    public static class HashtagService
    {
        //General
        public static List<string> loadHashtags()
        {
            return HashtagStorage.loadHashtags();
        }
        public static Dictionary<string, List<string>> loadAllItemHashtags()
        {
            return HashtagStorage.loadAllItemHashtags();
        }
        public static bool addHashtag(string hashtag)
        {
            List<string> hashtags = loadHashtags();
            if(!hashtags.Contains(hashtag))
            {
                hashtags.Add(hashtag);
                HashtagStorage.saveHashtags(hashtags);
                return true;
            }
            return false;
        }
        public static void removeHashtag(string hashtag)
        {
            List<string> hashtags = loadHashtags();
            if (hashtags.Contains(hashtag))
            {
                if (hashtags.Remove(hashtag)){
                    HashtagStorage.saveHashtags(hashtags);
                }
            }
        }
        public static void addItemHashtag(string itemId, string hashtag)
        {
            Dictionary<string, List<string>> itemHashtags = HashtagStorage.loadAllItemHashtags();
            if (!itemHashtags.ContainsKey(itemId))
            {
                itemHashtags[itemId] = new List<string>();
            }
            if (!itemHashtags[itemId].Contains(hashtag))
            {
                itemHashtags[itemId].Add(hashtag);
                HashtagStorage.saveItemHashtags(itemHashtags);
            }
        }
        public static void removeItemHashtag(string itemId, string hashtag)
        {
            Dictionary<string, List<string>> itemHashtags = HashtagStorage.loadAllItemHashtags();
            if (itemHashtags.ContainsKey(itemId))
            {
                itemHashtags[itemId].Remove(hashtag);
                if (itemHashtags[itemId].Count == 0)
                {
                    itemHashtags.Remove(itemId);
                }
                HashtagStorage.saveItemHashtags(itemHashtags);
            }
        }
        public static List<string> loadItemHashtags(string itemId)
        {
            var itemHashtags = HashtagStorage.loadAllItemHashtags();
            if (itemHashtags.ContainsKey(itemId))
            {
                List<string> hashtags = itemHashtags[itemId];
                return hashtags;
            }
            return new List<string>(); ;
        }   
        public static bool checkHashtag(string hashtag)
        {
            List<string> hashtags = loadHashtags();
            return hashtags.Contains(hashtag);
        }
        public static string getItemBody(object outlookItem)
        {
            switch (outlookItem)
            {
                case Outlook.MailItem mailItem:
                    return mailItem.Body;
                case Outlook.AppointmentItem appointmentItem:
                    return appointmentItem.Body;
                case Outlook.ContactItem contactItem:
                    return contactItem.Body;
                default:
                    throw new ArgumentException("Unsupported Outlook item type.");
            }
        }

        public static void setItemBody(object outlookItem, string newBody)
        {
            switch (outlookItem)
            {
                case Outlook.MailItem mailItem:
                    mailItem.Body = newBody;
                    break;
                case Outlook.AppointmentItem appointmentItem:
                    appointmentItem.Body = newBody;
                    break;
                case Outlook.ContactItem contactItem:
                    contactItem.Body = newBody;
                    break;
                default:
                    throw new ArgumentException("Unsupported Outlook item type.");
            }
        }
        public static string getItemEntryID(object outlookItem)
        {
            switch (outlookItem)
            {
                case Outlook.MailItem mailItem:
                    return mailItem.EntryID;
                case Outlook.AppointmentItem appointmentItem:
                    return appointmentItem.EntryID;
                case Outlook.ContactItem contactItem:
                    return contactItem.EntryID;
                default:
                    throw new ArgumentException("Unsupported Outlook item type.");
            }
        }
        public static void saveItem(object outlookItem)
        {
            switch (outlookItem)
            {
                case Outlook.MailItem mailItem:
                    mailItem.Save();
                    break;
                case Outlook.AppointmentItem appointmentItem:
                    appointmentItem.Save();
                    break;
                case Outlook.ContactItem contactItem:
                    contactItem.Save();
                    break;
                default:
                    throw new ArgumentException("Unsupported Outlook item type.");
            }
        }
        //Add Hashtag
        public static void addItemHashtag(object item, string hashtag)
        {
            string itemId = null;
            if (item is Outlook.MailItem mail)
            {
                mail.Body = appendHashtag(mail.Body, hashtag);
                mail.Save();
                itemId = mail.EntryID;
            }
            else if (item is Outlook.AppointmentItem appointment)
            {
                appointment.Body = appendHashtag(appointment.Body, hashtag);
                appointment.Save();
                itemId = appointment.EntryID;
            }
            else if (item is Outlook.ContactItem contact)
            {
                contact.Body = appendHashtag(contact.Body, hashtag);
                contact.Save();
                itemId = contact.EntryID;
            }
            addHashtag(hashtag);
            addItemHashtag(itemId, hashtag);
        }
        private static string appendHashtag(string body, string newTag)
        {
            const string tagSeparator = "-----";
            var lines = body.TrimEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            int tagLineIndex = lines.FindLastIndex(line => line.StartsWith(tagSeparator));
            List<string> tags;

            if (tagLineIndex >= 0 && tagLineIndex < lines.Count - 1)
            {
                string tagsLine = lines[tagLineIndex + 1];
                tags = tagsLine.Split(' ')
                              .Where(t => !string.IsNullOrWhiteSpace(t))
                              .ToList();

                if (!tags.Contains(newTag))
                {
                    tags.Add(newTag);
                    lines[tagLineIndex + 1] = string.Join(" ", tags);
                }
            }

            else if (tagLineIndex < 0)
            {
                lines.Add(tagSeparator);
                lines.Add(newTag);
            }
            else
            {
                lines.Add(newTag);
            }

            return string.Join("\r\n", lines);
        }

        //Remove Hashtag
        public static void removeItemHashtag(object item, string hashtag)
        {
            string itemId = null;
            if (item is Outlook.MailItem mail)
            {
                mail.Body = removeTagFromBody(mail.Body, hashtag);
                mail.Save();
                itemId = mail.EntryID;
            }
            else if (item is Outlook.AppointmentItem appointment)
            {
                appointment.Body = removeTagFromBody(appointment.Body, hashtag);
                appointment.Save();
                itemId = appointment.EntryID;
            }
            else if (item is Outlook.ContactItem contact)
            {
                contact.Body = removeTagFromBody(contact.Body, hashtag);
                contact.Save();
                itemId = contact.EntryID;
            }
            removeHashtag(hashtag);
            removeItemHashtag(itemId, hashtag);
        }
        private static string removeTagFromBody(string body, string tagToRemove)
        {
            const string delimiter = "-----";
            int delimiterIndex = body.IndexOf(delimiter);

            if (delimiterIndex != -1)
            {
                string before = body.Substring(0, delimiterIndex + delimiter.Length);
                string after = body.Substring(delimiterIndex + delimiter.Length).Trim();

                var tags = after.Split(' ').Where(t => !t.Equals(tagToRemove, StringComparison.OrdinalIgnoreCase)).ToList();
                string newAfter = string.Join(" ", tags);

                return before + Environment.NewLine + newAfter;
            }
            return body;
        }

    }
}
