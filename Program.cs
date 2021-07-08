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
                List<JsonElement> lemmaBlocks = root.EnumerateObject()
                    .Where(block => block.Value.ValueKind == JsonValueKind.Array && block.Name == "data")
                    .SelectMany(block => block.Value
                    .EnumerateArray()
                    .Select(t => t.GetProperty("lemmatizations")))
                    .ToList();

                List<string> lemmas = new();
                foreach(var l in lemmaBlocks)
                {
                    if (l.TryGetProperty("lemma", out JsonElement lemmaProp)){
                        lemmas.Add(lemmaProp.GetString());
                    } else {
                        lemmas.Add("");
                    }
                }

                foreach (var t in lemmas)
                {
                    Console.WriteLine(t);
                    Console.ReadLine();
                }
/*
                Console.WriteLine("Press Enter to display Lemmas:");
                Console.ReadLine();
                

                foreach (var l in lemmas)
                {
                    Console.WriteLine(l);
                }
                */
            }
        }
    }
}
