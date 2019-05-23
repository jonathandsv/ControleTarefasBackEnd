using ControleTarefas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ControleTarefas.Data
{
    public class UsuarioBO
    {
        private string _connection;

        public UsuarioBO(string connection)
        {
            _connection = connection;
        }

        public Usuario GetUsuario(Usuario usuario)
        {
            try
            {
                string select = @"SELECT * FROM Usuario WHERE Nome = @Nome AND Senha = @Senha";

                Usuario user = new Usuario();

                using (var sqlConnection = new SqlConnection(_connection))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(select, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = usuario.Nome;
                        sqlCommand.Parameters.Add("@Senha", SqlDbType.VarChar).Value = usuario.Senha;

                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            user.Nome = reader["Nome"].ToString();
                            user.Senha = reader["Senha"].ToString();
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
