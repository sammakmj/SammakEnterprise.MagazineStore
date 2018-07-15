using Newtonsoft.Json;
using System;

namespace SammakEnterprise.MagazineStore.Utilities
{
    public class JsonDisplay
    {
        public static void DisplayJsonData(Action<string> displayMethod, string message, object response)
        {
            var jsonData = JsonConvert.SerializeObject(response, Formatting.Indented);
            displayMethod($"{message}:{Environment.NewLine}{jsonData}");
        }

        public static void DisplayJsonResponse(Action<string> displayMethod, string methodName, object response)
        {
            DisplayJsonData(displayMethod, $"{methodName} Response", response);
        }

        public static void ConsoleDisplayJsonData(string methodName, object response, string methodNameAppend = null)
        {
            DisplayJsonData(Console.WriteLine, $"{methodName} {methodNameAppend}", response);
            Console.WriteLine("============================================================");
            Console.WriteLine();
        }

        public static void ConsoleDisplayJsonResponse(string methodName, object response)
        {
            ConsoleDisplayJsonData(methodName, response, "Response");
        }

    }
}
