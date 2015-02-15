using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpurgadorAutomatico
{
    /// <summary> Classe responsável pelas validações iniciais dos parâmetros passados para a aplicação </summary>
    public class ValidadorParametros : IValidadorParametros
    {
        /// <summary> Informa se os parâmetros passados são válidos ou não para a aplicação ExpurgadorAutomatico </summary>
        /// <param name="args"> Argumentos passados para a aplicação a serem validados </param>
        /// <returns> true caso passem em todos os testes, false caso não passem </returns>
        public bool ValidarParametros(string[] args)
        {
            return ValidarParametros(args, null);
        }

        /// <summary> Informa se os parâmetros passados são válidos ou não para a aplicação ExpurgadorAutomatico,
        ///  recebendo uma instância de ILogger para registro de erros </summary>
        /// <param name="args"> Argumentos passados para a aplicação a serem validados </param>
        /// <param name="logger"> Instância do ILogger para registro dos erros </param>
        /// <returns> true caso passem em todos os testes, false caso não passem </returns>
        public bool ValidarParametros(string[] args, SimpleLogger.ILogger logger)
        {
            //Verifica se não foi passado um dos parâmetros necessários, registra uma nesagem de erro e encerra a aplicação
            if (args.Length < 2
                || String.IsNullOrEmpty(args[0])
                || String.IsNullOrEmpty(args[1]))
            {
                if (logger != null)
                {
                    logger.RegistrarMensagem("Não foram passados os parâmetros necessários.");
                    logger.RegistrarMensagem("Informe o endereço e a quantidade de arquivos que deve ser mantida.");
                }
                return false;
            }

            //Converte o segundo parâmetro para inteiro (e retorna falso caso não consiga converter)
            try
            {
                int quantidadeArquivos = int.Parse(args[1]);
            }
            catch (FormatException)
            {
                if(logger != null)
                    logger.RegistrarMensagem("O segundo parâmetro deve ser um número");
                return false;
            }

            return true;
        }
    }
}
