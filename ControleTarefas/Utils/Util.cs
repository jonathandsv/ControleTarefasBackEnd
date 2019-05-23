using ControleTarefas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleTarefas.Utils
{
    public class Util
    {
        public bool ValidaUsuarioLogin(Usuario usuario)
        {
            if (!String.IsNullOrEmpty(usuario.Nome) && !String.IsNullOrEmpty(usuario.Senha))
            {
                return true;
            }
            else
                return false;
        }
    }
}
