using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HashtagPlugin.Storage
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
            string json = JsonSerializer.Serialize(hashtags, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(hashtagsFile, json);
        }
        // item hashtags
        public static Dictionary<string, ItemInfo> loadAllItemHashtags()
        {
            if (File.Exists(itemHashtagsFile) && new FileInfo(itemHashtagsFile).Length > 0)
            {
                string json = File.ReadAllText(itemHashtagsFile);
                Dictionary<string, ItemInfo> itemHashtags = JsonSerializer.Deserialize<Dictionary<string,ItemInfo>>(json);
                return itemHashtags;
            }
            return new Dictionary<string, ItemInfo>();
        }
        public static void saveItemHashtags(Dictionary<string, ItemInfo> itemHashtags)
        {
            Directory.CreateDirectory(path);
            string json = JsonSerializer.Serialize(itemHashtags, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(itemHashtagsFile, json);
        }


    }
}
