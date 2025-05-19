using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Core;
using PruebaTecnica.Dto;
using PruebaTecnica.Repositories;
using System;
using System.Threading.Tasks;

namespace PruebaTecnica.EntityFrameworkCore
{
    public class UserRepository : PruebaTecnicaRepositoryBase<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            IDbContextProvider<PruebaTecnicaDbContext> dbContextProvider,
            ILogger<UserRepository> logger
        ) : base(dbContextProvider) {
            _logger = logger;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            _logger.LogDebug($"Buscando usuario por ID: {id}");
            var context = (PruebaTecnicaDbContext) await GetDbContextAsync();
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            _logger.LogInformation($"Validando existencia de usuario: {username}");
            var context = (PruebaTecnicaDbContext) await GetDbContextAsync();
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            _logger.LogInformation($"Validando credenciales para: {username}");
            var context = (PruebaTecnicaDbContext) await GetDbContextAsync();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning($"Usuario no encontrado: {username}");
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password); 
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Contraseña incorrecta para: {username}");
                return null;
            }
            return user;
        }

        public async Task<bool> CreateAsync(UserCreateUpdateDto user)
        {
            try
            {
                var context = (PruebaTecnicaDbContext) await GetDbContextAsync();

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
                _logger.LogInformation($"Creando usuario: {user.Username}");

                var userData = new User
                {
                    Username = user.Username,
                    Password = passwordHash, 
                    Nombre = user.Nombre,
                    Apellido = user.Apellido
                };

                await context.Users.AddAsync(userData);
                await context.SaveChangesAsync();

                _logger.LogInformation($"Usuario creado. ID: {userData.Id}, Username: {userData.Username}");
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error al crear usuario: {user.Username}");
                throw new Exception("Error de base de datos (consulta logs)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado creando usuario: {user.Username}");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(UserCreateUpdateDto user)
        {
            try
            {
                var context = (PruebaTecnicaDbContext) await GetDbContextAsync();

                var findUser = await context.Users.FindAsync(user.Id);
                if (findUser == null) return false;

                // Actualiza solo los campos que quieres cambiar
                findUser.Username = user.Username;

                // Si el password no es nulo o vacío, actualízalo (hasheado)
                if (!string.IsNullOrWhiteSpace(user.Password) && user.ChangePassword.GetValueOrDefault())
                {
                    findUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
                }

                findUser.Nombre = user.Nombre;
                findUser.Apellido = user.Apellido;

                context.Users.Update(findUser);
                await context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error al actualizar usuario: {user.Username}");
                throw new Exception("Error de base de datos (consulta logs)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado actualizando usuario: {user.Username}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando usuario ID: {id}");
                var context = (PruebaTecnicaDbContext) await GetDbContextAsync();

                var user = await context.Users.FindAsync(id);
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando usuario ID: {id}");
                throw;
            }
        }
    }
}
