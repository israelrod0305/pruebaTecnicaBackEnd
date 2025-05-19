using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.IPruebaTecnicaAppService
{
    public interface IUserAppService
    {
        Task<ResponseModel<bool>> CrearUsuario(UserCreateUpdateDto usuario);

        Task<ResponseModel<UserDto>> ValiateUserAsync(String usuario, String password);

       
        Task<ResponseModel<bool>> ActualizarUsuario(UserDto usuario);
        Task<ResponseModel<List<UserDto>>> ListarTodosUsuarios();
        Task<ResponseModel<UserDto>> ObtenerUsuarioPorId(int id);
        Task<ResponseModel<bool>> EliminarUsuario(int id);

    }
}
