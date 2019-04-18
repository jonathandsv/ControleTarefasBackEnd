using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleTarefas.Models;
using Microsoft.Extensions.Configuration;
using ControleTarefas.Data;
using Newtonsoft.Json;

namespace ControleTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private IConfiguration _config;

        public TarefasController(IConfiguration config)
        {
            _config = config;
        }
        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            var tarefas = new TarefaBO(_config.GetConnectionString("DefaultConnection")).GetTarefas();
            
            return new JsonResult(tarefas);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult InserirTarefa([FromBody] Tarefa tarefa)
        {
            var inserirTarefa = new TarefaBO(_config.GetConnectionString("DefaultConnection")).SalvarTarefa(tarefa);

            return Ok("Inserido com sucesso!");
        }

        [HttpPost]
        [Route("IniciarTarefa")]
        public IActionResult IniciarTarefa(Tarefa tarefa)
        {
            try
            {
                var tarefaBO = new TarefaBO(_config.GetConnectionString("DefaultConnection")).IniciarTarefa(tarefa.Id, tarefa.dataHora);

                if (tarefaBO == true)
                    return Ok("Tarefa Iniciada!");
                else
                    return BadRequest("Erro ao iniciar tarefa!");
                
            }
            catch (Exception)
            {
                return BadRequest("Problema ao iniciar Tarefa!");
            }
        }


        [HttpPost]
        [Route("FinalizarTarefa")]
        public IActionResult FinalizarTarefa(Tarefa tarefa)
        {
            try
            {
                var tarefaBO = new TarefaBO(_config.GetConnectionString("DefaultConnection")).FinalizarTarefa(tarefa.Id, tarefa.dataHora);

                if (tarefaBO)
                    return Ok("Tarefa Finalizada com sucesso!");
                else
                    return BadRequest("Tarefa não Finalizada!");
            }
            catch (Exception)
            {
                return BadRequest("Problema ao finalizar Tarefa!");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
