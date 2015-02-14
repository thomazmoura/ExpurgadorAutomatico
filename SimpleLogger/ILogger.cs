using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLogger
{
    /// <summary> Interface para criação de um logger simples - deve conter um método para registrar mensagens e o método Dispose para concluir a gravação das mensagens </summary>
    public interface ILogger: IDisposable
    {
        /// <summary> Registra uma mensagem de log no sistema </summary>
        /// <param name="Mensagem"> Mensagem a ser registrada ou exibida </param>
        void RegistrarMensagem(string Mensagem);
    }
}
