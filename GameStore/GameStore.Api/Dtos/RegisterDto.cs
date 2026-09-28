using System.ComponentModel.DataAnnotations;
namespace GameStore.Api.Dtos;

public class RegisterDto
{
    [Required]
    public string Username{get; set;}="";

    [Required]
    [EmailAddress]
    public string Email{get;set;}="";
    
    [Required]
    [MinLength(8)]
    public string Password{get;set;}="";
}