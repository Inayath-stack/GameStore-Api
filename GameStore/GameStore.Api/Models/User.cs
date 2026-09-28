using GameStore.Api.Models;

namespace GameStore.Api.Models;

public class User
{
    public int Id{get;set;}
    public string Username{get;set;}="";
    public string Email{get;set;}="";
    public string PasswordHash{get;set;}="";
    public string Role{get;set;}="User";
    public List<RefreshToken> RefreshToken{get;set;}=new();

}