using ScheduleSim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.OleDb;
using System.Data;
using System.Threading;

namespace ScheduleSim.Access
{
    public class AccessConnectionFactory : IDbConnectionFactory
    {
        private Func<string> openPath;
        private IDbConnection transactionConn;
        private IDbTransaction transaction;
        private ManualResetEventSlim locker = new ManualResetEventSlim();

        public AccessConnectionFactory(Func<string> openPath)
        {
            this.openPath = openPath;
        }

        public IDbConnection Create()
        {
            var conn = null as IDbConnection;

            if (this.transactionConn == null)
            {
                var builder = new OleDbConnectionStringBuilder();
                builder.Provider = "Microsoft.ACE.OLEDB.12.0";
                builder.DataSource = this.openPath();

                conn =
                    new OleDbConnection(connectionString: builder.ConnectionString);
            }
            else
            {
                conn = this.transactionConn;
            }

            return conn;
        }
        
        public IDbTransaction BeginTransaction()
        {
            this.locker.Wait(3000);
            this.locker.Reset();
            this.transactionConn = Create();
            this.transaction = this.transactionConn.BeginTransaction();
            return this.transaction;
        }

        public void EndTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Dispose();
                this.transaction = null;
            }

            if (this.transactionConn != null)
            {
                this.transactionConn.Close();
                this.transactionConn = null;
            }
        }
    }
}
