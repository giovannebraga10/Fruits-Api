using Dapper;
using TesteNegocieOnline.Models;

namespace TesteNegocieOnline.Repositories
{
    public class FruitRepository
    {
        string connectionString; // atributo para receber a connection string
        public FruitRepository(IConfiguration configuration) // construtor para receber a injeção de dependencia das configurações da api
        {
            connectionString = configuration.GetConnectionString("DefaultConnectionString"); // exporta a connection string da configuração para o atributo da classe
        }
        public async Task InsertFruit(Fruit fruit)
        {
            using (var connection = new Npgsql.NpgsqlConnection(connectionString)) // using para abrir um escopo temporario com a conexão para o banco de dados
            {
                await connection.OpenAsync(); // abre a conexão
                await connection.ExecuteAsync("INSERT INTO \"Fruits\" (id, name, family, \"order\", genus) VALUES (@id, @name, @family, @order, @genus)", fruit); // executa comandos que NÃO DEPENDEM DE RETORNO!
            }
        }
        public async Task<IEnumerable<Fruit>> SelectAllFruits()
        {
            using (var connection = new Npgsql.NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var fruits = await connection.QueryAsync<Fruit>("SELECT * FROM \"Fruits\""); // Executa comandos de busca de dados e retorna um array de linhas
                return fruits;
            }
        }
    }
}
