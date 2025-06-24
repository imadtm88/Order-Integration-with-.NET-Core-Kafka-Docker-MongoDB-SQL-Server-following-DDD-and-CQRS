using Domain.Model.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Model.History
{
    public class OrderHistoryStatus
    {
        [BsonRepresentation(BsonType.String)]
        public OrderStatus PreviousStatus { get; set; }

        [BsonRequired]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastUpdated { get; set; }

        public OrderHistoryStatus()
        {
            LastUpdated = DateTime.UtcNow;
        }
    }
}
