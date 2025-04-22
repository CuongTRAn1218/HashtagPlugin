using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using HashtagPlugin.Storage;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Http;
using System.Text.Json;
namespace HashtagPlugin.Service
{
    public static class HashtagService
    {
        //General
        public static List<string> loadHashtags()
        {
            return HashtagStorage.loadHashtags();
        }
        public static Dictionary<string, ItemInfo> loadAllItemHashtags()
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
        public static void addItemHashtag(string itemId, string type,string hashtag)
        {
            Dictionary<string, ItemInfo> itemHashtags = HashtagStorage.loadAllItemHashtags();
            if (!itemHashtags.ContainsKey(itemId))
            {
                itemHashtags[itemId] = new ItemInfo { Type = type, Hashtags = new List<string>() };
            }
            if (!itemHashtags[itemId].Hashtags.Contains(hashtag))
            {
                itemHashtags[itemId].Hashtags.Add(hashtag);
                HashtagStorage.saveItemHashtags(itemHashtags);
            }
        }
        public static void removeItemHashtag(string itemId, string hashtag)
        {
            Dictionary<string, ItemInfo> itemHashtags = HashtagStorage.loadAllItemHashtags();
            if (itemHashtags.TryGetValue(itemId, out var tagInfo))
            {
                tagInfo.Hashtags.Remove(hashtag);
                if (tagInfo.Hashtags.Count == 0)
                {
                    itemHashtags.Remove(itemId);
                }
                HashtagStorage.saveItemHashtags(itemHashtags);
            }
        }
        public static List<string> loadItemHashtags(string itemId)
        {
            var itemHashtags = HashtagStorage.loadAllItemHashtags();
            return itemHashtags.TryGetValue(itemId, out var info) ? info.Hashtags : new List<string>();
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
            string type = null;
            string itemId = null;

            switch (item)
            {
                case Outlook.MailItem mail:
                    mail.Body = appendHashtag(mail.Body, hashtag);
                    mail.Save();
                    type = "Mail";
                    itemId = mail.EntryID;
                    break;
                case Outlook.AppointmentItem appt:
                    appt.Body = appendHashtag(appt.Body, hashtag);
                    appt.Save();
                    type = "Appointment";
                    itemId = appt.EntryID;
                    break;
                case Outlook.ContactItem contact:
                    contact.Body = appendHashtag(contact.Body, hashtag);
                    contact.Save();
                    type = "Contact";
                    itemId = contact.EntryID;
                    break;
                default:
                    throw new ArgumentException("Unsupported item type.");
            }

            addHashtag(hashtag);
            addItemHashtag(itemId, type, hashtag);
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
            switch (item)
            {
                case Outlook.MailItem mail:
                    mail.Body = removeTagFromBody(mail.Body, hashtag);
                    mail.Save();
                    itemId = mail.EntryID;
                    break;
                case Outlook.AppointmentItem appt:
                    appt.Body = removeTagFromBody(appt.Body, hashtag);
                    appt.Save();
                    itemId = appt.EntryID;
                    break;
                case Outlook.ContactItem contact:
                    contact.Body = removeTagFromBody(contact.Body, hashtag);
                    contact.Save();
                    itemId = contact.EntryID;
                    break;
                default:
                    throw new ArgumentException("Unsupported item type.");
            }

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
        //Search Hashtags
        public static List<string> SearchItemsByTags(IEnumerable<string> tags)
        {
            Dictionary<string, ItemInfo> itemHashtags = HashtagStorage.loadAllItemHashtags();
            List<string> result = new List<string>();

            foreach (var kvp in itemHashtags)
            {
                var itemHashtagsList = kvp.Value.Hashtags;

                if (tags.All(tag => itemHashtagsList.Contains(tag)))
                {
                    result.Add(kvp.Key);
                }
            }
            return result;

        }

        //Generate Hashtag
        public static List<string> ExtractHashtags(string text, List<string> alreadyAdded)
        {
            var words = text.Split(new[] { ' ', '\n', '\r', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var hashtags = new HashSet<string>();

            foreach (var word in words)
            {
                if (word.StartsWith("#") && word.Length > 1 && word.Skip(1).All(char.IsLetterOrDigit)&& !alreadyAdded.Contains(word))
                {
                    hashtags.Add(word);
                }
            }

            return hashtags.ToList();
        }

        public static async Task<List<string>> GenerateHashtagsFromOllamac(string content, List<string> alreadyAdded)
        {
            string existingHashtags = string.Join(",",loadHashtags());

            var httpClient = new HttpClient();
            var requestData = new
            {
                model = "gemma3",
                prompt = $"\"Generate under 10 hashtags also check and take any relevant hashtag in this{existingHashtags} for:{content} \nonly use lowercase letters. only output the hashtags comma-seprated.\"",
                stream = false
            };

            var json = JsonSerializer.Serialize(requestData);
            var contentw = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:11434/api/generate", contentw);
            var responseText = await response.Content.ReadAsStringAsync();
            return ExtractHashtags(responseText,alreadyAdded);
        }
    }
}
