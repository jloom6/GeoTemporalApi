using System.Runtime.Serialization;

namespace GeoTemporalModels
{
    public enum TripEvent
    {
        [EnumMember(Value = "begin")]
        Begin = 1,
        [EnumMember(Value = "update")]
        Update = 2,
        [EnumMember(Value = "end")]
        End = 3
    }
}
