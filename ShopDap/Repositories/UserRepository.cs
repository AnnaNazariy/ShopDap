using Dapper;
using ShopDap.Entities;
using MySql.Data.MySqlClient;
using System.Data;
using ShopDap.Repositories.Interfaces;

namespace ShopDap.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction)
            : base(mySqlConnection, dbTransaction, "Users")
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            string sql = @"SELECT * FROM Users WHERE Email = @Email";
            var result = await _mySqlConnection.QuerySingleOrDefaultAsync<User>(sql,
                param: new { Email = email },
                transaction: _dbTransaction);
            return result;
        }
    }
}
