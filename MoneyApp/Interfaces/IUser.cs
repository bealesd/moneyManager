using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Interfaces
{
    public interface IUser
    {
        Guid UserGuid { get; set; }
        [RegularExpression(@"^[A-Z]{1}[A-Za-z]{6,20}$")]
        List<Guid> AccountGuid { get; set; }
    }
}