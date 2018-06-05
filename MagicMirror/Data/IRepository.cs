using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Models;

namespace MagicMirror.Data
{
    public interface IRepository
    {
        //Dictionary<string, string> GetEntitiesAsStringList();
        List<HassEntity> GetEntities();
        HassEntity GetEntity(string entityId);
        List<HassEntity> GetSelectedEntities();
        void UpdateEntities(List<HassEntity> entitiesList);

        void LoadEntitiesFromHass();
    }
}
