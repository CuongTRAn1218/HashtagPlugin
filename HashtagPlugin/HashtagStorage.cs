using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HashtagPlugin
{
    public static class HashtagStorage
    {
        private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HashtagsPluginStorage");
        private static readonly string hashtagsFile = Path.Combine(path, "Hashtags.json");
        private static readonly string itemHashtagsFile = Path.Combine(path, "ItemHashtags.json");
        public static List<string> loadHashtags()
        {
            if (File.Exists(hashtagsFile))
            {
                string json = File.ReadAllText(hashtagsFile);

                List<string> hashtags = JsonSerializer.Deserialize<List<string>>(json);
                return hashtags;
            }
            return new List<string>();
        }

        public static void saveHashtags(List<string> hashtags)
        {
            Directory.CreateDirectory(path);
            string json = JsonSerializer.Serialize(hashtags);
            File.WriteAllText(hashtagsFile, json);
        }

        public static bool addHashtag(string hashtag)
        {
            List<string> hashtags = loadHashtags();
            if (!hashtags.Contains(hashtag))
            {
                hashtags.Add(hashtag);
                saveHashtags(hashtags);
                return true;
            }
            return false;
        }

        public static void removeHashtag(string hashtag)
        {
            List<string> hashtags = loadHashtags();
            if (hashtags.Remove(hashtag))
            {
                saveHashtags(hashtags);
            }
        }
        // item hashtags
        public static Dictionary<string, List<string>> loadItemHashtags()
        {
            if (File.Exists(itemHashtagsFile) && new FileInfo(itemHashtagsFile).Length > 0)
            {
                string json = File.ReadAllText(itemHashtagsFile);
                Dictionary<string, List<string>> itemHashtags = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                return itemHashtags;
            }
            return new Dictionary<string, List<string>>();
        }
        public static void saveItemHashtags(Dictionary<string, List<string>> itemHashtags)
        {
            Directory.CreateDirectory(path);
            string json = JsonSerializer.Serialize(itemHashtags);
            File.WriteAllText(itemHashtagsFile, json);
        }
        public static void addItemHashtag(string itemId, string hashtag)
        {
            Dictionary<string, List<string>> itemHashtags = loadItemHashtags();
            if (!itemHashtags.ContainsKey(itemId))
            {
                itemHashtags[itemId] = new List<string>();
            }
            if (!itemHashtags[itemId].Contains(hashtag))
            {
                itemHashtags[itemId].Add(hashtag);
                saveItemHashtags(itemHashtags);
            }
        }
        public static void removeItemHashtag(string itemId, string hashtag)
        {
            Dictionary<string, List<string>> itemHashtags = loadItemHashtags();
            if (itemHashtags.ContainsKey(itemId))
            {
                itemHashtags[itemId].Remove(hashtag);
                if (itemHashtags[itemId].Count == 0)
                {
                    itemHashtags.Remove(itemId);
                }
                saveItemHashtags(itemHashtags);
            }
        }
        public static List<string> getItemHashtags(string itemId)
        {
            Dictionary<string, List<string>> itemHashtags = loadItemHashtags();
            if (itemHashtags.ContainsKey(itemId))
            {
                return itemHashtags[itemId];
            }
            return new List<string>();
        }
    }
}
