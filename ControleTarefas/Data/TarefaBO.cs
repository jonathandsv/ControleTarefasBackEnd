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

                    Tarefa tarefa = new Tarefa();

                    while (reader.Read())
                    {
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

        public int GetTarefaIniciada(int id)
        {
            string select = @"SELECT * FROM InicioTarefas WHERE IdTarefa = @Id ORDER BY Id DESC";
            int IdTarefaIniciada = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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

        public int GetTarefaFinalizada(int id)
        {
            string select = @"SELECT * FROM FimTarefas WHERE IdTarefa = @Id ORDER BY Id DESC";
            int IdTarefaIniciada = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(select, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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
    }
}
