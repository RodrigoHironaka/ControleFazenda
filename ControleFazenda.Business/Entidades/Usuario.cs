using ControleFazenda.Business.Entidades.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ControleFazenda.Business.Entidades
{
    public class Usuario : IdentityUser
    {
        [Required]
        public Fazenda Fazenda { get; set; } = Fazenda.Negrinha;
    }
}
