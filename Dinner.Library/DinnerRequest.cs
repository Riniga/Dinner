using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Dinner.Library;
public class DinnerRequest
{
    public DinnerRequest(string name, string email, string restaurant, DateTime time)
    {
        Name=name;
        Email=email;
        Restaurant=restaurant;
        Time=time;
        Id = Guid.NewGuid();

    }
    public Guid Id;
    public string Email { get; set; }
    public string Name { get; set; }
    public string Restaurant { get; set; }
    public DateTime Time { get; set; }
}