using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace BitventureAssessment
{
    public partial class Response
    {
        // logging to the file
        static ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [JsonProperty("element")]
        public string Element { get; set; }

        [JsonProperty("regex", NullValueHandling = NullValueHandling.Ignore)]
        public string Regex { get; set; }

        [JsonProperty("identifier", NullValueHandling = NullValueHandling.Ignore)]
        public string Identifier { get; set; }

        public static void ComapareResults(Endpoint endpoint, HttpResponseMessage httpResponse)
        {
            try
            {
                // Reading Response.  
                string httpResult = httpResponse.Content.ReadAsStringAsync().Result;
                JObject joResponse = JObject.Parse(httpResult);

                foreach (Response endpointResult in endpoint.Response)
                {
                    //Get the associated value of an element from the httpResults
                    string valueElement = GetElementValue(joResponse, endpointResult.Element);

                    if (!string.IsNullOrEmpty(valueElement))
                    {
                        // Check if the Identifier is populated
                        if (string.IsNullOrEmpty(endpointResult.Identifier))
                        {
                            RegexMatcher(valueElement, endpointResult.Regex);
                        }
                        else
                        {
                            IdentifierMatcher(valueElement, endpointResult.Identifier);
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} Was not found from HttpResponse", endpointResult.Element);
                    }

                }
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }
        }

        private static void IdentifierMatcher( string element, string identifier)
        {
            Regex reg = new Regex(identifier);

            try
            {
                if (reg.IsMatch(element))
                {
                    Console.WriteLine("{0} from HttpResponce JSON Match Identifier {1} from JSON file", element, identifier);
                }
                else
                {
                    Console.WriteLine("{0} from HttpResponce JSON  Miss Match Identifier {1} from JSON file", element, identifier);
                }
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }
        }

        private static void RegexMatcher(string value, string regexString)
        {
            Regex reg = new Regex(regexString);
            try
            {
                if (reg.IsMatch(value))
                {
                    Console.WriteLine("{0} from HttpResponce JSON Match Regex {1} from JSON file", value, regexString);
                }
                else
                {
                    Console.WriteLine("{0} from HttpResponce JSON  Miss Match Regex {1} from JSON file", value, regexString);
                }
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }
        }

        private static string GetElementValue(JObject joResponse, string element)
        {
            string response = string.Empty;
            try
            {
                if (joResponse.ContainsKey(element))
                {
                    return joResponse.GetValue(element).ToString();
                }
                else
                {
                    foreach (var item in joResponse)
                    {
                        if (item.Value.HasValues)
                        {
                            if (item.Value is JArray)
                            {
                                foreach (var v in item.Value)
                                {
                                    if (joResponse.ContainsKey(element))
                                    {
                                        return joResponse.GetValue(element).ToString();
                                    }
                                }
                            }
                            else
                            {
                                JObject dictObject = JObject.FromObject(item.Value);

                                if (dictObject.ContainsKey(element))
                                {
                                    return dictObject.GetValue(element).ToString();
                                }
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }

            return response;
        }
    }
}
