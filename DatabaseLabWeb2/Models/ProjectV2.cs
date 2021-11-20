using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Models
{
    public class ProjectV2
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("owner_id")]
        public ObjectId owner_id { get; set; }
        [BsonElement("notes")]
        public string notes { get; set; }
        [BsonElement("current_amount")]
        public int current_amount { get; set; }
        [BsonElement("estimate_amount")]
        public int estimate_amount { get; set; }
    }
}
