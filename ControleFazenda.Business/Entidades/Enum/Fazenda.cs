using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades.Enum
{
    public enum Fazenda
    {
        [Display(Name = "Fazenda Negrinha - Parapuã - SP")]
        Negrinha,
        [Display(Name = "Fazenda Tamara - Salmorão - SP")]
        Tamara,
        [Display(Name = "Fazenda Angela - Salmorão - SP")]
        Angela,
        [Display(Name = "Fazenda Jaguaretê - Guararapes - SP")]
        Jaguaretê,
        [Display(Name = "Fazenda Ianduy - ver cidade - PR")]
        Ianduy,
        [Display(Name = "Fazenda Boca da Onça - ver cidade - MS")]
        BocaDaOnça
    }
}
