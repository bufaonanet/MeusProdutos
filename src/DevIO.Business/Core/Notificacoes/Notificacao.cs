namespace DevIO.Business.Core.Notificacoes
{
    public class Notificacao
    {        
        public string Mensage { get; }

        public Notificacao(string mensage)
        {
            Mensage = mensage;
        }
    }
}
