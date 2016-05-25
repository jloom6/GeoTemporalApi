# GeoTemporalApi

## Design Decisions
I had originally debated doing this all in memory vs storing in a database. Doing this all in memory would elimiate any DB connection issues/slowdowns but would require an extremely efficient data structure to store all of the messages. Some research led me to [Po-Trees](http://liris.cnrs.fr/~sservign/SDH04%20Noel.pdf) which would be an optimal structure, however given that I could not find any implementations it did not prove feasible. Another potential structure was a [Point Quadtree](https://en.wikipedia.org/wiki/Quadtree#Point_quadtree) which is essentially a 2 dimensional [Binary Tree](https://en.wikipedia.org/wiki/Binary_tree). I was able to find an implementation in C# but my testing found it not to be as efficient as expected. It could have just been the specific implementation that was bad but at this point I had decided it was probably a better idea to let a database handle the heavy lifting.

I chose a MySQL database because I have a lot of experience with MySQL and although it is definitely not the most performant database, it should do the job and most importantly is free. A simple database design for this project only needed 3 tables:

**trips** - Stores each trip as it comes in, mostly used to guarantee trip messages are coming in the correct order.

Name|Type|Other Info
---|---|---
id|INT 11|Primary Index
begin_epoch|BIGINT 20| Key Index
end_epoch|BIGINT 20| Key Index, Nullable
fare|DECIMAL 10,2|Nullable
last_lat_long|POINT|Nullable

**trip_messages** - Stores each message, needs to be a MyISAM table to use the Spatial index which does not allow foreign keys so there are no db checks that a trip or trip_event exist (those checks are done in the code instead).

Name|Type|Other Info
---|---|---
event_id|INT 11|Key Index
trip_id|INT 11|Key Index, Unique Index with epoch
lat_long|POINT|Spatial Index
epoch|BIGINT 20|Unique Index with epoch
fare|DECIMAL 10,2|Allow Null

**trip_events** - Stores the 3 trip events begin, update, and end. Not actually used since the trip_messages table can't use the foreign key but kept just in case.

Name|Type|Other Info
---|---|---
id|INT 11|Primary Index, Auto Increment
trip_id|INT 11

I contemplated using 2 decimal fields for latitude and longitude instead of the point since this project only required querying rectangular areas, but decided on the point since the spatial indexing is actually quite fast and could be much better adapted to other shapes (such as looking in a radius around a point)
