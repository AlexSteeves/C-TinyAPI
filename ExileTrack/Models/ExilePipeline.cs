using System;


using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;


using System.Text;


namespace ExileTrack.Models
{


    class ExilePipeline
    {




        public static async Task Main(string[] args)
        {


            await GetBeasts("Farric Tiger Alpha");
            await GetBeasts("Farrul, First of the Plains");
            //GetAuth().GetAwaiter().GetResult();
            //await SendPostRequest();
            /*
            var materials = GetMaterialsList("./data.JSON");
            var totalCost = 0.0;
            foreach (var material in materials)
            {

                totalCost += await GetItemPrice(material.Key) * material.Value;
            }
            Console.WriteLine($"Total Cost of Materials:{Math.Round(totalCost, 2)}");
            */
            //await GetFragmentPrice("Fragment of the Hydra");
            //await GetMapPrice("Lair of the Hydra Map");
        }

        public static double ExilePipeLine()
        {
            // Example implementation. Replace with your actual logic.
            return 42.0; // Returning a dummy value for demonstration.
        }



        public static Dictionary<string, int> GetMaterialsList(string pathToJson)
        {

            var materials = new Dictionary<string, int>();
            try
            {
                var json = File.ReadAllText(pathToJson);
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);


                var materialsArray = jsonObject?.Materials;
                foreach (var material in materialsArray!)
                {
                    foreach (JProperty prop in material)
                    {
                        materials.Add(prop.Name, (int)prop.Value);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return materials;

        }

        public static async Task<double> GetItemPrice(string itemName)
        {
            var url = "https://poe.ninja/api/data/currencyoverview?league=Affliction&type=Currency";

            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);

                // Parse the JSON to a dynamic object
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

                // Ensure 'lines' is not null before accessing it
                if (jsonObject?.lines != null)
                {
                    foreach (var item in jsonObject.lines)
                    {
                        if ((string)item.currencyTypeName == itemName)
                        {
                            // Assuming chaosEquivalent is the price and it's a double
                            return item.chaosEquivalent != null ? (double)item.chaosEquivalent : 0.0;
                        }
                    }
                }
            }
            return 0.0; // Return 0 or an indicative value if the item is not found or if lines is null
        }


        public static async Task<Dictionary<string, double>> GetBeasts(string beastName)
        {

            var beastURL = "https://poe.ninja/api/data/itemoverview?league=Affliction&type=Beast";
            var beastPrice = 0;

            string[] beastNames = ["Farric Tiger Alpha", "Vivid Vulture", "Wild Hellion Alpha"];

            Dictionary<string, double> result = new Dictionary<string, double>();


            using (var httpClient = new HttpClient())
            {
                var beastjson = await httpClient.GetStringAsync(beastURL);


                // Parse the JSON to a dynamic object
                var beastObject = JsonConvert.DeserializeObject<dynamic>(beastjson);


                // Ensure 'lines' is not null before accessing it
                if (beastObject?.lines != null)
                {
                    foreach (string name in beastNames)
                    {
                        foreach (var item in beastObject.lines)
                        {
                            if ((string)item.name == name)
                            {
                                // Assuming chaosEquivalent is the price and it's a double
                                Console.WriteLine($"{name}: {(double)item.chaosValue}");

                                result.Add(name, beastPrice);
                                beastPrice = item.chaosValue;


                            }
                        }
                    }

                }

                return result;

            }
        }


        public static async Task GetFragmentPrice(String fragmentName)
        {

            var fragmentURL = "https://poe.ninja/api/data/currencyoverview?league=Affliction&type=Fragment";

            using (var httpClient = new HttpClient())
            {
                var fragmentjson = await httpClient.GetStringAsync(fragmentURL);


                // Parse the JSON to a dynamic object
                var fragmentObject = JsonConvert.DeserializeObject<dynamic>(fragmentjson);


                // Ensure 'lines' is not null before accessing it
                if (fragmentObject?.lines != null)
                {
                    foreach (var item in fragmentObject.lines)
                    {
                        if ((string)item.currencyTypeName == fragmentName)
                        {
                            // Assuming chaosEquivalent is the price and it's a double
                            Console.WriteLine($"{fragmentName}: {(double)item.chaosEquivalent}");

                        }
                    }
                }

            }
        }


        public static async Task GetMapPrice(String mapName)
        {

            var mapURL = "https://poe.ninja/api/data/itemoverview?league=Affliction&type=Map";

            using (var httpClient = new HttpClient())
            {
                var mapjson = await httpClient.GetStringAsync(mapURL);


                var mapObject = JsonConvert.DeserializeObject<dynamic>(mapjson);

                // Ensure 'lines' is not null before accessing it
                if (mapObject?.lines != null)
                {
                    foreach (var item in mapObject.lines)
                    {
                        if ((string)item.name == mapName)
                        {
                            // Assuming chaosEquivalent is the price and it's a double
                            Console.WriteLine($"{mapName}: {(double)item.chaosValue}");

                        }
                    }
                }

            }
        }







    }
}