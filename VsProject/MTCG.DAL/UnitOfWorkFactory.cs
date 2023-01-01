
namespace MTCG.DAL
{
    using System.Data;

    public class UnitOfWorkFactory
    {
        public UnitOfWorkFactory(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IUnitOfWork CreateAndBeginTransaction()
        {
            IUnitOfWork unitOfWork = new NpgSqlUnitOfWorkDatabase(dbConnection);
            unitOfWork.BeginTransaction();
            return unitOfWork;
        }

        private readonly IDbConnection dbConnection;
    }
}
