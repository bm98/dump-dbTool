using System;
using System.Data.SQLite;
using System.IO;
using d1090dataLib.d1090ext_aplib;
using d1090dataLib.d1090ext_rtlib;

namespace d1090dataLib.d1090ext_flightsDB
{
  /// <summary>
  /// Create the dump1090fa-flights.sqb database file
  /// from the route and airports Database
  /// </summary>
  public class FlightsDB
  {
    /*
        CREATE TABLE "airports" (
            "apt_icao_code" TEXT NOT NULL,
            "apt_iata_code" TEXT,
            "iso_country" TEXT,
            "iso_region" TEXT,
            "lat" TEXT,
            "lon" TEXT,
            "elevation" TEXT,
            "apt_type" TEXT,
            "apt_name" TEXT
        );

        CREATE TABLE "routes" (
            "flight_code" TEXT NOT NULL,
            "from_apt_icao" TEXT,
            "to_apt_icao" TEXT
        );
        CREATE UNIQUE INDEX "i_routes_flight" on routes (flight_code ASC);
        CREATE UNIQUE INDEX "i_airports_icao" on airports (apt_icao_code ASC);

        CREATE VIEW "v_route_icao" AS SELECT
         flight_code AS flight,
         from_apt_icao AS from_apt,
         to_apt_icao AS to_apt
        FROM
         routes;

         CREATE VIEW "v_route_iata" AS SELECT
         flight_code AS flight,
         fapt.apt_iata_code AS from_apt,
         tapt.apt_iata_code AS to_apt
        FROM
         routes
        INNER JOIN airports fapt ON fapt.apt_icao_code = routes.from_apt_icao
        INNER JOIN airports tapt ON tapt.apt_icao_code = routes.to_apt_icao;

         CREATE VIEW "v_named_route" AS SELECT
         flight_code AS flight,
 
         fapt.apt_icao_code AS from_apt_ic,
         fapt.apt_iata_code AS from_apt_ia,
         fapt.apt_name AS from_apt_name,
 
         tapt.apt_icao_code AS to_apt_ic,
         tapt.apt_iata_code AS to_apt_ia,
         tapt.apt_name AS to_apt_name
        FROM
         routes
        INNER JOIN airports fapt ON fapt.apt_icao_code = routes.from_apt_icao
        INNER JOIN airports tapt ON tapt.apt_icao_code = routes.to_apt_icao;
     */
    private string CmdCreateTable =
        $"CREATE TABLE \"airports\" (\"apt_icao_code\" TEXT NOT NULL,\"apt_iata_code\" TEXT,\"iso_country\" TEXT,\"iso_region\" TEXT,"
      + "\"lat\" TEXT,\"lon\" TEXT,\"elevation\" TEXT,\"apt_type\" TEXT,\"apt_name\" TEXT);"
      + $"CREATE TABLE \"routes\" (\"flight_code\" TEXT NOT NULL,\"from_apt_icao\" TEXT,\"to_apt_icao\" TEXT);";

    private string CmdCreateIndex =
        $"CREATE UNIQUE INDEX \"i_routes_flight\" on routes (flight_code ASC);"
      + $"CREATE UNIQUE INDEX \"i_airports_icao\" on airports ( apt_icao_code ASC );";

    private string CmdCreateView =
        $"CREATE VIEW \"v_route_icao\" AS SELECT flight_code AS flight, from_apt_icao AS from_apt, to_apt_icao AS to_apt FROM routes;"

      + $"CREATE VIEW \"v_route_iata\" AS SELECT flight_code AS flight, fapt.apt_iata_code AS from_apt, tapt.apt_iata_code AS to_apt FROM routes "
      + $"INNER JOIN airports fapt ON fapt.apt_icao_code = routes.from_apt_icao "
      + $"INNER JOIN airports tapt ON tapt.apt_icao_code = routes.to_apt_icao;"

      + $"CREATE VIEW \"v_named_route\" AS SELECT flight_code AS flight, fapt.apt_icao_code AS from_apt_ic, fapt.apt_iata_code AS from_apt_ia, fapt.apt_name AS from_apt_name,"
      + $"tapt.apt_icao_code AS to_apt_ic, tapt.apt_iata_code AS to_apt_ia, tapt.apt_name AS to_apt_name  FROM routes "
      + $"INNER JOIN airports fapt ON fapt.apt_icao_code = routes.from_apt_icao "
      + $"INNER JOIN airports tapt ON tapt.apt_icao_code = routes.to_apt_icao;";

    private SQLiteConnection m_dbc = null;

    /// <summary>
    /// Create the table with index
    /// </summary>
    /// <returns></returns>
    private string CreateTable()
    {
      string ret = "";
      try {
        using ( SQLiteCommand sqlite_cmd = m_dbc.CreateCommand( ) ) {
          sqlite_cmd.CommandText = CmdCreateTable + CmdCreateIndex + CmdCreateView;
          sqlite_cmd.ExecuteNonQuery( );
        }
      }
      catch ( SQLiteException sqex ) {
        ret = sqex.Message;
      }

      return ret;
    }

    /// <summary>
    /// Writes the complete db into the sqlite table
    /// </summary>
    /// <param name="adb">The Airport Database as Input</param>
    /// <returns>String as result either empty or error</returns>
    public string LoadDBfromAirports( apDatabase adb )
    {
      string ret = apSqlWriter.WriteSqDB( adb, m_dbc );
      return ret;
    }

    /// <summary>
    /// Writes the complete db into the sqlite table
    /// NOTE:  Closes and releases DB resources
    /// </summary>
    /// <param name="rdb">The Airport Database as Input</param>
    /// <returns>String as result either empty or error</returns>
    public string LoadDBfromARoutes( rtDatabase rdb )
    {
      string ret = rtSqlWriter.WriteSqDB( rdb, m_dbc );

      m_dbc.Close( );
      m_dbc.Dispose( );
      return ret;
    }

    /// <summary>
    /// Create and load the database
    /// </summary>
    /// <param name="dbFile">The filename of the db to create</param>
    /// <returns>Err string empty or error info</returns>
    public string CreateDB( string dbFile )
    {
      string ret = "";

      if ( File.Exists( dbFile ) ) return $"ERROR- db file exists: {dbFile}\n"; // should really not happen with timestamps...

      // create sqLite db file
      SQLiteConnection.CreateFile( dbFile );
      // create a new database connection:
      m_dbc = new SQLiteConnection( $"Data Source={dbFile};Version=3;" );
      // open the connection:
      m_dbc.Open( );

      if ( m_dbc.State != System.Data.ConnectionState.Open ) {
        m_dbc = null;
        return $"ERROR - cannot open db file: {dbFile}\n";
      }

      var s = CreateTable( ); if ( !string.IsNullOrEmpty( s ) ) return s;


      return ret;
    }



  }
}
