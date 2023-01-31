using System;
using System.Collections.Generic;

namespace BankAPI.Data.DTOs;

public partial class ClientDTO
{
    //public int Id { get; set; } 
    public string Email { get; set; } = null!;
    public string Pwd { get; set; } = null!;

}