using System;
using System.ComponentModel.DataAnnotations;

namespace HBSIS.Padawan.Produtos.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        private Guid _Id;
        public Guid Id 
        { 
            get => _Id; 
            set => _Id = Guid.NewGuid(); 
        }
    }
}