using System;


using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;


using System.Text;
using System.ComponentModel;


namespace ExileTrack.Models
{


    class ExilePipeline
    {




        public static async Task Main(string[] args)
        {


            //await GetBeasts("Farric Tiger Alpha");
            //await GetBeasts("Farrul, First of the Plains");
            
            //https://www.pathofexile.com/trade/search/Affliction/rMOdrQmIQ
            
            var materials = GetMaterialsList("./data.JSON");
            var totalCost = 0.0;
            foreach (var material in materials)
            {

                totalCost += await GetItemPrice(material.Key) * material.Value;
            }


    
            
            
            //await GetFragmentPrice("Fragment of the Hydra");
            //await GetMapPrice("Lair of the Hydra Map");
        }


        public static async Task<Dictionary<string, Dictionary<string, double>>> GetFlaskProfits(){
            var path = "./data.JSON";

            Dictionary<string, Dictionary<string, double>> result = new Dictionary<string, Dictionary<string, double>>();

            Dictionary<string, double> materialCost = GetMaterialsList(path);

           
            var totalCost = 0.0;
            foreach (var material in materialCost)
            {

                totalCost += await GetItemPrice(material.Key) * material.Value;
            }

            Dictionary<string, string> flaskBaseStats = getFlaskStats(path);
            materialCost.Add("Materials Cost", totalCost);
            
            result.Add(flaskBaseStats["Item Name"], materialCost);

            
            

            return result;
            
        }

      



        public static Dictionary<string, double> GetMaterialsList(string pathToJson)
        {

            var materials = new Dictionary<string, double>();
            try
            {
                var json = File.ReadAllText(pathToJson);
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);


                var materialsArray = jsonObject?.Materials;
                foreach (var material in materialsArray!)
                {
                    foreach (JProperty prop in material)
                    {
                        materials.Add(prop.Name, (double)prop.Value);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return materials;

        }
        public static Dictionary<string, string> getFlaskStats(string pathToJson){
            Dictionary<string, string> result = new Dictionary<string, string>();
          

            try{
                var json = File.ReadAllText(pathToJson);
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

                var baseTypesArray = jsonObject?["Base Type"];
                foreach (var material in baseTypesArray!)
                {
                    foreach (JProperty prop in material)
                    {
                        result.Add(prop.Name, (string)prop.Value!);
                    }
                }
               


            }catch(Exception ex){
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


            return result;

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