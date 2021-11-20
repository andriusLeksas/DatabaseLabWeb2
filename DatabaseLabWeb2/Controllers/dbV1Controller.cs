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
    public class dbV1Controller : Controller
    {
        private readonly ILogger<dbV1Controller> _logger;
        public dbV1Controller(ILogger<dbV1Controller> logger)
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
            MongoCRUD dbV1 = new MongoCRUD("DB2");
            var collection = db.GetCollection<ProjectV1>("projects");
            List<ProjectV1> projectCollection = collection.AsQueryable<ProjectV1>().ToList();
            return View(projectCollection);
        }
    }
}
