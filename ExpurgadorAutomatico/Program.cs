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
        /// <summary> Método principal de entrada para a aplicação. Espera 3 argumentos:
        ///     endereço para exclusão, quantidade de arquivos mantidos e endereço para logs (opcional) </summary>
        /// <param name="args"> Os argumentos passados para o método </param>
        static void Main(string[] args)
        {
            //Cria-se a variável de validação e a instancia
            IValidadorParametros validador = new ValidadorParametros();
            //Cria-se a variável para registro de mensagens - que será instanciada a seguir
            ILogger logger;

            //Tenta criar uma instância do logger utilizando o endereço passado, mas caso não tenha sido passado nenhum endereço,
            //  ou seja lançada alguma exceção ao criá-lo, crie a instância padrão
            try
            {
                //Caso tenham sido passados pelo menos 3 parâmetros, espera-se que o terceiro seja o endereço para registro dos logs
                if (args.Length >= 3)
                    //Cria uma nova instância de logger com base no endereço passado por parâmetro
                    logger = LoggerFactory.CreateLogger(args[2]);
                else
                    logger = LoggerFactory.CreateLogger();
            }
            catch
            {
                logger = LoggerFactory.CreateLogger();
            }
            
            //Utiliza a instância do logger apenas durante a execução do sistema
            using (logger)
            {
                //Garante que caso ocorra alguma exceção inesperada o sistema irá registrar corretamente o erro
                try
                {
                    #region Validação dos parâmetros
                    //Verifica se os dados enviados são válidos
                    if (!validador.ValidarParametros(args, logger))
                    {
                        logger.RegistrarMensagem("Os parâmetros passados não são válidos, o aplicativo será encerrado.");
                        return;
                    }

                    //Verifica se o diretório passado como parâmetro não existe - e encerra o aplicativo nesse caso.
                    var diretorio = new DirectoryInfo(args[0]);
                    if (!diretorio.Exists)
                    {
                        logger.RegistrarMensagem("O diretorio não existe, o aplicativo será encerrado.");
                        return;
                    }
                    #endregion

                    //Cria variáveis locais mais legíveis para os parâmetros passados
                    string enderecoArquivos = args[0];
                    int quantidadeArquivosMantidos = int.Parse(args[1]);

                    #region Feedback inicial ao usuário (informações iniciais)
                    //Obtém a lista de arquivos armazenadas no diretório especificado (como vetor)
                    var arquivos = diretorio
                        .GetFiles()
                        .OrderByDescending(a => a.LastWriteTimeUtc);

                    //Se os dados foram considerados válidos, e os arquivos da pasta já obtidos,
                    //  registre o início do aplicado e as informações de arquivos do diretório
                    logger.RegistrarMensagem(string.Format("Iniciando aplicativo ExpurgadorAutomatico no endereço: {0} ", enderecoArquivos));
                    logger.RegistrarMensagem(string.Format("Quantidade de arquivos que deverão ser mantidos: {0} ", quantidadeArquivosMantidos));
                    logger.RegistrarMensagem(string.Format("Quantidade de arquivos presentes no diretório informado: {0} ", arquivos.Count()));
                    logger.RegistrarMensagem(""); //Quebra de linha
                    
                    logger.RegistrarMensagem("Arquivos presentes no diretório:");
                    //Lista os arquivos no logger, exibindo o nome e data de modificação de cada
                    foreach (var arquivo in arquivos)
                    {
                        logger.RegistrarMensagem(string.Format("  -Nome do Arquivo: {0};", arquivo.Name));
                        logger.RegistrarMensagem(string.Format("    -Data de Modificação do Arquivo: {0};", arquivo.LastWriteTimeUtc) );
                          
                    }
                    logger.RegistrarMensagem(""); //Quebra de linha
                    #endregion

                    #region Exclusão (se necessário) dos arquivos que excedam a quantidade estipulada
                    //Verifica se não há nenhum arquivo que seria deletado
                    if (arquivos.Count() <= quantidadeArquivosMantidos)
                    {
                        logger.RegistrarMensagem("Não há nenhum arquivo para ser deletado.");
                        return;
                    }

                    //Obtém a lista de arquivos que serão excluídos, "pulando" a quantidade de arquivos definidas por parâmetro
                    var arquivosFiltrados = arquivos
                        .Skip(quantidadeArquivosMantidos);

                    //Executa em paralelo a deleção de todos os arquivos filtrados - A execução em paralelo é dispensável em testes com arquivos pequenos,
                    //  mas em produção com arquivos pesados como backups de bancos de dados, ela pode ser extremamente importante em termos de performance
                    Parallel.ForEach(arquivosFiltrados, arquivoFiltrado =>
                    {
                        logger.RegistrarMensagem( string.Format("Arquivo {0} ({1}) deletado.", arquivoFiltrado.Name, arquivoFiltrado.LastWriteTimeUtc) );
                        arquivoFiltrado.Delete();
                    });
                    #endregion
                }
                catch (Exception ex)
                {
                    logger.RegistrarMensagem(string.Format("Ocorreu uma exceção inesperada: {0}\nRastreamento de pilha: {1}", ex.Message, ex.StackTrace));
                }
            }
        }
    }
}
