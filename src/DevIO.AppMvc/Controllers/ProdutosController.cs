using AutoMapper;
using DevIO.AppMvc.ViewModels;
using DevIO.Business.Models.Produtos;
using DevIO.Business.Models.Produtos.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevIO.AppMvc.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, 
                                  IProdutoService produtoService, 
                                  IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [Route("lista-produtos")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var produtosVm = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
            return View(produtosVm);
        }

        
        [Route("dados-do-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            var produtoVm = await ObterProdutoVm(id);
            if (produtoVm == null)
            {
                return HttpNotFound();
            }
            return View(produtoVm);
        }

        [Route("novo-produto")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProdutoViewModel produtoVm)
        {
            if (ModelState.IsValid)
            {
                await _produtoService.Adicionar(_mapper.Map<Produto>(produtoVm));
                return RedirectToAction("Index");
            }

            return View(produtoVm);
        }

        [Route("editar-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var produtoVm = await ObterProdutoVm(id);
            if (produtoVm == null)
            {
                return HttpNotFound();
            }
            return View(produtoVm);
        }

        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProdutoViewModel produtoViewModel)
        {
            if (ModelState.IsValid)
            {
                await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));
                return RedirectToAction("Index");
            }
            return View(produtoViewModel);
        }

        [Route("excluir-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var produtoVm = await ObterProdutoVm(id);
            if (produtoVm == null)
            {
                return HttpNotFound();
            }
            return View(produtoVm);
        }
               
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var produtoVm = await ObterProdutoVm(id);
            if (produtoVm == null)
            {
                return HttpNotFound();
            }
            await _produtoService.Remover(produtoVm.Id);
            return RedirectToAction("Index");
        }

        private async Task<ProdutoViewModel> ObterProdutoVm(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            return produto;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _produtoService.Dispose();
                _produtoRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
