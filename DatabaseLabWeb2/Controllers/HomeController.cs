using DatabaseLabWeb2.Controllers.Models;
using DatabaseLabWeb2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers
{
    public class HomeController : Controller
    {
        public static IMongoDatabase db;
        public static IMongoDatabase dbV1;
        public static IMongoDatabase dbV2;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            var client = new MongoClient("mongodb+srv://andlek1:andlek1@db1.dtfyj.mongodb.net/DB1?authSource=admin&replicaSet=atlas-jww9tl-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
            db = client.GetDatabase("DB1");
            dbV1 = client.GetDatabase("DB2");
            dbV2 = client.GetDatabase("DB3");
            var collection = db.GetCollection<Project>("projects");

            List<Project> projectCollection = collection.AsQueryable<Project>().ToList();
            return View(projectCollection);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("creation_date, project_name, contracted_amount," +
            " address, city, country, notes, current_amount, estimate_amount")] Project project)
        {

            if (ModelState.IsValid)
            {
                var collection = db.GetCollection<Project>("projects");
                collection.InsertOne(project);
                var collection2 = dbV1.GetCollection<ProjectV1>("projects");
                ProjectV1 projectV1 = new ProjectV1
                {
                    Id = project.Id,
                    creation_date = project.creation_date,
                    project_name = project.project_name,
                    contracted_amount = project.contracted_amount,
                    address = project.address,
                    city = project.city,
                    country = project.country
                };
                collection2.InsertOne(projectV1);
                var collection3 = dbV2.GetCollection<ProjectV2>("projects");
                ProjectV2 projectV2 = new ProjectV2
                {
                    Id = project.Id,
                    notes = project.notes,
                    current_amount = project.current_amount,
                    estimate_amount = project.estimate_amount,
                };
                collection3.InsertOne(projectV2);

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        public ActionResult Edit(string id)
        {
            var collection = db.GetCollection<Project>("projects");
            var Id = new ObjectId(id);
            var project = collection.AsQueryable<Project>().SingleOrDefault(x => x.Id == Id);
            return View(project);
        }
        [HttpPost]
        public ActionResult Edit(string id, Project project)
        {
            try
            {
                var filter = Builders<Project>.Filter.Eq("_id", ObjectId.Parse(id));
                var filterV1 = Builders<ProjectV1>.Filter.Eq("_id", ObjectId.Parse(id));
                var filterV2 = Builders<ProjectV2>.Filter.Eq("_id", ObjectId.Parse(id));

                var update = Builders<Project>.Update
                    .Set("creation_date", project.creation_date)
                    .Set("project_name", project.project_name)
                    .Set("contracted_amount", project.contracted_amount)
                    .Set("address", project.address)
                    .Set("city", project.city)
                    .Set("notes", project.notes)
                    .Set("current_amount", project.current_amount)
                    .Set("estimate_amount", project.estimate_amount)
                    .Set("potential_profit", project.potential_profit);

                var updateV1 = Builders<ProjectV1>.Update
                    .Set("creation_date", project.creation_date)
                    .Set("project_name", project.project_name)
                    .Set("contracted_amount", project.contracted_amount)
                    .Set("address", project.address)
                    .Set("city", project.city);

                var updateV2 = Builders<ProjectV2>.Update
                    .Set("notes", project.notes)
                    .Set("current_amount", project.current_amount)
                    .Set("estimate_amount", project.estimate_amount);

                var collection = db.GetCollection<Project>("projects");
                var collection2 = dbV1.GetCollection<ProjectV1>("projects");
                var collection3 = dbV2.GetCollection<ProjectV2>("projects");

                var result = collection.UpdateOne(filter, update);
                var result2 = collection2.UpdateOne(filterV1, updateV1);
                var result3 = collection3.UpdateOne(filterV2, updateV2);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Delete(string id)
        {
            var collection = db.GetCollection<Project>("projects");
            var Id = new ObjectId(id);
            var project = collection.AsQueryable<Project>().SingleOrDefault(x => x.Id == Id);
            return View(project);
        }

        [HttpPost]
        public ActionResult Delete(string id, IFormCollection c)
        {
            var collection = db.GetCollection<Project>("projects");
            var collection2 = dbV1.GetCollection<ProjectV1>("projects");
            var collection3 = dbV2.GetCollection<ProjectV2>("projects");

            try
            {
                collection.DeleteOne(Builders<Project>.Filter.Eq("_id", ObjectId.Parse(id)));
                collection2.DeleteOne(Builders<ProjectV1>.Filter.Eq("_id", ObjectId.Parse(id)));
                collection3.DeleteOne(Builders<ProjectV2>.Filter.Eq("_id", ObjectId.Parse(id)));
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
