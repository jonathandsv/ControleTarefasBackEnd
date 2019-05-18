using ControleTarefas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ControleTarefas.Data
{
    public class TarefaBO
    {
        private string _connectionString;

        public TarefaBO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int SalvarTarefa(Tarefa tarefa)
        {
            string inserir = @"INSERT INTO Tarefas (Nome) VALUES (@Nome)";

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(inserir, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = tarefa.Nome;

                    var result = sqlCommand.ExecuteNonQuery();

                    return result;
                }
            }
        }

        public List<Tarefa> GetTarefas()
        {
            string select = @"SELECT * FROM Tarefas";
            List<Tarefa> tarefas = new List<Tarefa>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    SqlDataReader reader = sqlCommand.ExecuteReader();


                    while (reader.Read())
                    {
                        Tarefa tarefa = new Tarefa();
                        tarefa.Id = Convert.ToInt32(reader["Id"]);
                        tarefa.Nome = reader["Nome"].ToString();
                        tarefas.Add(tarefa);
                    }
                }
            }
            return tarefas;
        }

        public bool IniciarTarefa(int id, DateTime dataHora)
        {
            var inserir = @"INSERT INTO InicioTarefas (IdTarefa, dataHora) VALUES (@Id, @dataHora)";
            var result = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(inserir, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@dataHora", SqlDbType.DateTime).Value = dataHora;

                    result = sqlCommand.ExecuteNonQuery();
                }
            }

            var IdTarefaIniciada = GetTarefaIniciada(id);

            AtivarTarefa(IdTarefaIniciada);

            return (result > 0 ? true : false);
        }

        public int GetTarefaIniciada(int IdTarefa)
        {
            string select = @"SELECT TOP 1 * FROM InicioTarefas WHERE IdTarefa = @IdTarefa ORDER BY Id DESC";
            int IdTarefaIniciada = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@IdTarefa", SqlDbType.Int).Value = IdTarefa;

                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        IdTarefaIniciada = Convert.ToInt32(reader["Id"]);
                    }
                }
            }

            return IdTarefaIniciada;
        }

        public bool FinalizarTarefa(int id, DateTime dataHora)
        {
            string inserir = @"INSERT INTO FimTarefas (IdTarefa, dataHora) VALUES (@Id, @dataHora)";
            var result = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(inserir, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@dataHora", SqlDbType.DateTime).Value = dataHora;

                    result = sqlCommand.ExecuteNonQuery();
                }
            }

            var IdTarefaFinalizada = GetTarefaFinalizada(id);

            var IdTarefaIniciada = GetTarefaIniciada(id);

            DesativarTarefa(IdTarefaFinalizada, IdTarefaIniciada);

            return result > 0 ? true : false;
        }

        public int GetTarefaFinalizada(int IdTarefa)
        {
            string select = @"SELECT TOP 1 * FROM FimTarefas WHERE IdTarefa = @IdTarefa ORDER BY Id DESC";
            int IdTarefaIniciada = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@IdTarefa", SqlDbType.Int).Value = IdTarefa;

                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        IdTarefaIniciada = Convert.ToInt32(reader["Id"]);
                    }
                }
            }

            return IdTarefaIniciada;
        }

        public int AtivarTarefa(int IdInicioTarefa)
        {
            string inserir = @"INSERT INTO SessaoTarefas (IdInicioTarefas, Ativo) VALUES (@IdInicioTarefas, 1)";
            var result = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(inserir, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@IdInicioTarefas", SqlDbType.Int).Value = IdInicioTarefa;

                        result = sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

        public int DesativarTarefa(int IdFimTarefa, int IdTarefaIniciada)
        {
            string inserir = @"Update SessaoTarefas SET IdFimTarefas = @IdFimTarefas, Ativo = 0 WHERE IdInicioTarefas = @IdTarefaIniciada";

            var result = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(inserir, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@IdFimTarefas", SqlDbType.Int).Value = IdFimTarefa;
                        sqlCommand.Parameters.Add("@IdTarefaIniciada", SqlDbType.Int).Value = IdTarefaIniciada;

                        result = sqlCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Tarefa>> GetTarefasEmAndamento()
        {
            string select = @"SELECT IT.Id, IT.dataHora, T.Nome from InicioTarefas as IT 
                                inner join SessaoTarefas as ST on IT.Id = ST.IdInicioTarefas 
                                and ST.Ativo = 1 inner join Tarefas as T 
                                on T.Id = IT.IdTarefa";

            List<Tarefa> tarefas = new List<Tarefa>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        Tarefa tarefa = new Tarefa();

                        tarefa.Id = Convert.ToInt32(reader["Id"]);
                        tarefa.Nome = reader["Nome"].ToString();
                        tarefa.dataHora = Convert.ToDateTime(reader["dataHora"]);
                        tarefas.Add(tarefa);
                    }
                }
            }
            return tarefas;
        }

        public async Task<List<TarefaFinalizada>> GetTarefasFinalizadas()
        {
            string select = @"SELECT ST.Ativo, T.Nome, IT.dataHora AS DataIniciada, FT.dataHora AS DataFinalizada FROM SessaoTarefas AS ST 
                                INNER JOIN InicioTarefas AS IT ON ST.IdInicioTarefas = IT.Id AND ST.Ativo = 0
                                INNER JOIN FimTarefas AS FT ON FT.Id = ST.IdFimTarefas
                                INNER JOIN Tarefas AS T ON IT.IdTarefa = T.Id
                                WHERE Ativo = 0 ";

            List<TarefaFinalizada> tarefas = new List<TarefaFinalizada>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        TarefaFinalizada tarefa = new TarefaFinalizada();

                        tarefa.Ativo = Convert.ToInt32(reader["Ativo"]);
                        tarefa.Nome = reader["Nome"].ToString();
                        tarefa.DataIniciada = Convert.ToDateTime(reader["DataIniciada"]);
                        tarefa.DataFinalizada = Convert.ToDateTime(reader["DataFinalizada"]);
                        tarefas.Add(tarefa);
                    }
                }
            }
            return tarefas;
        }
    }
}
