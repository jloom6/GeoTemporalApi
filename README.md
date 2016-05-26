# GeoTemporalApi

## Design Decisions
I had originally debated doing this all in memory vs storing in a database. Doing this all in memory would elimiate any DB connection issues/slowdowns but would require an extremely efficient data structure to store all of the messages. Some research led me to [Po-Trees](http://liris.cnrs.fr/~sservign/SDH04%20Noel.pdf) which would be an optimal structure, however given that I could not find any implementations it did not prove feasible. Another potential structure was a [Point Quadtree](https://en.wikipedia.org/wiki/Quadtree#Point_quadtree) which is essentially a 2 dimensional [Binary Tree](https://en.wikipedia.org/wiki/Binary_tree). I was able to find an implementation in C# but my testing found it not to be as efficient as expected. It could have just been the specific implementation that was bad but at this point I had decided it was probably a better idea to let a database handle the heavy lifting.

###The Database

I chose a MySQL database because I have a lot of experience with MySQL and although it is definitely not the most performant database, it should do the job and most importantly is free. Postgres would have probably been a better choice but I stuck with what I know. A simple database design for this project only needed 3 tables:

**trips** - Stores each trip as it comes in, mostly used to guarantee trip messages are coming in the correct order.

Name|Type|Other Info
---|---|---
id|INT 11|Primary Key
begin_epoch|BIGINT 20|Index
end_epoch|BIGINT 20|Index, Nullable
fare|DECIMAL 10,2|Nullable
last_lat_long|POINT|Nullable

**trip_messages** - Stores each message, needs to be a MyISAM table to use the Spatial index which does not allow foreign keys so there are no db checks that a trip or trip_event exist (those checks are done in the code instead).

Name|Type|Other Info
---|---|---
event_id|INT 11|Index
trip_id|INT 11|Index, Unique Index with epoch
lat_long|POINT|Spatial Index
epoch|BIGINT 20|Unique Index with epoch
fare|DECIMAL 10,2|Allow Null

**trip_events** - Stores the 3 trip events begin, update, and end. Not actually used since the trip_messages table can't use the foreign key but kept just in case.

Name|Type|Other Info
---|---|---
id|INT 11|Primary Key, Auto Increment
name|VARCHAR 10

I contemplated using 2 decimal fields for latitude and longitude instead of the point since this project only required querying rectangular areas, but decided on the point since the spatial indexing is actually quite fast and could be much better adapted to other shapes (such as looking in a radius around a point).

The stored procedures and table generating scripts can be found in the sql folder.

###The Codebase
The codebase is organized into 5 projects, that may seem like a lot for this small project but it's well organized by function.

Project|Purpose|Dependencies
---|---|---
GeoTemporalApi|Handles all API interactions, turns the web requests into business requests|GeoTemporalBl, GeoTemporalModels
GeoTemporalBl|The Business Layer, does the business logic|GeoTemporalDal, GeoTemporalModels
GeoTemporalDal|The Data Access Layer, handles all database logic|GeoTemporalModel
GeoTemporalModels|The models
GeoTemporalTests|The tests|GeoTemporalBl, GeoTemporalModels

###The API
The API has 4 Enpoints:

Endpoint|HTTP Method|Purpose
---|---|---
*/api/v1/tripMessages*|POST|Publish new message
*/api/v1/trips/:tripId*|GET|Get the trip with the requested tripId
*/api/v1/trips*|GET|Get the results of the requested query, supplied as query parameters
*/api/v1/trips*|DELETE|Truncate all of the trips currently in the database

####Example calls
GET */api/v1/trips/432*

```
{
   "id": 432,
   "beginEpoch": 1392864673030,
   "endEppoch": 1392864673070,
   "fare": 25,
   "lastMessageEpoch": 1392864673070,
   "lastLatitude": 37.79947,
   "lastLongitude": 122.511635,
   "messages": [
      {
         "event": "begin",
         "tripId": 432,
         "lat": 37.79947,
         "lng": 122.511635,
         "fare": null,
         "epoch": 1392864673030
      },
      {
         "event": "update",
         "tripId": 432,
         "lat": 37.79947,
         "lng": 122.511635,
         "fare": null,
         "epoch": 1392864673040
      },
      {
         "event": "end",
         "tripId": 432,
         "lat": 37.79947,
         "lng": 122.511635,
         "fare": 25,
         "epoch": 1392864673070
      }
   ]
}
```

GET */api/v1/trips?type=PointInTime&epoch=1392864673030*

Gets the number of trips occuring at a given point in time

Parameter|Explanation
---|---
type|The query type, PointInTime for this request
epoch|The epoch to query for, an integer greater than 0

```
{
   "queryType": "PointInTime",
   "epoch": 1392864673030,
   "numberOfTrips": 1
}
```

GET */api/v1/trips?type=StartStopInGeoRect&bottomLeftLatitude=37&bottomLeftLongitude=122&topRightLatitude=38&topRightLongitude=123*

Gets the number of trips that start or stop in a geo rectangle and the sum of their fares

Parameter|Explanation
---|---
type|The query type, StartStopInGeoRect for this request
bottomLeftLatitude|The latitude of the bottom left corner, a decimal between -90 and 90
bottomLeftLongitude|The longitude of the bottom left corner, a decimal between -180 and 180
topRightLatitude|The latitude of the top right corner, a decimal between -90 and 90
topRightLongitude|The longitude of the top right corner, a decimal between -180 and 180

```
{
   "queryType": "StartStopInGeoRect",
   "geoRectangle": {
      "bottomLeft": {
         "latitude": 37,
         "longitude": 122
      },
      "topRight": {
         "latitude": 38,
         "longitude": 123
      }
   },
   "totalTrips": 1,
   "totalFares": 25
}
```

GET */api/v1/trips?type=StartStopInGeoRect&bottomLeftLatitude=37&bottomLeftLongitude=122&topRightLatitude=38&topRightLongitude=123*

Gets the number of trips that start or stop in a geo rectangle and the sum of their fares

Parameter|Explanation
---|---
type|The query type, ThroughGeoRect for this request
bottomLeftLatitude|The latitude of the bottom left corner, a decimal between -90 and 90
bottomLeftLongitude|The longitude of the bottom left corner, a decimal between -180 and 180
topRightLatitude|The latitude of the top right corner, a decimal between -90 and 90
topRightLongitude|The longitude of the top right corner, a decimal between -180 and 180

```
{
   "queryType": "ThroughGeoRect",
   "geoRectangle": {
      "bottomLeft": {
         "latitude": 37,
         "longitude": 122
      },
      "topRight": {
         "latitude": 38,
         "longitude": 123
      }
   },
   "totalTrips": 1
}
```

**All Params are required**

###The Tests

The tests are split up into 6 classes:

Name|Purpose
---|---
GetTripPointInTimeQueryTests|Tests the happy paths for the PointInTime queries
GetTripQueryExceptionTests|Tests the unhappy paths for all queries
GetTripStartOrStopQueryTests|Tests the happy paths for the StartStopInGeoRect queries
GetTripThroughGeoRectQueryTests|Tests the happy paths for ThroughGeoRect queries
InsertTripMessageExceptionTests|Tests the unhappy paths for inserting trip messages
InsertTripMessageTests|Tests the happy paths for inserting trip messages

##Fault Tolerance
The API itself is not very fault tolerant, if it goes down while processing then the message will not be saved. However the client would recieve a 500 error (or not recieve the 201 created response) and should know that is the case. The only other moving part is the database. If that goes down then the message will not be saved, however again a non 201 code would be the response and the client should know that the message was not saved. If the database can not be recovered then we would be relying on any backups to use.

##Scaling considerations
The system is set up for easy scaling, both vertically and horizontally the API server could be made larger or more instances could be put on the load balancer with no changes needed.

The database instance could be made larger or set up with master/slaves with only changes needed to the database. If MySQL is not performing well enough a better database such as Postgres, MSSQL, Oracle, etc. could be used with minimal change to the code base (just changing the database layer to use the new database, the stored procedures calls can most likely stay the same).
