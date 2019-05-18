using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleTarefas.Models;
using Microsoft.Extensions.Configuration;
using ControleTarefas.Data;
using Newtonsoft.Json;
using ControleTarefas.ViewModels;

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

            TarefasVM tarefasVM = new TarefasVM();
            tarefasVM.Tarefas = tarefas;

            return new JsonResult(tarefasVM);
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

        [HttpGet]
        [Route("MostrarTarefasIniciadas")]
        public async Task<ActionResult<Tarefa>> MostrarTarefasIniciadas()
        {
            try
            {
                var tarefas = new TarefaBO(_config.GetConnectionString("DefaultConnection"));
                var tarefaslista = await tarefas.GetTarefasEmAndamento();
                if (tarefaslista != null)
                {
                    return new JsonResult(tarefaslista);
                }
                else
                    return BadRequest("Não tem nenhuma tarefa iniciada!");
            }
            catch (Exception ex)
            {

                return BadRequest(@"Ocorreu um erro" + ex.Message + "");
            }
        }

        [HttpGet]
        [Route("MostrarTarefasFinalizadas")]
        public async Task<ActionResult<TarefaFinalizada>> MostrarTarefasFinalizadas()
        {
            try
            {
                var tarefas = new TarefaBO(_config.GetConnectionString("DefaultConnection"));
                var tarefaslista = await tarefas.GetTarefasFinalizadas();
                if (tarefaslista != null)
                {
                    return new JsonResult(tarefaslista);
                }
                else
                    return BadRequest("Não tem nenhuma tarefa Finalizada!");
            }
            catch (Exception ex)
            {

                return BadRequest(@"Ocorreu um erro" + ex.Message + "");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            var teste = "";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
