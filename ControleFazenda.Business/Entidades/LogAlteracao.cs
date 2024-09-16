using ControleFazenda.Business.Entidades.Componentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades
{
    public class LogAlteracao : Entidade
    {
        public String? Chave { get; set; }
        public String? Historico { get; set; }
    }
}
