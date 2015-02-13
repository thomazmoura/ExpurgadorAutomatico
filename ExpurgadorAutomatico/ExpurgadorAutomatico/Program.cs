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
            //Verifica se não foi passado um dos parâmetros necessários, manda uma nesagem de erro e encerra a aplicação
            if ( args.Length < 2 
                || String.IsNullOrEmpty(args[0])
                || String.IsNullOrEmpty(args[1]) )
            {
                Console.WriteLine("Não foram passados os parâmetros necessários.");
                Console.WriteLine("Informe o endereço e a quantidade de arquivos que deve ser mantida.");
                Console.ReadKey();
                return;
            }

            try
            {
                int quantidadeArquivos = int.Parse(args[1]);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("O segundo parâmetro deve ser um número");
                Console.ReadKey();
                return;
            }

            DirectoryInfo diretorio = new DirectoryInfo(args[0]);
            if (diretorio.Exists)
            {
                Console.WriteLine("O diretorio existe.");
            }
            else
            {
                Console.WriteLine("O diretorio não existe.");
            }


            string endereco = args[0];

            Console.ReadKey();
        }
    }
}
