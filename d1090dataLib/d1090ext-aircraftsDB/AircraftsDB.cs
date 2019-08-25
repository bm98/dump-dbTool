using System;
using System.IO;
using System.Data.SQLite;
using d1090dataLib.d1090fa_dblib;

namespace d1090dataLib.d1090ext_aircraftsDB
{
  /// <summary>
  /// Create the dump1090fa-aircrafts.sqb database file
  /// from the icaoDatabase
  /// </summary>
  public class AircraftsDB
  {
    /*
     * CREATE TABLE "fa_modes" ("icao" TEXT NOT NULL, "registration" TEXT, "airctype" TEXT, "manufacturer" TEXT, "aircname" TEXT, "operator_" TEXT ); 
     * CREATE UNIQUE INDEX "i_fa_icao" on fa_modes (icao ASC);
     */
    private  string CmdCreateTable =
      $"CREATE TABLE \"fa_modes\" (\"icao\" TEXT NOT NULL, \"registration\" TEXT, \"airctype\" TEXT, \"manufacturer\" TEXT, \"aircname\" TEXT, \"operator_\" TEXT );";
    private  string CmdCreateIndex =
      $"CREATE UNIQUE INDEX \"i_fa_icao\" on fa_modes (icao ASC);";

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
          sqlite_cmd.CommandText = CmdCreateTable + CmdCreateIndex;
          sqlite_cmd.ExecuteNonQuery( );
        }
      }
      catch (SQLiteException sqex ) {
        ret = sqex.Message;
      }

      return ret;
    }

    /// <summary>
    /// Writes the complete db into the sqlite table
    /// NOTE: Closes and releases SqLite DB items
    /// </summary>
    /// <param name="idb">The Icao Database as Input</param>
    /// <returns>String as result either empty or error</returns>
    public string LoadDBfromIcao( icaoDatabase idb )
    {
      string ret = icaoSqlWriter.WriteSqDB( idb, m_dbc );
      m_dbc.Close( );
      m_dbc.Dispose( );
      return ret;
    }

    /// <summary>
    /// Create and load the database
    /// </summary>
    /// <param name="idb">The data source</param>
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
