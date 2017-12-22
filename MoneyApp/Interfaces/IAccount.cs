using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Interfaces
{
    public interface IAccount
    {
        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        string AccountName { get; set; }
        
        float AccountBalance { get; set; }
        Guid AccountGuid { get; set; }
    }
}
