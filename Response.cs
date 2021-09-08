using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

        public static void ComapareJSONResults(Identifier[] identifiers, Endpoint endpoint, HttpResponseMessage httpResponse, string dataType)
        {
            try
            {
                // Reading Response.  
                string httpResult = httpResponse.Content.ReadAsStringAsync().Result;
                JObject joResponse = JObject.Parse(httpResult);

                foreach (Response endpointResult in endpoint.Response)
                {
                    //Get the associated value of an element from the httpResults
                    string valueElement = GetJSONelementValue(joResponse, endpointResult.Element);

                    if (!string.IsNullOrEmpty(valueElement))
                    {
                        // Check if the Identifier is populated
                        if (string.IsNullOrEmpty(endpointResult.Identifier))
                        {
                            RegexMatcher(valueElement, endpointResult.Regex);
                        }
                        else
                        {
                            IdentifierMatcher(identifiers,valueElement, dataType, endpointResult.Identifier);
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

        private static void IdentifierMatcher(Identifier[] identifiers, string element,string dataType, string identifierChecker)
        {
            Regex reg = new Regex(identifierChecker);
            bool found = false;
            try
            {
                if (reg.IsMatch(element))
                {
                    Console.WriteLine("{0} from HttpResponce {1} Match Identifier {2} from JSON file", element,dataType, identifierChecker);
                }
                else
                {
                    // Bonus Code

                    if (identifiers.Length > 0)
                    { 
                        foreach (Identifier identifier in identifiers)
                        {
                            if(identifier.Key.CompareTo(identifierChecker) == 0)
                            {
                                found = true;
                                if (identifier.Value.CompareTo(element) == 0)
                                {
                                    Console.WriteLine("{0} from HttpResponce {1} Match Identifier {2} from JSON file", element, dataType, identifier.Value);
                                }
                                else
                                {
                                    Console.WriteLine("{0} from HttpResponce {1} Located But Mis Match Identifier {2} from JSON file", element, dataType, identifier.Value);
                                }

                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("{0} from HttpResponce {1}  MisMatch Identifier from JSON file", element, dataType);
                        }
                    }
                    else 
                    {
                        Console.WriteLine("{0} from HttpResponce {1}  Miss Match Identifier {2} from JSON file", element,dataType, identifierChecker);
                    }

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

        private static string GetJSONelementValue(JObject joResponse, string element)
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

        public static void ComapareXMLResults(Identifier[] identifiers, Endpoint endpoint, HttpResponseMessage httpResponse, string dataType)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(httpResponse.Content.ReadAsStringAsync().Result);
                var xml = XElement.Parse(xdoc.ToString()); // Parse the response

                foreach (Response endpointResult in endpoint.Response)
                {
                    //Get the associated value of an element from the httpResults
                    string valueElement = GetXMLElementValue(xml, endpointResult.Element);

                    if (!string.IsNullOrEmpty(valueElement))
                    {
                        // Check if the Identifier is populated
                        if (string.IsNullOrEmpty(endpointResult.Identifier))
                        {
                            RegexMatcher(valueElement, endpointResult.Regex);
                        }
                        else
                        {
                            IdentifierMatcher(identifiers, valueElement,dataType, endpointResult.Identifier);
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} element was not found on HttpResponse", endpointResult.Element);
                    }

                }

            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }
        }

        private static string GetXMLElementValue(XElement xml, string element)
        {
            string result = string.Empty;
            try
            {
                result = xml.Element(element).Value.ToString();
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }

            return result;
        }
    }
}
