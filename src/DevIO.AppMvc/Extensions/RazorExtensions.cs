using System.Web.Mvc;
using System;
using System.Web;

namespace DevIO.AppMvc.Extensions
{
    public static class RazorExtensions
    {
        public static string FormatarDocumento(this WebViewPage page, int tipoPessoa, string documento)
        {
            return tipoPessoa == 1
                ? Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00")
                : Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");
        }

        public static bool ExibirNaURL(this WebViewPage value, Guid Id)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var urlTarget = urlHelper.Action("Edit", "Fornecedores", new { id = Id });
            var urlTarget2 = urlHelper.Action("ObterEndereco", "Fornecedores", new { id = Id });

            var urlEmUso = HttpContext.Current.Request.Path;

            return urlTarget == urlEmUso || urlTarget2 == urlEmUso;
        }
    }
}