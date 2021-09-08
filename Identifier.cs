using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitventureAssessment
{
    public class Identifier
    {
        [JsonProperty("key")]
        public string Key;

        [JsonProperty("value")]
        public string Value;
    }
}
