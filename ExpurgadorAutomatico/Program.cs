using SimpleLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExpurgadorAutomatico
{
    class Program
    {
        static void Main(string[] args)
        {
            //Instancia a variável logger com uma instância da classe ConsoleLogger (que "registra" as mensagens apenas na tela de console e aguarda a
            //  entrada do usuário ao ser encerrada a execução)
            ILogger logger = new ConsoleLogger();
            
            //Utiliza a instância do logger apenas durante a execução do sistema
            using (logger)
            {
                //Garante que caso ocorra alguma exceção inesperada o sistema irá registrar corretamente o erro
                try
                {

                    //Verifica se não foi passado um dos parâmetros necessários, registra uma nesagem de erro e encerra a aplicação
                    if (args.Length < 2
                        || String.IsNullOrEmpty(args[0])
                        || String.IsNullOrEmpty(args[1]))
                    {
                        logger.RegistrarMensagem("Não foram passados os parâmetros necessários.");
                        logger.RegistrarMensagem("Informe o endereço e a quantidade de arquivos que deve ser mantida.");
                        return;
                    }

                    //Converte o segundo parâmetro para inteiro (e encerra a aplicação caso não consiga converter)
                    try
                    {
                        int quantidadeArquivos = int.Parse(args[1]);
                    }
                    catch (FormatException)
                    {
                        logger.RegistrarMensagem("O segundo parâmetro deve ser um número");
                        return;
                    }

                    //Verifica se o diretório passado como parâmetro existe ou não
                    DirectoryInfo diretorio = new DirectoryInfo(args[0]);
                    if (diretorio.Exists)
                    {
                        logger.RegistrarMensagem("O diretorio existe.");
                    }
                    else
                    {
                        logger.RegistrarMensagem("O diretorio não existe.");
                    }

                }
                catch (Exception ex)
                {
                    logger.RegistrarMensagem(string.Format("Ocorreu uma exceção inesperada: {0}\nRastreamento de pilha: {1}", ex.Message, ex.StackTrace));
                }
            }
        }
    }
}
