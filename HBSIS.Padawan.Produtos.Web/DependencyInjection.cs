using FluentValidation;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Application.Services;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Interfaces.Validation;
using HBSIS.Padawan.Produtos.Domain.Validacoes;
using HBSIS.Padawan.Produtos.Domain.Validation;
using HBSIS.Padawan.Produtos.Infra.Csv;
using HBSIS.Padawan.Produtos.Infra.Repository.GenericRepository;
using Microsoft.Extensions.DependencyInjection;

namespace HBSIS.Padawan.Produtos.Web
{
    public static class DependencyInjection
    {
        public static void Inject(this IServiceCollection services)
        {
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IValidator<Fornecedor>, FornecedorValidation>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICategoriaCsvService, CategoriaCsvService>();
            services.AddScoped<IValidator<Categoria>, CategoriaValidation>();
            services.AddScoped<IValidator<CategoriaCsvDto>, ImportCategoriaValidation>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoCsvService, ProdutoCsvService>();
            services.AddScoped<IValidator<Produto>, ProdutoValidation>();
            services.AddScoped<IValidator<ProdutoCsvDto>, ImportProdutoValidation>();
        }
    }
}