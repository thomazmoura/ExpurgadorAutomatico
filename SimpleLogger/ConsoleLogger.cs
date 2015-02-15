using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLogger
{
    /// <summary> Logger para depuração - Digita as mensagens no console e, ao terminar a transação, aguarda uma entrada qualquer do usuário </summary>
    public class ConsoleLogger: ILogger
    {
        /// <summary> Construtor do ConsoleLogger. Efetua configurações estéticas </summary>
        public ConsoleLogger()
        {
            Console.WriteLine();
        }

        /// <summary> Insere uma mensagem como uma linha do Console </summary>
        /// <param name="Mensagem"> A mensagem a ser exibida na tela de Console </param>
        public void RegistrarMensagem(string Mensagem)
        {
            Console.WriteLine(Mensagem);
        }

        /// <summary> Ao se terminar todas as interações, o console é colocado em espera por uma entrada do usuário </summary>
        public void Dispose()
        {
            Console.WriteLine();
            Console.WriteLine("**Execução encerrada. Digite qualquer tecla para continuar.**");
            Console.ReadKey();
        }
    }
}
