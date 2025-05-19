using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core;
using PruebaTecnica.Repositories;
using System;
using System.Threading.Tasks;

namespace PruebaTecnica.EntityFrameworkCore
{
    public class RefreshTokenRepository : PruebaTecnicaRepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IDbContextProvider<PruebaTecnicaDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate)
        {
            var context = (PruebaTecnicaDbContext)await GetDbContextAsync();


            var existingToken = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpiryDate = expiryDate;
                context.RefreshTokens.Update(existingToken);
            }
            else
            {
                var newToken = new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken,
                    ExpiryDate = expiryDate
                };
                await context.RefreshTokens.AddAsync(newToken);
            }

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateRefreshToken(int userId, string refreshToken)
        {
            var context = (PruebaTecnicaDbContext)await GetDbContextAsync();


            var token = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken);

            return token != null && token.ExpiryDate > DateTime.UtcNow;
        }

        public async Task<RefreshToken> GetRefreshToken(int userId, string refreshToken)
        {
            var context = (PruebaTecnicaDbContext)await GetDbContextAsync();


            return await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken);
        }

        public async Task<bool> DeleteRefreshToken(int userId, string refreshToken)
        {
            var context = (PruebaTecnicaDbContext)await GetDbContextAsync();

            
            var token = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken);

            if (token != null)
            {
                context.RefreshTokens.Remove(token);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }
}
