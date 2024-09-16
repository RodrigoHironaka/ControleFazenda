using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.Business.Entidades.Componentes
{
    [Owned]
    public class Endereco
    {
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Referencia { get; set; }
    }
}
