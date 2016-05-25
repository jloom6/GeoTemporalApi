using System.Runtime.Serialization;

namespace GeoTemporalModels
{
    public enum QueryType
    {
        [EnumMember(Value = "throughGeoRect")]
        ThroughGeoRect,
        [EnumMember(Value = "startStopInGeoRect")]
        StartStopInGeoRect,
        [EnumMember(Value = "pointInTime")]
        PointInTime
    }
}
