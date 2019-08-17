using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;
using d1090dataLib.d1090fa_dblib;

namespace d1090dataLib.bsDB_connection
{
  /// <summary>
  /// Connector to the BaseStation sqLite database
  /// </summary>
  public class BaseStation
  {

    private string m_bsFilename = "";
    private SQLiteConnection m_dbc = null;


    public bool Connect( string bsDB_Filename )
    {
      if ( !File.Exists( bsDB_Filename ) ) return false; // ERROR EXIT
      m_bsFilename = bsDB_Filename;

      try {
        // create a new database connection:
        m_dbc = new SQLiteConnection( $"Data Source={m_bsFilename};Version=3;" );
        // open the connection:
        m_dbc.Open( );

        if ( m_dbc.State != System.Data.ConnectionState.Open ) {
          m_dbc = null;
          return false;
        }
      }
      catch ( SQLiteException sqEx ) {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Read the BS Aircraft table data into the IcaoDB provided
    /// NOTE: Closes and releases SqLite DB items
    /// </summary>
    /// <param name="idb">An ICAO db to fill</param>
    /// <returns>Result string either empty or error information</returns>
    public string ReadDb( ref icaoDatabase idb )
    {
      string ret = "";

      if ( m_dbc?.State == System.Data.ConnectionState.Open ) {
        using ( SQLiteCommand sqlite_cmd = m_dbc.CreateCommand( ) ) {
          sqlite_cmd.CommandText = "SELECT * FROM Aircraft";
          using ( SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader( ) ) {
            // The SQLiteDataReader allows us to run through each row per loop
            while ( sqlite_datareader.Read( ) ) // Read() returns true if there is still a result line to read
            {
              /*  Table aircraft:
               *   0                1                2          3         4              5             6              7                8              9            10
                   "AircraftID", "FirstCreated", "LastModified", "ModeS", "ModeSCountry", "Country", "Registration", "CurrentRegDate", "PreviousID", "FirstRegDate", "Status",
                       11             12            13             14        15         16             17              18             19           20
                   "DeRegDate", "Manufacturer", "ICAOTypeCode", "Type", "SerialNo", "PopularName", "GenericName", "AircraftClass", "Engines", "OwnershipStatus", 
                        21              22         23           24           25              26            27            28            29         30         31
                   "RegisteredOwners", "MTOW", "TotalHours", "YearBuilt", "CofACategory", "CofAExpiry", "UserNotes", "Interested", "UserTag", "InfoURL", "PictureURL1", 
                       32           33             34           35            36           37           38           39             40             41             42          
                  "PictureURL2", "PictureURL3", "UserBool1", "UserBool2", "UserBool3", "UserBool4", "UserBool5", "UserString1", "UserString2", "UserString3", "UserString4", 
                       43           44             45         46         47         48           49
                  "UserString5", "UserInt1", "UserInt2", "UserInt3", "UserInt4", "UserInt5", "OperatorFlagCode"

              --> we use: [3] = icao, [6] = regName, [13] = airctype, [12] = manufacturer
              */

              // Print out the content of the text field:
              // System.Console.WriteLine("DEBUG Output: '" + sqlite_datareader["text"] + "'");

              string icao = sqlite_datareader.GetString( 3 );
              string regName = sqlite_datareader.GetValue( 6 ).ToString( );
              string airctype = sqlite_datareader.GetValue( 13 ).ToString( );
              string manufacturer = sqlite_datareader.GetValue( 12 ).ToString( );

              var rec = new icaoRec( icao, regName, airctype, manufacturer );
              if ( rec.IsValid ) {
                ret += idb.Add( rec ); // collect adding information
              }
            }//while
          }//reader
        }//cmd
        m_dbc.Close( );
        m_dbc.Dispose( );
      }
      return ret;
    }


  }
}
