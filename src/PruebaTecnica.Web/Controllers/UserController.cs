using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Dto;
using PruebaTecnica.IPruebaTecnicaAppService;
using System.Threading.Tasks;

namespace PruebaTecnica.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class UserController : AbpController
    {
        private readonly IUserAppService _userAppService;
        public UserController(IUserAppService userAppService) {
            _userAppService = userAppService;
        }


        [HttpPost("CreateUser")]
       
       
        public async Task<IActionResult> CreateUser([FromBody] UserCreateUpdateDto user)
        {
            await _userAppService.CrearUsuario(user);
            if (user == null)
                return Unauthorized();

            
            return Ok(new
            {
                data = "Usuario Creado correctamente",
                message = "Ok"
            });
        }

        // Actualizar usuario
        [HttpPut("Update")]
       
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            var result = await _userAppService.ActualizarUsuario(user);
            return Ok(result);
        }

        // Eliminar usuario
        [HttpDelete("Delete/{id}")]
    
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userAppService.EliminarUsuario(id);
            return Ok(result);
        }

        // Listar todos los usuarios
        [HttpGet("All")]
     
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userAppService.ListarTodosUsuarios();
            return Ok(result);
        }

        [HttpGet("ById")]
  
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userAppService.ObtenerUsuarioPorId(id);
            return Ok(result);
        }
    }
}
