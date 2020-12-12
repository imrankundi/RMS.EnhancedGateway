using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace RMS
{
    public class LanguageResource
    {
        public IEnumerable<FormResource> Forms { get; set; }
    }

    public class FormResource
    {
        public string Name { get; set; }
        public IEnumerable<LabelResource> Labels { get; set; }
        public IEnumerable<ListResource> Lists { get; set; }

        public Dictionary<string, LabelResource> GetLabels()
        {
            Dictionary<string, LabelResource> dictionary = new Dictionary<string, LabelResource>();
            foreach (var label in Labels)
            {
                if (!dictionary.ContainsKey(label.Name))
                {
                    dictionary.Add(label.Name, label);
                }

            }
            return dictionary;
        }

        public Dictionary<string, ListResource> GetLists()
        {
            Dictionary<string, ListResource> dictionary = new Dictionary<string, ListResource>();
            foreach (var list in Lists)
            {
                if (!dictionary.ContainsKey(list.Name))
                {
                    dictionary.Add(list.Name, list);
                }

            }
            return dictionary;
        }

    }
    public class LabelResource
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
    public class ListResource
    {
        public string Name { get; set; }
        public IEnumerable<ItemResource> Items { get; set; }

    }

    public class ItemResource
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }


    public sealed class LanguageManager
    {
        public Dictionary<string, FormResource> LanguageDictionary;
        private static readonly Lazy<LanguageManager> lazy = new Lazy<LanguageManager>(() => new LanguageManager());
        public static LanguageManager Instance => lazy.Value;
        private LanguageManager()
        {
            LoadLanguage();
        }

        private IEnumerable<FormResource> LoadFromFolder(string languageCode)
        {
            string path = string.Format(@"i18n\{0}\", languageCode);
            if (Directory.Exists(path))
            {
                var files = Directory.EnumerateFiles(path, "*.json");

                foreach (var file in files)
                {
                    FormResource formResource = null;
                    try
                    {
                        var jsonString = File.ReadAllText(file);
                        formResource = JsonConvert.DeserializeObject<FormResource>(jsonString);

                    }
                    catch (Exception ex)
                    {
                    }

                    yield return formResource;
                }
            }
        }
        private void LoadLanguage()
        {
            string languageCode = ConfigurationManager.AppSettings["Language"];
            //LanguageResource languageResource = DeserializeLanguageFile(languageCode);
            IEnumerable<FormResource> formResources = LoadFromFolder(languageCode);
            LanguageDictionary = CreateDictionary(formResources);
        }

        private LanguageResource DeserializeLanguageFile(string languageCode)
        {
            string path = string.Format(@"i18n\{0}.json", languageCode);
            if (!File.Exists(path))
            {
                throw new Exception("Language Resource Not Found");
            }
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<LanguageResource>(jsonString);
        }

        private Dictionary<string, FormResource> CreateDictionary(IEnumerable<FormResource> formResources)
        {

            var resourceDictionary = new Dictionary<string, FormResource>();

            foreach (FormResource formResource in formResources)
            {
                if (!resourceDictionary.ContainsKey(formResource.Name))
                {
                    resourceDictionary.Add(formResource.Name, formResource);
                }
            }

            return resourceDictionary;
        }

        public FormResource GetFormResource(string name)
        {
            if (!LanguageDictionary.ContainsKey(name))
            {
                return null;
            }

            return LanguageDictionary[name];
        }


    }
}
