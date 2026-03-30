using System;
using System.Transactions;

namespace SBS.IT.Utilities.Shared.UtilityExtension
{
    public sealed class TransactionContext : IDisposable
    {
        private readonly TransactionScope transactionScope;
        private readonly Transaction transactionToUse;
        private readonly TransactionScopeOption transactionScopeOption;
        private readonly TimeSpan transactionTimeout;
        private readonly TransactionOptions transactionOptions;
        public TransactionContext()
        {
            transactionScopeOption = TransactionScopeOption.Required;
            transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.MaxValue };
            transactionScope = new TransactionScope(transactionScopeOption, transactionOptions);
        }
        public TransactionContext(Transaction TransactionToUse)
        {
            transactionToUse = TransactionToUse;
            transactionScope = new TransactionScope(transactionToUse);
        }
        public TransactionContext(TransactionScopeOption TransactionScopeOption)
        {
            transactionScopeOption = TransactionScopeOption;
            transactionScope = new TransactionScope(transactionScopeOption);
        }
        public TransactionContext(Transaction TransactionToUse, TimeSpan TransactionTimeout)
        {
            transactionToUse = TransactionToUse;
            transactionTimeout = TransactionTimeout;
            transactionScope = new TransactionScope(transactionToUse, transactionTimeout);
        }
        public TransactionContext(TransactionScopeOption TransactionScopeOption, TimeSpan TransactionTimeout)
        {
            transactionScopeOption = TransactionScopeOption;
            transactionTimeout = TransactionTimeout;
            transactionScope = new TransactionScope(transactionScopeOption, transactionTimeout);
        }
        public TransactionContext(TransactionScopeOption TransactionScopeOption, TransactionOptions TransactionOptions)
        {
            transactionScopeOption = TransactionScopeOption;
            transactionOptions = TransactionOptions;
            transactionScope = new TransactionScope(transactionScopeOption, transactionOptions);
        }
        public void Complete()
        {
            if (transactionScope != null)
            {
                transactionScope.Complete();
            }
        }
        public void Dispose()
        {
            if (transactionScope != null)
            {
                transactionScope.Dispose();
            }
        }
    }
}
