using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.TopUp.Core.Entities;
using TA.TopUp.Core.Interfaces.Repositories;
using TA.TopUp.Infrastructure.DataAccessAbstractions;

namespace TA.TopUp.Infrastructure.Repositories
{
    public class UserWalletBalancesRepository : GenericRepository<UserWalletBalance>, IUserWalletBalancesRepository
    {
        public UserWalletBalancesRepository(TopUpSystemDbContext context) :base(context) { }
    }
}
