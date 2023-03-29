using Common.Settings;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;
        public UnitOfWork(IOptions<AppSettings> appSettings)
        {
            InitCustomColumn();

            _connection = new SqlConnection("Server=localhost;Database=test;Trusted_Connection=true;TrustServerCertificate=Yes;");
            _connection.Open();
            _transaction = _connection.BeginTransaction(); 
        }

        private void InitCustomColumn()
        {
            //Dapper.SqlMapper.SetTypeMap(typeof(Contacts), new ColumnAttributeTypeMapper<Contacts>());
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void resetRepositories()
        {

        }

        public void Commit()
        {
            try
            {
                _transaction!.Commit();
            }
            catch
            {
                _transaction!.Rollback();
                throw;
            }
            finally
            {
                _transaction!.Dispose();
                _transaction = _connection!.BeginTransaction();
                resetRepositories();
            }
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }
        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
