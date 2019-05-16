using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleTarefas.Models;

namespace ControleTarefas.ViewModels
{
    public class TarefasVM
    {
        public List<Tarefa> Tarefas { get; set; }

        public TarefasVM()
        {
            Tarefas = new List<Tarefa>();
        }

    }
}
