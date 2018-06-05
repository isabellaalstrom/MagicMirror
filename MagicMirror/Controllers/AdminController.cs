using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Data;
using MagicMirror.Models;
using MagicMirror.Models.AdminModels;
using MagicMirror.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicMirror.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository _repo;
        private readonly IHassService _hassService;
        private readonly ICredentialsRepository _credentialsRepo;
        private readonly List<HassEntity> _entities;
        public AdminController(IRepository repo, IHassService hassService, ICredentialsRepository credentialsRepo)
        {
            _repo = repo;
            _hassService = hassService;
            _credentialsRepo = credentialsRepo;
            _entities = _repo.GetEntities();

        }
        public IActionResult Index()
        {
            var model = new Settings
            {
                EntitiesToTrack = _entities.Where(x => x.SelectedEntity).ToList()
            };
            return View(model);
        }

        public IActionResult SetCredentials()
        {
            var model = new SetCredentialsViewModel();
            var credentials = _credentialsRepo.GetCredentials();
            if (credentials != null)
            {
                model.HassPassword = credentials.HassPassword;
                model.HassBaseUrl = credentials.HassBaseUrl;
                model.DarkSkyApiKey = credentials.DarkSkyApiKey;
                model.SlRealTimeApiKey = credentials.SlRealTimeApiKey;
                model.MqttBroker = credentials.MqttBroker;
                model.MqttUsername = credentials.MqttUsername;
                model.MqttPassword = credentials.MqttPassword;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SetCredentials(SetCredentialsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var credentials = new Credentials
                {
                    HassBaseUrl = model.HassBaseUrl,
                    HassPassword = model.HassPassword,
                    DarkSkyApiKey = model.DarkSkyApiKey,
                    SlRealTimeApiKey = model.SlRealTimeApiKey,
                    MqttPassword = model.MqttPassword,
                    MqttBroker = model.MqttBroker,
                    MqttUsername = model.MqttUsername
                };
                _credentialsRepo.SetCredentials(credentials);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> ChooseEntities()
        {
            var model = _entities;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseEntities(IFormCollection collection)
        {
            var updatedEntities = _entities;
            var keys = collection.Keys.Where(x => x.StartsWith("hass-"));
            var enumerableKeys = keys.Select(key => key.Remove(0, 5)).ToList();

            foreach (var key in enumerableKeys)
            {
                var entityId = key;
                var entity = updatedEntities.FirstOrDefault(x => x.EntityId == entityId);
                if (entity != null)
                {
                    updatedEntities.Remove(updatedEntities.FirstOrDefault(x => x.EntityId == entityId));
                    entity.SelectedEntity = true;
                    updatedEntities.Add(entity);
                }
            }

            var entitiesToUpdate = new List<HassEntity>();
            foreach (var jsonEntity in _entities.Where(x => x.SelectedEntity))
            {
                var entityToUpdate = updatedEntities.FirstOrDefault(x => x.EntityId == jsonEntity.EntityId);
                if (entityToUpdate != null )
                {
                    if (!enumerableKeys.Contains(jsonEntity.EntityId))
                    {
                        entitiesToUpdate.Add(entityToUpdate);
                    }
                }
            }

            foreach (var hassEntity in entitiesToUpdate)
            {
                updatedEntities.Remove(hassEntity);
                hassEntity.SelectedEntity = false;
                updatedEntities.Add(hassEntity);
            }

            _repo.UpdateEntities(updatedEntities);
            return View(this._entities);
        }

        public IActionResult TrackedEntities(List<HassEntity> entities)
        {
            var model = entities;
            return View(model);
        }

        public void UpdateEntities()
        {
            _repo.LoadEntitiesFromHass();
        }
    }
}