using DatabaseLabWeb2.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers
{
    public class dbHLController : Controller
    {
        private readonly ILogger<dbHLController> _logger;
        public dbHLController(ILogger<dbHLController> logger)
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
            MongoCRUD dbHL = new MongoCRUD("DB22");
            var collection = db.GetCollection<JobMaterial>("job_materials");
            List<JobMaterial> job_materialsCollection = collection.AsQueryable<JobMaterial>().ToList();
            return View(job_materialsCollection.Where(x=> x.quantity < 50).ToList());
        }
    }
}
