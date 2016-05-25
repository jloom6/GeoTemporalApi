using System;

namespace GeoTemporalModels
{
    public class InvalidGeoPointException : Exception
    {
        public InvalidGeoPointException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class InvalidGeoRectangleException : Exception
    {
        public InvalidGeoRectangleException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class InvalidTripMessageException : Exception
    {
        public InvalidTripMessageException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class TripAlreadyExistsException : Exception
    {
        public TripAlreadyExistsException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class TripDoesNotExistException : Exception
    {
        public TripDoesNotExistException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class TripAlreadyHasMessageAfterEpochException : Exception
    {
        public TripAlreadyHasMessageAfterEpochException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class TripAlreadyEndedException : Exception
    {
        public TripAlreadyEndedException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class QueryException : Exception
    {
        public QueryException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
