using PruebaTecnica.Core;
using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using PruebaTecnica.IPruebaTecnicaAppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;


namespace PruebaTecnica.PruebaTecnicaAppService
{
    public class UserAppService: PruebaTecnicaAppServiceBase ,IUserAppService
    {
        private IUserRepository _iuserRepository;
        public UserAppService(IUserRepository iuserRepository)
        {
            _iuserRepository = iuserRepository;
        }
        public async Task<ResponseModel<bool>> CrearUsuario(UserCreateUpdateDto usuario)
        {
            var validacion = await _iuserRepository.CreateAsync(usuario);
            ResponseModel<bool> data = new ResponseModel<bool>();
            data.Mensaje = "Usuario Creado Correctamente";
            data.Data = validacion;
            data.Codigo = 200;
            if (data == null)
            {
                return new ResponseModel<bool> { Codigo = 500, Mensaje = "Error al inicial session", Data = false };
            }
            else
            {
                return data;
            }
        }

        public async Task<ResponseModel<bool>> ActualizarUsuario(UserDto usuario)
        {
            
            var userModel = new UserCreateUpdateDto
            {
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Password = usuario.Password,
                Id = usuario.Id,
                ChangePassword = usuario.ChangePassword
            };
            var validacion = await _iuserRepository.UpdateUserAsync(userModel);
            if (!validacion)
            {
                return new ResponseModel<bool>
                {
                    Codigo = 404,
                    Mensaje = "Usuario no actualizado ",
                    Data = false
                };
            }
            if (usuario == null)
            {
                return new ResponseModel<bool>
                {
                    Codigo = 404,
                    Mensaje = "Usuario no actualizado encontrado",
                    Data = false
                };
            }

            return new ResponseModel<bool>
            {
                Codigo = 200,
                Mensaje = "Usuario actualizado encontrado",
                Data = true
            };

        }

        public async Task<ResponseModel<List<UserDto>>> ListarTodosUsuarios()
        {
            var usuarios = await _iuserRepository.GetAllAsync();

            // Mapeo manual de entidad User a UserDto
            var usuarioDtos = usuarios.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Nombre = u.Nombre,
                Apellido = u.Apellido
            }).ToList();

            return new ResponseModel<List<UserDto>>
            {
                Codigo = 200,
                Mensaje = "Usuarios obtenidos correctamente",
                Data = usuarioDtos
            };
        }

        public async Task<ResponseModel<UserDto>> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _iuserRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return new ResponseModel<UserDto>
                {
                    Codigo = 404,
                    Mensaje = "Usuario no encontrado",
                    Data = null
                };
            }

            var userDto = new UserDto
            {
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Id = usuario.Id
            };

            return new ResponseModel<UserDto>
            {
                Codigo = 200,
                Mensaje = "Usuario encontrado",
                Data = userDto
            };
        }
        public async Task<ResponseModel<bool>> EliminarUsuario(int id)
        {
            var result = await _iuserRepository.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Codigo = result ? 200 : 500,
                Mensaje = result ? "Usuario eliminado correctamente" : "Error al eliminar usuario",
                Data = result
            };
        }

        public async Task<ResponseModel<UserDto>> ValiateUserAsync(String usuario, String password)
        {
            var validacion = await _iuserRepository.ValidateUserAsync(usuario, password);
            if (validacion == null)
            {
                return new ResponseModel<UserDto>
                {
                    Codigo = 500,
                    Mensaje = "Error al iniciar sesión",
                    Data = null
                };
            }

            var userDto = new UserDto
            {
                Username = validacion.Username,
                Nombre = validacion.Nombre,
                Apellido = validacion.Apellido,
                Id = validacion.Id
            };
            return new ResponseModel<UserDto>
            {
                Codigo = 200,
                Mensaje = "Usuario Validado Correctamente",
                Data = userDto
            };
            
        }

    }
}
