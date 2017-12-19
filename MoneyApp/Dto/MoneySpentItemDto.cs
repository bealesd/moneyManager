using System;
using Newtonsoft.Json;

namespace MoneyApp.Dto
{
    public class MoneySpentItemDto
    {
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }
        [JsonProperty("ItemCost")]
        public float ItemCost { get; set; }
        [JsonProperty("DateTime")]
        public DateTime DateTime { get; set; }
    }
}
