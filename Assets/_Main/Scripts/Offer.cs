
using System.Runtime.Serialization;
using Newtonsoft.Json;

[DataContract]
public class Offer
{
    [DataMember(Name = "nftContract", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "nftContract")]
    public string nftContract { get; set; }
    
    [DataMember(Name = "owner", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "owner")]
    public string owner { get; set; }
    
    [DataMember(Name = "tokenId", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "tokenId")]
    public string tokenId { get; set; }
    
    [DataMember(Name = "price", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "price")]
    public string price { get; set; }
    
}
