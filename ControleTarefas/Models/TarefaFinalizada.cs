using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleTarefas.Models
{
    public class TarefaFinalizada
    {
        public int Ativo { get; set; }
        public string Nome { get; set; }
        public DateTime DataIniciada { get; set; }
        public DateTime DataFinalizada { get; set; }
    }
}
