using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    public Employee Employee { get; set; }
    public Member Member { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}
