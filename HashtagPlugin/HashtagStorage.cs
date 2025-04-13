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
        private static readonly string file = Path.Combine(path, "Hashtags.json");
        public static List<string> loagHashtags()
        {
            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);

                List<string> hashtags = JsonSerializer.Deserialize<List<string>>(json);
                return hashtags;
            }
            return new List<string>();
        }

        public static void saveHashtags(List<string> hashtags)
        {
            Directory.CreateDirectory(path);
            string json = JsonSerializer.Serialize(hashtags);
            File.WriteAllText(file, json);
        }

        public static bool addHashtag(string hashtag)
        {
            List<string> hashtags = loagHashtags();
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
            List<string> hashtags = loagHashtags();
            if (hashtags.Remove(hashtag))
            {
                saveHashtags(hashtags);
            }
        }
    }
}
