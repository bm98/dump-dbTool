Output folder - may contain any or all of the following files.
Depending on available input - output is created accordingly

 - <BUILD-DATETIME>
   // goes into folder sqldb dump1090fa FA repository
      \dump1090fa-aircrafts.sqb
      \dump1090fa-flights.sqb
      
   // goes into folder layers of the FA web repository
      \nav-ndb-region.geojson
      \nav-vordme-region.geojson
      \apt-midlarge-region.geojson
      \apt-others-region.geojson
      \navx-awy-fix-region.geojson
      \navx-awy-lo-region.geojson
      \navx-awy-hi-region.geojson
      
   // regular FA website databases (backup the original e.g. $ mv db db.backup)   
   // goes into folder html of the FA web repository $ cp -r 
      \db\Xn.json
      \db\README
      \db\aircraft_types\icao_aircraft_types.json
      \db\aircraft_types\README


