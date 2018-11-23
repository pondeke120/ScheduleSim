using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.BusinessLogics;
using ScheduleSim.Entities;
using System.Data;

namespace ScheduleSim.Core.Dispatcher
{
    public class TransactionDispatcher : IDispatcher
    {
        private IDbConnectionFactory dbConnectionFactory;

        public TransactionDispatcher(
            IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public TOut Dispatch<TIn, TOut>(IBusinessLogic<TIn, TOut> businessLogic, TIn input)
        {
            var ret = default(TOut);
            var transaction = null as IDbTransaction;
            try
            {
                transaction = this.dbConnectionFactory.BeginTransaction();
                ret = businessLogic.Execute(input);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                this.dbConnectionFactory.EndTransaction();
            }
            return ret;
        }
    }
}
