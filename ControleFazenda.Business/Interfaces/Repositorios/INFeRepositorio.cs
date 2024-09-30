﻿using ControleFazenda.Business.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface INFeRepositorio : IRepositorio<NFe>
    {
        Task<NFe> ObterPorIdComFornecedor(Guid Id);
        Task<IEnumerable<NFe>> ObterTodosComFornecedor();
    }
}
