using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Data
{
    public interface IRepository
    {
        List<string> GetEntities();
    }
}
