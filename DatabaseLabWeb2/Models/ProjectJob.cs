using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers.Models
{
    public class ProjectJob
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId WorkerId { get; set; }
        public string Job_name { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public int Contracted_amount { get; set; }
        public ObjectId Project_jobs_id { get; set; }
    }
}
