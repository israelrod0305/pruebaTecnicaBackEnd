using PruebaTecnica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.IPruebaTecnicaAppService
{
    public interface IRefreshTokeAppService
    {
        Task<ResponseModel<bool>> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate);
        Task<ResponseModel<bool>> ValidateRefreshToken(int userId, string refreshToken);
        Task<ResponseModel<bool>> DeleteRefreshToken(int userId, string refreshToken);
    }
}
