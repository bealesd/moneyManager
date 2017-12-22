using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MoneyApp.Dto
{
    public class MoneySpentItemDto
    {
        [JsonProperty("ItemName")]
        [RegularExpression(@"^[A-Z]{1}[A-Za-z]{3,20}$")]
        public string ItemName { get; set; }
        [JsonProperty("ItemCost")]
        [RegularExpression(@"^[0-9]{1,4}(\.[0-9]{1,2})?$")]
        public float ItemCost { get; set; }
        [JsonProperty("DateTime")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }
    }
}
