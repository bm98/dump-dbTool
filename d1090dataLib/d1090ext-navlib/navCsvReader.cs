using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_navlib
{
  /// <summary>
  /// Reads navaid database in CSV format 
  /// </summary>
  public class navCsvReader
  {
    private const string NULL = "NULL";

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static navRec FromNative( string native )
    {
      /*   0        1       2       3      4            5               6           7               8             9 
     *    "id","filename","ident","name","type","frequency_khz","latitude_deg","longitude_deg","elevation_ft","iso_country",
                 10               11              12                13                  14                 15                      16
     *    "dme_frequency_khz","dme_channel","dme_latitude_deg","dme_longitude_deg","dme_elevation_ft","slaved_variation_deg","magnetic_variation_deg",
                   17      18               19
 *      *    "usageType","power","associated_airport",

    example:  
          85080,"Drayton_Valley_Industrial_NDB-DME_CA","3M","Drayton Valley Industrial",
          "NDB-DME",385,53.26639938354492,-114.95500183105469,2785,"CA",110600,"043X",53.2681,-114.957,2785,,17.225,"LO","LOW","CER3"
   */
      // should be the CSV variant
      string[] e = native.Split( new char[] { ',' } );
      string ident = "", filename = "", name = "", type = "", frequency_khz = "", lat = "", lon = "", elevation = "",
              iso_country = "", dme_frequency_khz = "", dme_channel = "", dme_latitude_deg = "", dme_longitude_deg = "", dme_elevation_ft = "",
              usageType = "", associated_airport = "";

      ident = e[2].Trim( new char[] { ' ', '"' } ).ToUpperInvariant( );
      if ( e.Length > 1 ) filename = e[1].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 4 ) type = e[4].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 3 ) name = e[3].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 5 ) frequency_khz = e[5].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 6 ) lat = e[6].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 7 ) lon = e[7].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 8 ) elevation = e[8].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 9 ) iso_country = e[9].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 10 ) dme_frequency_khz = e[10].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 11 ) dme_channel = e[11].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 12 ) dme_latitude_deg = e[12].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 13 ) dme_longitude_deg = e[13].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 14 ) dme_elevation_ft = e[14].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 17 ) usageType = e[17].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 18 ) associated_airport = e[18].Trim( new char[] { ' ', '"' } );

      return new navRec( ident, filename, type, name, frequency_khz, lat, lon, elevation, iso_country,
                        dme_frequency_khz, dme_channel, dme_latitude_deg, dme_longitude_deg, dme_elevation_ft,
                        usageType, associated_airport );

    }

    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>A table or null</returns>
    private string ReadDbFile( ref navDatabase db, string fName )
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
    /// Reads all data from the given folder
    /// </summary>
    /// <param name="tsvFile">A fully qualified path to where the db files are located</param>
    /// <returns>A populated table or null</returns>
    public string ReadDb( ref navDatabase db, string csvFile )
    {
      if ( !File.Exists( csvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, csvFile );
    }

  }
}
