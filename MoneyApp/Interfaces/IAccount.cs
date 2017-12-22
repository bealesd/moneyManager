using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Interfaces
{
    public interface IAccount
    {
        [RegularExpression(@"^[A-Z]{1}[A-Za-z]{6,20}$")]
        string AccountName { get; set; }
        
        float AccountBalance { get; set; }
        Guid AccountGuid { get; set; }
    }
}
