using System;
using System.Data.SqlClient;
using System.IO;

namespace ProjetoGEADI_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            string pastaAlvo = @"C:\Users\rwmac\Documents\Projetos\PSI GEADI\PSI_Master\arquivos";
            string connectionString = "Server=RENATSPC;Database=ADIMPLENCIA;Integrated Security=True;";

            // Obter todos os arquivos da pasta
            var arquivos = Directory.GetFiles(pastaAlvo);

            // Para cada arquivo, capture as informações necessárias
            foreach (var arquivo in arquivos)
            {
                FileInfo infoArquivo = new FileInfo(arquivo);

                string nome = infoArquivo.Name;
                string endereco = Path.GetFullPath(infoArquivo.FullName).ToLower(); // Normalizando o caminho do arquivo para evitar variações
                double tamanhoKB = infoArquivo.Length / 1024.0;
                DateTime dataHora = infoArquivo.CreationTime;

                // Verifique se o arquivo já existe antes de inserir
                if (!ArquivoJaExiste(nome, endereco, connectionString))
                {
                    InserirDadosNoBanco(nome, endereco, tamanhoKB, dataHora, connectionString);
                }
                else
                {
                    Console.WriteLine($"Arquivo {nome} já existe no banco de dados.");
                }
            }

            Console.WriteLine("Processo concluído.");
        }

        // Método para verificar se o arquivo já está no banco de dados
        static bool ArquivoJaExiste(string nome, string endereco, string connectionString)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string query = "SELECT 1 FROM ArquivosCapturados WHERE Nome = @Nome AND LOWER(Endereco) = @Endereco";

                using (SqlCommand comando = new SqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", nome);
                    comando.Parameters.AddWithValue("@Endereco", endereco);

                    conexao.Open();
                    var resultado = comando.ExecuteScalar();
                    conexao.Close();

                    // Retorna true se o arquivo existir (EXISTS retorna 1), caso contrário, false
                    return resultado != null;
                }
            }
        }

        // Método para inserir dados no banco de dados
        static void InserirDadosNoBanco(string nome, string endereco, double tamanhoKB, DateTime dataHora, string connectionString)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO ArquivosCapturados (Nome, Endereco, [Tamanho (Kb)], DataHoraDisponibilizacao) " +
                               "VALUES (@Nome, @Endereco, @TamanhoKb, @DataHora)";

                using (SqlCommand comando = new SqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", nome);
                    comando.Parameters.AddWithValue("@Endereco", endereco);
                    comando.Parameters.AddWithValue("@TamanhoKb", tamanhoKB);
                    comando.Parameters.AddWithValue("@DataHora", dataHora);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                    conexao.Close();
                }
            }
        }
    }
}
