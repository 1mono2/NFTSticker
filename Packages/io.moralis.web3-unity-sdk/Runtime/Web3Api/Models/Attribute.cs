using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MoralisUnity.Web3Api.Models
{
    [DataContract]
    public class Attribute
    {
        [DataMember(Name = "trait_type", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "trait_type")]
        public string Key { get; set; }

        [DataMember(Name = "value", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}