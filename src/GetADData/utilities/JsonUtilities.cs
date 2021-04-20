using Newtonsoft.Json;
using System;
using System.IO;

namespace GetADData.utilities
{
    public class JsonUtilities
    {
        public static void JsonFileExporter(string[] args, object data)
        {
            var filename = args[1];

            if (filename.Split('.').Length < 2)
                filename += ".json";

            var fullPath = Path.GetFullPath(filename);

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(data, Formatting.Indented));

        }

        public static void JsonConsoleViewer(object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}