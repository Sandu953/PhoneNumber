using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;

public class MakeAbstractRequest
{

    public class PhoneValidationResponse
    {
        public string Phone { get; set; }
        public bool Valid { get; set; }
        public Format Format { get; set; }
        public Country Country { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Carrier { get; set; }
    }

    public class Format
    {
        public string International { get; set; }
        public string Local { get; set; }
    }

    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
    }

    public static void Main()
    {
        // Console.WriteLine("Enter a phone number to validate:");
        //string phoneNumber = Console.ReadLine();

        // MakeAbstractRequestAsync(phoneNumber).Wait();
        var path = @"C:\Users\probook\source\repos\TestPhoneApp\dataTest.csv"; // Habeeb, "Dubai Media City, Dubai"
        using (TextFieldParser csvParser = new TextFieldParser(path))
        {
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = true;

            // Skip the row with the column names
            csvParser.ReadLine();

            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] fields = csvParser.ReadFields();
                Console.WriteLine(String.Join(',',fields));
            }
        }
    }

    private static async Task MakeAbstractRequestAsync(string phoneNumber)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string apiKey = "c6075e6c2c9a427fa0a482b005c81114";
                string url = $"https://phonevalidation.abstractapi.com/v1/?api_key={apiKey}&phone={phoneNumber}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
               

                // Assuming jsonResponse is the JSON string you received
                var phoneValidationResponse = JsonConvert.DeserializeObject<PhoneValidationResponse>(content);

                //Console.WriteLine(content);
                // var phoneValidationResponse = JsonSerializer.Deserialize<PhoneValidationResponse>(content);

                // Print the parsed JSON response
                Console.WriteLine($"Phone: {phoneValidationResponse.Phone}");
                Console.WriteLine($"Valid: {phoneValidationResponse.Valid}");
                Console.WriteLine($"Country: {phoneValidationResponse.Country.Name}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"JSON Parsing error: {e.Message}");
            }
        }
    }
}
