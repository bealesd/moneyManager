using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MoneyApp.Dto
{
    public class MoneySpentItemDto
    {
        [JsonProperty("ItemName")]
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string ItemName { get; set; }
        [JsonProperty("ItemCost")]
        [Required]
        [DataType(DataType.Currency)]
        public float ItemCost { get; set; }
        [JsonProperty("DateTime")]
        public DateTime DateTime { get; set; }
    }
}
