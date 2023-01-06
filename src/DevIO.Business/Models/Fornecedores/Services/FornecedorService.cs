using System.Threading.Tasks;
using System;
using DevIO.Business.Core.Services;
using DevIO.Business.Models.Fornecedores.Validations;
using System.Linq;
using DevIO.Business.Core.Notificacoes;

namespace DevIO.Business.Models.Fornecedores.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepositoy;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
                                 IEnderecoRepository enderecoRepositoy,
                                 INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepositoy = enderecoRepositoy;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) ||
                !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            if (await FornecedorExistente(fornecedor)) return;

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            if (await FornecedorExistente(fornecedor)) return;

            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task Remover(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterFornecedorProdutoEndereco(id);

            if (fornecedor.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return;
            }

            if (fornecedor.Endereco != null)
            {
                await _enderecoRepositoy.Remover(fornecedor.Endereco.Id);
            }

            await _fornecedorRepository.Remover(fornecedor.Id);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepositoy.Atualizar(endereco);
        }

        private async Task<bool> FornecedorExistente(Fornecedor fornecedor)
        {
            var fornecedorAtual = await _fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id);

            if (!fornecedorAtual.Any()) return false;

            Notificar("Já existe um fornecedor com este documento informado.");

            return true;
        }
        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepositoy?.Dispose();
        }
    }
}
