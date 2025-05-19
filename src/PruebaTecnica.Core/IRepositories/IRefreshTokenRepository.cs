
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Core
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<bool> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate);
        Task<bool> ValidateRefreshToken(int userId, string refreshToken);
        Task<RefreshToken> GetRefreshToken(int userId, string refreshToken);
        Task<bool> DeleteRefreshToken(int userId, string refreshToken);
    }
}
