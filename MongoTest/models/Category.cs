using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MongoTest.models
{
    [DataContract]
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DataMember]
        [BsonRequired]
        [BsonElement("Name")]
        public string Name { get; set; }

        [DataMember]
        [BsonRequired]
        [BsonElement("Description")]
        public string Description { get; set; }
    }
}
