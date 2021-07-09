using System;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace json_experiments
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Environment.CurrentDirectory + ".\\lemma-response.json"; 
            string data = File.ReadAllText(path);

            using (JsonDocument json = JsonDocument.Parse(data))
            {
                JsonElement root = json.RootElement;
                // works;
                /*
                var tokens = root.EnumerateObject()
                    .Where(block => block.Value.ValueKind == JsonValueKind.Array && block.Name == "data")
                    .SelectMany(block => block.Value.EnumerateArray().Select(subBlock => subBlock.GetProperty("token").GetString()));
                
                */
                IEnumerable<JsonElement> lemmaBlocks = root.EnumerateObject()
                    .Where(block => block.Value.ValueKind == JsonValueKind.Array && block.Name == "data")
                    .SelectMany(block => block.Value.EnumerateArray().Select(subBlock => subBlock.GetProperty("lemmatizations")));

                List<JsonElement> lemmaList = new();

            lemmaList.AddRange(lemmaBlocks);
            List<string> lemmaWords = new();
            for (var i = 0; i < lemmaList.Count; i++)
            {
                try {
                    Console.WriteLine(lemmaList[i].ValueKind);
                    var lemmas = lemmaList[i].EnumerateArray();

// @TODO make sure if lemmas is blank that a "" word gets added
                    while (lemmas.MoveNext())
                    {

                        
                   var lwordsObj = lemmas.Current.EnumerateObject();
                   while (lwordsObj.MoveNext())
                   {
                       if (lwordsObj.Current.Name == "word")
                       {
                           Console.WriteLine("Inside Word");
                           lemmaWords.Add(lwordsObj.Current.Value.GetProperty("lemma").GetString());
                       } else {
                        Console.WriteLine(lwordsObj.Current.Name);
                       }


                   }
                       //  var lWord = lemmas.Current.TryGetProperty("lemma", out JsonElement lemmaElement);
                    }
                } catch {
                }
            }




        }
    }
}
}