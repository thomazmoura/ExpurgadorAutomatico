using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLogger
{
    /// <summary> Logger responsável por registrar as mensagens enviadas em arquivo </summary>
    public class FileLogger: ILogger
    {
        /// <summary> Diretório onde serão mantidos os arquivos de Log com as mensagens registradas </summary>
        public DirectoryInfo Diretorio { get; private set; }

        /// <summary> Arquivo onde serão mantidos as mensagens de Log registradas por essa instância </summary>
        public FileInfo Arquivo { get; private set; }

        /// <summary> Construtor de strings responsável por armazenar todas as mensagens recebidas </summary>
        private StringBuilder ConstrutorMensagens { get; set; }

        /// <summary> Construtor da class, exige um diretorio como parâmetro e cria a pasta caso ela não exista </summary>
        /// <param name="diretorio"> O diretório no qual os arquivos de log serão gravados </param>
        public FileLogger(DirectoryInfo diretorio)
        {
            //Armazena o diretório na propriedade privada
            Diretorio = diretorio;
            //Caso o diretório não exista, cria-o
            if (!Diretorio.Exists)
            {
                Diretorio.Create();
            }

            //Define o nome do arquivo com base na data de Criação
            var nomeArquivo = string.Format("Log-De-Execucao-{0}.txt",  DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm"));

            //Cria a informação do arquivo com base no diretório e no nome do arquivo
            Arquivo = new FileInfo( Path.Combine(Diretorio.FullName, nomeArquivo) );

            //Instancia a propriedade Mensagens
            ConstrutorMensagens = new StringBuilder();
        }

        /// <summary> Registra a mensagem no construtor para ser gravado posteriormente</summary>
        /// <param name="Mensagem"> A mensagem a ser registrada </param>
        public void RegistrarMensagem(string Mensagem)
        {
            //Inclue a nova mensagem no construtor, separando-a da nova linha por uma quebra de linha
            ConstrutorMensagens.Append(Mensagem + Environment.NewLine);
        }

        /// <summary> Encerra a utilização do Logger registrando efetivamente as mensagens no arquivo </summary>
        public void Dispose()
        {
            //Caso não haja mensagens para serem registradas no arquivo, apenas deleta-se o arquivo
            if (string.IsNullOrEmpty(ConstrutorMensagens.ToString()))
            {
                return;
            }

            //Acrescenta uma última mensagem de conclusão
            ConstrutorMensagens.Append(Environment.NewLine + "**Gravação encerrada**");

            //Caso haja mensagens, elas serão gravadas no arquivo, para isso, utiliza-se um StreamWriter para gravação de texto
            using(StreamWriter streamWriter = Arquivo.CreateText()){
                streamWriter.Write(ConstrutorMensagens.ToString());
            }

        }
    }
}
