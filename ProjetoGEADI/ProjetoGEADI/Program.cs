using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace ProjetoGEADI
{
    class Program
    {
        static void Main(string[] args)
        {
            string pastaAlvo = @"C:\Users\rwmac\Documents\Projetos\PSI_Master\arquivos";
            string connectionString = "Server=RENATSPC;Database=ADIMPLENCIA;Integrated Security=True;";

            // Obter todos os arquivos da pasta
            var arquivos = Directory.GetFiles(pastaAlvo);

            // Para cada arquivo, capture as informações necessárias
            foreach (var arquivo in arquivos)
            {
                FileInfo infoArquivo = new FileInfo(arquivo);

                string nome = infoArquivo.Name;
                string endereco = infoArquivo.FullName;
                // Aqui converte o tamanho de bytes para kilobytes
                double tamanhoKB = infoArquivo.Length / 1024.0;
                DateTime dataHora = infoArquivo.CreationTime;

                // Insira essas informações no banco de dados
                InserirDadosNoBanco(nome, endereco, tamanhoKB, dataHora, connectionString);
            }

            Console.WriteLine("Processo concluído.");
        }

        static void InserirDadosNoBanco(string nome, string endereco, double tamanhoKB, DateTime dataHora, string connectionString)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO ArquivosCapturados (Nome, Endereco, Tamanho, DataHoraDisponibilizacao) " +
                               "VALUES (@Nome, @Endereco, @Tamanho, @DataHora)";

                using (SqlCommand comando = new SqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", nome);
                    comando.Parameters.AddWithValue("@Endereco", endereco);
                    comando.Parameters.AddWithValue("@Tamanho", tamanhoKB);  // Aqui o valor em KB
                    comando.Parameters.AddWithValue("@DataHora", dataHora);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    conexao.Close();
                }
            }
        }
    }
}
