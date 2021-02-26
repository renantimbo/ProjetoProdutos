using System.Collections.Generic;

namespace HBSIS.Padawan.Produtos.Domain.Result
{
    public class Result<T>
    {
        public T Return { get; set; }
        public bool Success { get; set; }
        public List<string> Messages { get; set; }

        public Result(bool success, string message)
        {
            Success = success;
            Messages = new List<string>();
            Messages.Add(message);
        }

        public Result(bool success, string message, T retorno) : this(success, message)
        {
            Return = retorno;
        }
    }
}