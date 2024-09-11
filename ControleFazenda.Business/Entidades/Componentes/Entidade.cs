namespace ControleFazenda.Business.Entidades.Componentes
{
    public abstract class Entidade
    {
        protected Entidade()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

        public Guid UsuarioCadastroId { get; set; }
        public Guid UsuarioAlteracaoId { get; set; }

    }
}
