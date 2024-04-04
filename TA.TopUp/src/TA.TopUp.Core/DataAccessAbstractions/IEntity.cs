using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Core.DataAccessAbstractions
{
    public interface IEntity<T> where T : struct
    {
        T Id { get; protected set; }

    }

    public interface IEntity : IEntity<Guid> { }
}
