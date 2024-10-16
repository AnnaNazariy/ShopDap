using Dapper;
using ShopDap.Entities;
using MySql.Data.MySqlClient;
using System.Data;
using ShopDap.Repositories.Interfaces;

namespace ShopDap.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction)
            : base(mySqlConnection, dbTransaction, "Products")
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            string sql = @"SELECT * FROM Products WHERE CategoryID = @CategoryId";
            var result = await _mySqlConnection.QueryAsync<Product>(sql,
                param: new { CategoryId = categoryId },
                transaction: _dbTransaction);
            return result;
        }
    }
}
