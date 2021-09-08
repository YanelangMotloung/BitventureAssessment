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
    }
}
