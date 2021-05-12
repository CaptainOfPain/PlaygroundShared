using Newtonsoft.Json;
using JsonSerializer = RawRabbit.Serialization.JsonSerializer;

namespace PlaygroundShared.RabbitMq
{
    public class RawRabbitSerializer : JsonSerializer
    {
        public RawRabbitSerializer() : base(Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings 
        {
            TypeNameHandling = TypeNameHandling.None,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        }))
        {
            
        }
    }
}