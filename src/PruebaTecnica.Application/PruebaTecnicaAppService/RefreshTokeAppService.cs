using PruebaTecnica.Core;
using PruebaTecnica.Entities;
using PruebaTecnica.IPruebaTecnicaAppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.PruebaTecnicaAppService
{
    public class RefreshTokeAppService : PruebaTecnicaAppServiceBase, IRefreshTokeAppService
    {
        private IRefreshTokenRepository _refreshRepository;
        public RefreshTokeAppService(IRefreshTokenRepository refreshRepository) { 
            _refreshRepository = refreshRepository;
        }
        public async Task<ResponseModel<bool>> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate)
        {
            var validacion = await _refreshRepository.SaveRefreshToken(userId, refreshToken, expiryDate);
            ResponseModel<bool> data = new ResponseModel<bool>();
            data.Mensaje = "Se guardo correctamente el token";
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

        public async Task<ResponseModel<bool>> ValidateRefreshToken(int userId, string refreshToken)
        {
            var validacion = await _refreshRepository.ValidateRefreshToken(userId, refreshToken);
            ResponseModel<bool> data = new ResponseModel<bool>();
            data.Mensaje = "Se valido correctamente el token";
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

        public async Task<ResponseModel<bool>> DeleteRefreshToken(int userId, string refreshToken)
        {
            var validacion = await _refreshRepository.DeleteRefreshToken(userId, refreshToken);
            ResponseModel<bool> data = new ResponseModel<bool>();
            data.Mensaje = "Se elimino correctamente el token";
            data.Data = validacion;
            data.Codigo = 200;
            if (data == null)
            {
                return new ResponseModel<bool> { Codigo = 500, Mensaje = "Error al cerrar session", Data = false };
            }
            else
            {
                return data;
            }
        }

    }
}
