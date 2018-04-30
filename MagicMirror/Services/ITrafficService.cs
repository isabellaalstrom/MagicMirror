using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Models.TrafficModels;

namespace MagicMirror.Services
{
    public interface ITrafficService
    {
        Task<List<Transport>> GetRealTime(string siteId);
    }
}
