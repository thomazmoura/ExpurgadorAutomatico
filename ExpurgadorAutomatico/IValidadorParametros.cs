using SimpleLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpurgadorAutomatico
{
    /// <summary> Interface para validar os parâmetros passados como string para a aplicação console </summary>
    public interface IValidadorParametros
    {
        /// <summary> Informa se os parâmetros passados são válidos ou não para a aplicação ExpurgadorAutomatico </summary>
        /// <param name="args"> Argumentos passados para a aplicação a serem validados </param>
        /// <returns> true caso passem em todos os testes, false caso não passem </returns>
        bool ValidarParametros(string[] args);

        /// <summary> Informa se os parâmetros passados são válidos ou não para a aplicação ExpurgadorAutomatico,
        ///  recebendo uma instância de ILogger para registro de erros </summary>
        /// <param name="args"> Argumentos passados para a aplicação a serem validados </param>
        /// <param name="logger"> Instância do ILogger para registro dos erros </param>
        /// <returns> true caso passem em todos os testes, false caso não passem </returns>
        bool ValidarParametros(string[] args, ILogger logger);
    }
}
