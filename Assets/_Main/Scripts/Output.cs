using System.Runtime.Serialization;
using Newtonsoft.Json;

[DataContract]
public class Output
{
    [DataMember(Name = "internalType", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "internalType")]
    public string internalType { get; set; }
    
    [DataMember(Name = "name", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "name")]
    public string name { get; set; }
    
    [DataMember(Name = "type", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "type")]
    public string type { get; set; }
    
}
