using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using HashtagPlugin.Storage;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
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
        public static void removeHashtagFromAllItems(string hashtag)
        {
            var itemHashtags = loadAllItemHashtags();
            Outlook.Application outlookApp = new Outlook.Application();

            foreach (var kvp in itemHashtags.ToList())
            {
                string entryId = kvp.Key;
                ItemInfo info = kvp.Value;

                if (info.Hashtags.Contains(hashtag))
                {
                    try
                    {
                        object itemObj = outlookApp.Session.GetItemFromID(entryId);

                        if (itemObj is Outlook.MailItem mail)
                        {
                            mail.Body = removeTagFromBody(mail.Body, hashtag);
                            mail.Save();
                        }
                        else if (itemObj is Outlook.AppointmentItem appointment)
                        {
                            appointment.Body = removeTagFromBody(appointment.Body, hashtag);
                            appointment.Save();
                        }
                        else if (itemObj is Outlook.ContactItem contact)
                        {
                            contact.Body = removeTagFromBody(contact.Body, hashtag);
                            contact.Save();
                        }else if (itemObj is Outlook.TaskItem task)
                        {
                            task.Body = removeTagFromBody(task.Body, hashtag);
                            task.Save();
                        }
                        else if (itemObj is Outlook.NoteItem note)
                        {
                            note.Body = removeTagFromBody(note.Body, hashtag);
                            note.Save();
                        }
                        else if (itemObj is Outlook.PostItem post)
                        {
                            post.Body = removeTagFromBody(post.Body, hashtag);
                            post.Save();
                        }
                        removeItemHashtag(entryId, hashtag);
                    }
                    catch (System.Exception ex)
                    {
                        // Handle or log exception if needed
                        System.Diagnostics.Debug.WriteLine($"Failed to update item {entryId}: {ex.Message}");
                    }
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
                case Outlook.TaskItem taskItem:
                    return taskItem.Body;
                case Outlook.NoteItem noteItem:
                    return noteItem.Body;
                case Outlook.PostItem postItem:
                    return postItem.Body;
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
                case Outlook.TaskItem taskItem:
                    taskItem.Body = newBody;
                    break;
                case Outlook.NoteItem noteItem:
                    noteItem.Body = newBody;
                    break;
                case Outlook.PostItem postItem:
                    postItem.Body = newBody;
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
                case Outlook.TaskItem taskItem:
                    return taskItem.EntryID;
                case Outlook.NoteItem noteItem:
                    return noteItem.EntryID;
                case Outlook.PostItem postItem:
                    return postItem.EntryID;

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
                case Outlook.TaskItem taskItem:
                    taskItem.Save();
                    break;
                case Outlook.NoteItem noteItem:
                    noteItem.Save();
                    break;
                case Outlook.PostItem postItem:
                    postItem.Save();
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
                case Outlook.TaskItem task:
                    task.Body = appendHashtag(task.Body, hashtag);
                    task.Save();
                    type = "Task";
                    itemId = task.EntryID;
                    break;
                case Outlook.NoteItem note:
                    note.Body = appendHashtag(note.Body, hashtag);
                    note.Save();
                    type = "Note";
                    itemId = note.EntryID;
                    break;
                case Outlook.PostItem post:
                    post.Body = appendHashtag(post.Body, hashtag);
                    post.Save();
                    type = "Post";
                    itemId = post.EntryID;
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
                case Outlook.TaskItem task:
                    task.Body = removeTagFromBody(task.Body, hashtag);
                    task.Save();
                    itemId = task.EntryID;
                    break;
                case Outlook.NoteItem note:
                    note.Body = removeTagFromBody(note.Body, hashtag);
                    note.Save();
                    itemId = note.EntryID;
                    break;
                case Outlook.PostItem post:
                    post.Body = removeTagFromBody(post.Body, hashtag);
                    post.Save();
                    itemId = post.EntryID;
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
        public static List<string> ExtractHashtags(string text)
        {
            var words = text.Split(new[] { ' ', '\n', '\r', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var hashtags = new HashSet<string>();

            foreach (var word in words)
            {
                if (word.StartsWith("#") && word.Length > 1 && word.Skip(1).All(char.IsLetterOrDigit))
                {
                    hashtags.Add(word);
                }
            }

            return hashtags.ToList();
        }

        public static async Task<List<string>> GenerateHashtagsFromOllama(string content)
        {
            string existingHashtags = string.Join(",",loadHashtags());
            content = CondenseContent(content, 1000);
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
            return ExtractHashtags(responseText);
        }

        public static async Task<List<string>> GenerateHashtagsFromAPIVerse(string content)
        {
            int count = 5;
            int maxContentLength = 500;
            var client = new HttpClient();
        
            try
            {
                if (content.Length > maxContentLength)
                {
                    content = CondenseContent(content, maxContentLength);
                    MessageBox.Show("Content was too long, truncated to: " + content);
                }
                var requestData = new
                {
                    text = content,
                    count = count
                };
                var json = JsonSerializer.Serialize(requestData);
                var contentw = new StringContent(json, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("x-api-key", "c4a115e8-aadd-41e3-9f95-8a5a595c801c");

                var response = await client.PostAsync(
                    "https://api.apiverve.com/v1/hashtaggenerator", 
                    contentw);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return ExtractAPIHashtags(body);
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Details: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
            return null;
        }
        private static List<string> ExtractAPIHashtags(string json)
        {
            var jsonDoc = JsonDocument.Parse(json);

            var hashtags = jsonDoc.RootElement
                                  .GetProperty("data")
                                  .GetProperty("hashtags")
                                  .EnumerateArray();
            List<string> hashtagList = new List<string>();
            foreach (var hashtag in hashtags)
            {
                hashtagList.Add(hashtag.GetString());
            }

            return hashtagList;
        }

        private static string CondenseContent(string content, int maxLength)
        {
            content = RemoveUrls(content);
            content = content.Replace("\n", " ").Replace("\r", "");
            content = Regex.Replace(content, @"\s+", " ").Trim();
            
            if (content.Length > maxLength)
            {
                content = content.Substring(0, maxLength);
                int lastSpaceIndex = content.LastIndexOf(' ');
                if (lastSpaceIndex > 0)
                {
                    content = content.Substring(0, lastSpaceIndex);
                }

            }

            return content;
        }

        private static string RemoveUrls(string content)
        {
            var regex = new Regex(@"http[^\s]+", RegexOptions.IgnoreCase);
            return regex.Replace(content, string.Empty);
        }

    }
}
