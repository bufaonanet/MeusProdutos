using DevIO.Business.Models.Fornecedores;
using DevIO.Infra.Data.Context;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Infra.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(MeuDbContext db) : base(db) { }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await _db.Fornecedores
                .AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutoEndereco(Guid id)
        {
            return await _db.Fornecedores
                .AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Produtos)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public override async Task Remover(Guid id)
        {
            var fornecedor = await ObterPorId(id);
            if (fornecedor != null)
            {
                fornecedor.Ativo = false;
                await Atualizar(fornecedor);
            }
        }

        public async Task<Fornecedor> TesteBusca()
        {
            var fornecedores = await Buscar(f => f.Ativo && f.Documento == "" && f.Nome == "");
            return fornecedores.FirstOrDefault();
        }
    }
}