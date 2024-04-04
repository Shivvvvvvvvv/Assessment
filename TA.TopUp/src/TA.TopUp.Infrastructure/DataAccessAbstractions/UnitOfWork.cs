using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Core.Interfaces.Repositories;
using TA.TopUp.Core.Interfaces.Services;
using TA.TopUp.Infrastructure.Repositories;

namespace TA.TopUp.Infrastructure.DataAccessAbstractions
{
    public class UnitOfWork : IUnitOfWork
    {
        private IBeneficiaryRepository _beneficiaryRepository;
        private ITopUpOptionsRepository _topupOptionsRepository;
        private IUserTransactionsRepository _userTransactionsRepository;
        private IUserWalletBalancesRepository _userWalletBalancesRepository;
        private IBeneficiaryService _beneficiaryService;

        private readonly TopUpSystemDbContext _context;

        public UnitOfWork(TopUpSystemDbContext context)
        {
            _context = context;
        }

        public IBeneficiaryRepository BeneficiaryRepository => _beneficiaryRepository ?? new BeneficiaryRepository(_context);

        public ITopUpOptionsRepository TopUpOptionsRepository => _topupOptionsRepository ?? new TopUpOptionsRepository(_context);

        public IUserTransactionsRepository UserTransactionsRepository => _userTransactionsRepository ?? new UserTransactionsRepository(_context);

        public IUserWalletBalancesRepository UserWalletBalancesRepository => _userWalletBalancesRepository ?? new UserWalletBalancesRepository(_context);

        public async Task<bool> SaveEntitiesAsync(CancellationToken ct = default)
        {
            try
            {
                await _context.SaveChangesAsync(ct);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
