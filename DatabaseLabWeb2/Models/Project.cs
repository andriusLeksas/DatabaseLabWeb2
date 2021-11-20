﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLabWeb2.Controllers.Models
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("creation_date")]
        public DateTime creation_date { get; set; }
        [BsonElement("project_name")]
        public string project_name { get; set; }
        [BsonElement("contracted_amount")]
        public int contracted_amount { get; set; }
        [BsonElement("address")]
        public string address { get; set; }
        [BsonElement("city")]
        public string city { get; set; }
        [BsonElement("country")]
        public string country { get; set; }
        [BsonElement("owner_id")]
        public ObjectId owner_id { get; set; }
        [BsonElement("notes")]
        public string notes { get; set; }
        [BsonElement("current_amount")]
        public int current_amount { get; set; }
        [BsonElement("estimate_amount")]
        public int estimate_amount { get; set; }
        [BsonElement("potential_profit")]
        public int potential_profit { get; set; }
    }
}
