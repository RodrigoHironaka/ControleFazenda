using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades
{
    public class Malote : Entidade
    {
        public Int32 Numero { get; set; }
        public string? QuemLevou { get; set; }
        public bool Enviado { get; set; }
        public string? Descricao { get; set; }
    }
}
