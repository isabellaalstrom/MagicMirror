using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Models;

namespace MagicMirror.Services
{
    public interface IHassService
    {

        Task<HassEntity> GetEntityStateAsync(string entityId);
        Task<IEnumerable<HassEntity>> GetStatesAsync();
        Task<IEnumerable<HassEntity>> GetAllDoorEntitiesAsync();
    }
}
