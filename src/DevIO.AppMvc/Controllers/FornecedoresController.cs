using AutoMapper;
using DevIO.AppMvc.ViewModels;
using DevIO.Business.Models.Fornecedores;
using DevIO.Business.Models.Fornecedores.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevIO.AppMvc.Controllers
{
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorService _fornecedorService;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorService fornecedorService,
                                      IFornecedorRepository fornecedorRepository,
                                      IMapper mapper)
        {
            _fornecedorService = fornecedorService;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        [Route("lista-de-fornecedores")]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            var fornecedorVm = await ObterFornecedorEnderecoVm(id);

            if (fornecedorVm == null)
            {
                return HttpNotFound();
            }

            return View(fornecedorVm);
        }

        [Route("novo-fornecedor")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<ActionResult> Create(FornecedorViewModel fornecedorVm)
        {
            if (!ModelState.IsValid) return View(fornecedorVm);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorVm);
            await _fornecedorService.Adicionar(fornecedor);

            //TODO: Tratar se possível falha

            return RedirectToAction("Index");
        }

        [Route("editar-fornecedor/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var fornecedorVm = await ObterFornecedorProdutoEnderecoVm(id);

            if (fornecedorVm is null) return HttpNotFound();

            return View(fornecedorVm);
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, FornecedorViewModel fornecedorVm)
        {
            if (id != fornecedorVm.Id) return HttpNotFound();

            if (!ModelState.IsValid) return View(fornecedorVm);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorVm);
            await _fornecedorService.Atualizar(fornecedor);

            //TODO: Tratar se possível falha

            return RedirectToAction("Index");
        }

        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var fornecedorVm = await ObterFornecedorEnderecoVm(id);

            if (fornecedorVm is null) return HttpNotFound();

            return View(fornecedorVm);
        }

        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorVm = await ObterFornecedorEnderecoVm(id);

            if (fornecedorVm is null) return HttpNotFound();

            await _fornecedorService.Remover(id);

            //TODO: Tratar se possível falha

            return RedirectToAction("Index");
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedorVm = await ObterFornecedorEnderecoVm(id);

            if (fornecedorVm is null) return HttpNotFound();

            return PartialView("_AtualizaEndereco", new FornecedorViewModel { Endereco = fornecedorVm.Endereco });
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<ActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));            

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorId });
            return Json(new { success = true, url });
        }


        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<ActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEnderecoVm(id);

            if (fornecedor == null)
            {
                return HttpNotFound();
            }

            return PartialView("_DetalhesEndereco", fornecedor);
        }

        private async Task<FornecedorViewModel> ObterFornecedorEnderecoVm(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutoEnderecoVm(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutoEndereco(id));
        }

    }
}