using DatabaseLabWeb2.Controllers.Models;
using DatabaseLabWeb2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers
{
    public class Home2Controller : Controller
    {
        public static IMongoDatabase db;
        public static IMongoDatabase dbHL;
        public static IMongoDatabase dbHu;

        private readonly ILogger<Home2Controller> _logger;

        public Home2Controller(ILogger<Home2Controller> logger)
        {
            _logger = logger;
        }

        //public class MongoCRUD
        //{
        //    public MongoCRUD(string database)
        //    {
        //        var client = new MongoClient("mongodb+srv://andlek1:andlek1@db1.dtfyj.mongodb.net/DB1?authSource=admin&replicaSet=atlas-jww9tl-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
        //        db = client.GetDatabase(database);
        //    }
        //}
        public IActionResult Index()
        {
            var client = new MongoClient("mongodb+srv://andlek1:andlek1@db1.dtfyj.mongodb.net/DB1?authSource=admin&replicaSet=atlas-jww9tl-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
            db = client.GetDatabase("DB1");
            dbHL = client.GetDatabase("DB22");
            dbHu = client.GetDatabase("DB33");
            var jobMaterialCollection = db.GetCollection<JobMaterial>("job_materials");

            List<JobMaterial> c = jobMaterialCollection.AsQueryable<JobMaterial>().ToList();
            return View(c);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("material_name, vendor_name, quantity, amount_paid")] JobMaterial jobMaterial)
        {

            if (ModelState.IsValid)
            {
                var collection = db.GetCollection<JobMaterial>("job_materials");
                collection.InsertOne(jobMaterial);
                var collectionHu = dbHu.GetCollection<JobMaterial>("job_materials");
                var collectionHl = dbHL.GetCollection<JobMaterial>("job_materials");

                if (jobMaterial.quantity < 50)
                {
                    collectionHl.InsertOne(jobMaterial);
                }
                else
                {
                    collectionHu.InsertOne(jobMaterial);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(jobMaterial);
        }
        public ActionResult Edit(string id)
        {
            var collection = db.GetCollection<JobMaterial>("job_materials");
            var Id = new ObjectId(id);
            var jobMaterial = collection.AsQueryable<JobMaterial>().SingleOrDefault(x => x.Id == Id);
            return View(jobMaterial);
        }
        [HttpPost]
        public ActionResult Edit(string id, JobMaterial jobMaterial)
        {
            try
            {
                var filter = Builders<JobMaterial>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<JobMaterial>.Update
                    .Set("material_name", jobMaterial.material_name)
                    .Set("vendor_name", jobMaterial.vendor_name)
                    .Set("quantity", jobMaterial.quantity)
                    .Set("amount_paid", jobMaterial.amount_paid);
                var collection = db.GetCollection<JobMaterial>("job_materials");
                var collection2 = dbHL.GetCollection<JobMaterial>("job_materials");
                var collection3 = dbHu.GetCollection<JobMaterial>("job_materials");

                var Id = new ObjectId(id);
                var jobMaterialOld = collection.AsQueryable<JobMaterial>().SingleOrDefault(x => x.Id == Id);

                if (jobMaterialOld.quantity < 50)
                {
                    var result2 = collection2.UpdateOne(filter, update);
                    if(jobMaterial.quantity >= 50)
                    {
                        collection2.DeleteOne(filter);
                        collection3.InsertOne(jobMaterial);
                    }
                }
                else
                {                  
                    var result3 = collection3.UpdateOne(filter, update);
                    if (jobMaterial.quantity < 50)
                    {
                        collection3.DeleteOne(filter);
                        collection2.InsertOne(jobMaterial);
                    }
                }
                var result = collection.UpdateOne(filter, update);              

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }     

        }

        public ActionResult Delete(string id)
        {
            var collection = db.GetCollection<JobMaterial>("job_materials");
            var Id = new ObjectId(id);
            var jobMaterial = collection.AsQueryable<JobMaterial>().SingleOrDefault(x => x.Id == Id);
            return View(jobMaterial);
        }

        [HttpPost]
        public ActionResult Delete(string id, IFormCollection c)
        {
            var collection = db.GetCollection<JobMaterial>("job_materials");
            var collectionHu = dbHu.GetCollection<JobMaterial>("job_materials");
            var collectionHL = dbHL.GetCollection<JobMaterial>("job_materials");

            var Id = new ObjectId(id);
            var jobMaterial = collection.AsQueryable<JobMaterial>().SingleOrDefault(x => x.Id == Id);


            try
            {
                if(jobMaterial.quantity < 50)
                {
                    collectionHL.DeleteOne(Builders<JobMaterial>.Filter.Eq("_id", ObjectId.Parse(id)));
                }
                else
                {
                    collectionHu.DeleteOne(Builders<JobMaterial>.Filter.Eq("_id", ObjectId.Parse(id)));
                }
                collection.DeleteOne(Builders<JobMaterial>.Filter.Eq("_id", ObjectId.Parse(id)));
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
