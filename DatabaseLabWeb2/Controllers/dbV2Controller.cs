using DatabaseLabWeb2.Controllers.Models;
using DatabaseLabWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers
{
    public class dbV2Controller : Controller
    {
        private readonly ILogger<dbV2Controller> _logger;
        public dbV2Controller(ILogger<dbV2Controller> logger)
        {
            _logger = logger;
        }

        public static IMongoDatabase db;
        public class MongoCRUD
        {
            public MongoCRUD(string database)
            {
                var client = new MongoClient("mongodb+srv://andlek1:andlek1@db1.dtfyj.mongodb.net/DB1?authSource=admin&replicaSet=atlas-jww9tl-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
                db = client.GetDatabase(database);
            }
        }
        public IActionResult Index()
        {
            MongoCRUD dbV2 = new MongoCRUD("DB3");
            var collection = db.GetCollection<ProjectV2>("projects");
            List<ProjectV2> projectCollection = collection.AsQueryable<ProjectV2>().ToList();
            return View(projectCollection);
        }
      
    }
}
