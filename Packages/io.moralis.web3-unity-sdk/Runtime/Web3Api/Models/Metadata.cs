using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MoralisUnity.Web3Api.Models
{
    [DataContract]
    public class Metadata
    {
        [DataMember(Name = "image", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [DataMember(Name = "image_url", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DataMember(Name = "external_link", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "external_link")]
        public string ExternalLink { get; set; }

        [DataMember(Name = "animation_url", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "animation_url")]
        public string AnimationUrl { get; set; }

        [DataMember(Name = "attributes", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute> Attributes { get; set; }

    }
}