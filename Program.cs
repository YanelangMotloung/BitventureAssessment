using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace BitventureAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonProcessor jsonProcessor = new JsonProcessor();
            jsonProcessor.Process(@"C:\Users\DELL\Downloads\bonus_endpoints.json");
            Console.ReadLine();
        }

        public static string LoadDummyData(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                   
                 return file.ReadToEnd();
                   
                }
            }
            else
            {
                return "";
            }

        }
    }

}

/***
 * Root root = ReadJSON(@"C:\Users\DELL\Downloads\bonus_endpoints.json");





        foreach (Service service in root.services)
        {
            DataTable responseObj = new DataTable();

            // HTTP GET.  
            using (var client = new HttpClient())
            {
                // Setting Base address. 

                client.BaseAddress = new Uri(service.BaseUrl.ToString());


                foreach (Endpoint endpoint in service.Endpoints)
                {
                    if (endpoint.Enabled)
                    {
                        // Initialization.  
                        HttpResponseMessage response = new HttpResponseMessage();

                        // HTTP GET  
                        response = await client.GetAsync(endpoint.Resource).ConfigureAwait(false);

                        // Verification  
                        if (response.IsSuccessStatusCode)
                        {
                            // Reading Response.  
                            string result = response.Content.ReadAsStringAsync().Result;
                            JObject joResponse = JObject.Parse(result);
                            string partten = string.Empty;

                            foreach (var item in joResponse)
                            {
                                if (item.Value is JArray)
                                {
                                    foreach (var v in item.Value)
                                    {
                                        Console.WriteLine(v);
                                    }
                                }
                            }


                            foreach (Response res in endpoint.Response)
                            {
                                JToken value = joResponse.SelectToken(res.Element);

                                if (joResponse.ContainsKey(res.Element))
                                {
                                    if (string.IsNullOrEmpty(res.Identifier))
                                    {
                                        partten = res.Regex;
                                    }
                                    else
                                    {
                                        partten = res.Identifier;
                                    }
                                    Regex reg = new Regex(partten);

                                    if (reg.IsMatch(joResponse.GetValue(res.Element).ToString()))
                                    {
                                        Console.WriteLine("Match");
                                    }

                                }

                            }

                        }
                    }

                }


            }
        }

        Console.WriteLine();
        Console.ReadLine();
    }














    public static Root ReadJSON(string path)
    {
        using (StreamReader file = new StreamReader(path))
        {
            try
            {
                string json = file.ReadToEnd();

                return JsonConvert.DeserializeObject<Root>(json);
            }
            catch (Exception)
            {
                Console.WriteLine("Problem reading file");

                return null;
            }
        }
    }
 * 
 * 
 */