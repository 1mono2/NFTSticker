using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

[DataContract]
public class TotalSupply
{
    [DataMember(Name = "inputs", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "inputs")]
    public List<string> inputs { get; set; }
    
    [DataMember(Name = "name", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "name")]
    public string name { get; set; }
    
    [DataMember(Name = "outputs", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "outputs")]
    public List<Output> outputs { get; set; }
    
    [DataMember(Name = "stateMutability", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "stateMutability")]
    public string stateMutability { get; set; }
    
    [DataMember(Name = "type", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "type")]
    public string type { get; set; }
}
