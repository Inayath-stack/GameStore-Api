using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public class CreateGameDto
{
    [Required]
    public string Name{get;set;}="";
    public string Genre{get;set;}="";
}