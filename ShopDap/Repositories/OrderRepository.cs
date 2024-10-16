using Dapper;
using ShopDap.Entities;
using MySql.Data.MySqlClient;
using System.Data;
using ShopDap.Repositories.Interfaces;

namespace ShopDap.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction)
            : base(mySqlConnection, dbTransaction, "Orders")
        {
        }

        // Отримання замовлень за ID користувача
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            string sql = @"SELECT * FROM Orders WHERE UserID = @UserId";
            var result = await _mySqlConnection.QueryAsync<Order>(sql,
                param: new { UserId = userId },
                transaction: _dbTransaction);
            return result;
        }
    }
}
