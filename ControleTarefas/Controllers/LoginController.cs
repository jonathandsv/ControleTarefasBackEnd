using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleTarefas.Models;
using Microsoft.Extensions.Configuration;
using ControleTarefas.Data;
using ControleTarefas.Utils;

namespace ControleTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        [HttpPost]
        [Route("Logar")]
        public IActionResult Logar(Usuario usuario)
        {
            bool validaUsuarioParaLogin = new Util().ValidaUsuarioLogin(usuario);

            try
            {
                if (validaUsuarioParaLogin)
                {
                    var usuarioBO = new UsuarioBO(_configuration.GetConnectionString("DefaultConnection")).GetUsuario(usuario);

                    if (usuarioBO.Nome == usuario.Nome && usuarioBO.Senha != usuario.Senha)
                    {
                        return BadRequest("Senha Incorreta!");
                    }
                    else if (usuarioBO.Nome != usuario.Nome && usuarioBO.Senha == usuario.Senha)
                    {
                        return BadRequest("Nome do UsuariO incorreto!");
                    }
                    else if (usuarioBO.Nome == usuario.Nome && usuarioBO.Senha == usuario.Senha)
                    {
                        return Ok(usuarioBO);
                    }
                    else
                        return BadRequest("Usuario e Senha incorretos!");
                }
                else
                    BadRequest("Usuario ou Senha em Branco!");
                
            }
            catch (Exception ex)
            {

                return BadRequest("Problema ao Logar Usuário!");
            }
            return null;
        }
    }
}