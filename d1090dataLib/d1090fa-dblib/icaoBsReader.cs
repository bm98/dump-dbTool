using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Reads BaseStation aircraft database in CSV format 
  /// </summary>
  public class icaoBsReader
  {
    private const string NULL = "NULL";

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static icaoRec FromNative(string native )
    {
      /*   0                1                2          3         4              5             6              7                8              9            10
       "AircraftID", "FirstCreated", "LastModified", "ModeS", "ModeSCountry", "Country", "Registration", "CurrentRegDate", "PreviousID", "FirstRegDate", "Status",
           11             12            13             14        15         16             17              18             19           20
       "DeRegDate", "Manufacturer", "ICAOTypeCode", "Type", "SerialNo", "PopularName", "GenericName", "AircraftClass", "Engines", "OwnershipStatus", 
            21              22         23           24           25              26            27            28            29         30         31
       "RegisteredOwners", "MTOW", "TotalHours", "YearBuilt", "CofACategory", "CofAExpiry", "UserNotes", "Interested", "UserTag", "InfoURL", "PictureURL1", 
           32           33             34           35            36           37           38           39             40             41             42          
      "PictureURL2", "PictureURL3", "UserBool1", "UserBool2", "UserBool3", "UserBool4", "UserBool5", "UserString1", "UserString2", "UserString3", "UserString4", 
           43           44             45         46         47         48           49
      "UserString5", "UserInt1", "UserInt2", "UserInt3", "UserInt4", "UserInt5", "OperatorFlagCode"
       */
      // should be the CSV variant
      string[] e = native.Split( new char[] { ',' } );
      // FirstCreated	LastModified	ModeS	ModeSCountry	Registration	ICAOTypeCode	type_flight
      string icao = "", regName = "", airctype = "", manufacturer ="";


      if ( e.Length > 3 )
        icao = e[3].Trim(new char[] { ' ', '"' }).ToUpperInvariant( );
      if ( e.Length > 6 )
        regName = e[6].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 13 )
        airctype = e[13].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 12 )
        manufacturer = e[12].Trim( new char[] { ' ', '"' } );

      airctype = ( airctype == "0000" ) ? "" : airctype; // fix NULL

      return new icaoRec( icao, regName, airctype, manufacturer );
    }

    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="db">The icaoDatabase to fill from the file</param>
    /// <param name="fName">The qualified filename</param>
    /// <returns>The result string, either empty or error</returns>
    private static string ReadDbFile( ref icaoDatabase db, string fName )
    {
      var icaoPre = Path.GetFileNameWithoutExtension( fName );
      string ret = "";
      using ( var sr = new StreamReader( fName ) ) {
        string buffer = sr.ReadLine( ); // header line
        buffer = sr.ReadLine( );
        while ( !sr.EndOfStream ) {
          var rec = FromNative( buffer );
          if ( rec.IsValid ) {
            ret += db.Add( rec ); // collect adding information
          }
          buffer = sr.ReadLine( );
        }
        //
      }
      return ret;
    }

    /// <summary>
    /// Reads all data from the given file
    /// </summary>
    /// <param name="db">The icaoDatabase to fill from the file</param>
    /// <param name="csvFile">The file to read</param>
    /// <returns>The result string, either empty or error</returns>
    public static string ReadDb( ref icaoDatabase db, string csvFile )
    {
      if ( !File.Exists( csvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, csvFile );
    }



  }
}
