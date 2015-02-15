using SimpleLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpurgadorAutomatico
{
    class Program
    {
        static void Main(string[] args)
        {
            //Instancia a variável logger com uma instância da classe ConsoleLogger (que "registra" as mensagens apenas na tela de console e aguarda a
            //  entrada do usuário ao ser encerrada a execução)
            ILogger logger = new ConsoleLogger();
            IValidadorParametros validador = new ValidadorParametros();
            
            //Utiliza a instância do logger apenas durante a execução do sistema
            using (logger)
            {
                //Garante que caso ocorra alguma exceção inesperada o sistema irá registrar corretamente o erro
                try
                {
                    //Verifica se os dados enviados são válidos
                    if (!validador.ValidarParametros(args, logger))
                        return;


                    //Verifica se o diretório passado como parâmetro não existe - e instancia o diretório nesse caso.
                    var diretorio = new DirectoryInfo(args[0]);
                    if (!diretorio.Exists)
                    {
                        logger.RegistrarMensagem("O diretorio não existe.");
                        diretorio.Create();
                        logger.RegistrarMensagem("Diretório criado.");
                    }

                    //Obtém a lista de arquivos armazenadas no diretório especificado (como vetor)
                    var arquivos = diretorio.GetFiles();

                    //Obtém a lista de arquivos que serão excluídos ordenando-os pela data de modificação ("pulando" a quantidade de arquivos definidas por parâmetro)
                    var arquivosFiltrados = arquivos
                        .OrderByDescending(a => a.LastWriteTimeUtc)
                        .Skip(int.Parse(args[1]));

                    //Verifica se não há nenhum arquivo que seria deletado
                    if (arquivosFiltrados.Count() <= 0)
                    {
                        logger.RegistrarMensagem("Não há nenhum arquivo para ser deletado.");
                        return;
                    }

                    //Executa em paralelo a deleção de todos os arquivos filtrados - A execução em paralelo é dispensável em testes com arquivos pequenos,
                    //  mas em produção com arquivos pesados como backups de bancos de dados, ela pode ser extremamente importante em termos de performance
                    Parallel.ForEach(arquivosFiltrados, arquivoFiltrado =>
                    {
                        logger.RegistrarMensagem( string.Format("Arquivo {0} ({1}) deletado.", arquivoFiltrado.Name, arquivoFiltrado.LastWriteTimeUtc) );
                        arquivoFiltrado.Delete();
                    });
                }
                catch (Exception ex)
                {
                    logger.RegistrarMensagem(string.Format("Ocorreu uma exceção inesperada: {0}\nRastreamento de pilha: {1}", ex.Message, ex.StackTrace));
                }
            }
        }
    }
}
