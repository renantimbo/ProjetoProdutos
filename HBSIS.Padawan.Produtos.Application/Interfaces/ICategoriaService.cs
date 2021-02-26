﻿using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;

namespace HBSIS.Padawan.Produtos.Application.Interfaces
{
    public interface ICategoriaService : IBaseService<Categoria, ICategoriaRepository>
    {
    }
}