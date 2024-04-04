using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Core.Interfaces.Repositories;
using TA.TopUp.Core.Interfaces.Services;

namespace TA.TopUp.Infrastructure.DataAccessAbstractions
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync(CancellationToken ct = default);
        IBeneficiaryRepository BeneficiaryRepository { get; }
        ITopUpOptionsRepository TopUpOptionsRepository { get; }
        IUserTransactionsRepository UserTransactionsRepository { get; }
        IUserWalletBalancesRepository UserWalletBalancesRepository { get; }

    }
}
