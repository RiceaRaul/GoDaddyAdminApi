using Common.Settings;
using Microsoft.Extensions.Options;
using System.Data;

namespace DataAccessLayer
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        public UnitOfWork(IOptions<AppSettings> appSettings)
        {
            InitCustomColumn();
            /*
            _connection = new SqlConnection("Data Source=91.92.136.222;Initial Catalog=CST;User ID=sa;Password=Relisys123;MultipleActiveResultSets=True");
            _connection.Open();
            _transaction = _connection.BeginTransaction(); */
        }

        private void InitCustomColumn()
        {
            //Dapper.SqlMapper.SetTypeMap(typeof(Contacts), new ColumnAttributeTypeMapper<Contacts>());
        }

        public void Dispose()
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
            GC.SuppressFinalize(this);
        }

        private void ResetRepositories()
        {
            throw new NotImplementedException();
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
                ResetRepositories();
            }
        }

       
        ~UnitOfWork()
        {
            Dispose();
        }
    }
}
