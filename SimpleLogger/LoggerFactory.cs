using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLogger
{
    /// <summary> Fábrica para o Logger - Responsável por decidir qual implementação de ILogger instanciar </summary>
    public class LoggerFactory
    {
        public static ILogger CreateLogger( string caminhoDiretorio = null)
        {
            if (caminhoDiretorio != null)
            {
                var diretorio = new DirectoryInfo(caminhoDiretorio);
                var logger = new FileLogger(diretorio);
                var mensagemInicial = string.Format("Para informações de execução deste aplicativo acesse o log de registro em: {0}", logger.Arquivo.FullName);
                Console.WriteLine();
                Console.WriteLine(mensagemInicial);
                return logger;
            }
            return new ConsoleLogger();
        }
    }
}
