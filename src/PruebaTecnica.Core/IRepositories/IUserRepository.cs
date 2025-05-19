using Abp.Domain.Repositories;
using PruebaTecnica.Core;
using PruebaTecnica.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Core
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);

        // Método para validar usuario por username y password
        Task<User> ValidateUserAsync(string username, string password);

        // Opcionalmente, métodos para CRUD:
        Task<bool> CreateAsync(UserCreateUpdateDto user);
        Task<bool> UpdateUserAsync(UserCreateUpdateDto user);
        Task<bool> DeleteAsync(int id);
    }
}
