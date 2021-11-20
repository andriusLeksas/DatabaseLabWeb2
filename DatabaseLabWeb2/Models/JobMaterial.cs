using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers.Models
{
    public class JobMaterial
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string material_name { get; set; }
        public string vendor_name { get; set; }
        public int quantity { get; set; }
        public int amount_paid { get; set; }
        public ObjectId project_jobs_id { get; set; }
    }
}
