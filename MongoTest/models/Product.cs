using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MongoTest.models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        [BsonElement("Name")]       
        [MaxLength(100)]
        public string Name { get; set; }

        [BsonRequired]
        [MaxLength(150)]
        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonRequired]
        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonRequired]
        [BsonElement("Category")]
        public Category Category { get; set; }


        [BsonElement("Color")]
        public string Color { get; set; }
    }
}