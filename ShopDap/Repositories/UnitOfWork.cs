using ShopDap.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

namespace ShopDap.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MySqlConnection _mySqlConnection;
        private IDbTransaction _dbTransaction;

        public IUserRepository UserRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IProductRepository ProductRepository { get; }

        public UnitOfWork(MySqlConnection mySqlConnection)
        {
            _mySqlConnection = mySqlConnection;
            _dbTransaction = _mySqlConnection.BeginTransaction();
            UserRepository = new UserRepository(_mySqlConnection, _dbTransaction);
            OrderRepository = new OrderRepository(_mySqlConnection, _dbTransaction);
            ProductRepository = new ProductRepository(_mySqlConnection, _dbTransaction);
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            _mySqlConnection?.Dispose();
        }
    }
}
