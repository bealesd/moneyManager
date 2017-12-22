using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Interfaces
{
    public interface IUser
    {
        Guid UserGuid { get; set; }
        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        string Username { get; set; }
        List<Guid> AccountGuid { get; set; }
    }
}
