using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// Reads airport database in CSV format 
  /// </summary>
  public class apCsvReader
  {
    private const string NULL = "NULL";

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static apRec FromNative( string native )
    {
      /*   0      1       2      3          4             5               6           7            8             9             10
          "id","ident","type","name","latitude_deg","longitude_deg","elevation_ft","continent","iso_country","iso_region","municipality",
                   11            12         13          14           15           16              17
          "scheduled_service","gps_code","iata_code","local_code","home_link","wikipedia_link","keywords"

    example:  
          4505,"LSZH","large_airport","Zürich Airport",47.464699,8.54917,1416,"EU","CH","CH-ZH","Zurich",
          "yes","LSZH","ZRH",,"http://www.zurich-airport.com/","http://en.wikipedia.org/wiki/Z%C3%BCrich_Airport",

   */
      // should be the CSV variant
      string[] e = native.Split( new char[] { ',' } );
      string apt_icao_code = "", apt_iata_code = "", iso_country = "", iso_region = "", lat = "", lon = "", elevation = "", apt_type = "", apt_name = "";

      if ( e.Length > 1 )
        apt_icao_code = e[1].Trim( new char[] { ' ', '"' } ).ToUpperInvariant( );
      if ( e.Length > 13 )
        apt_iata_code = ( e[13] == NULL ) ? "" : e[13].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 8 )
        iso_country = ( e[8] == NULL ) ? "" : e[8].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 9 )
        iso_region = ( e[9] == NULL ) ? "" : e[9].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 4 )
        lat = ( e[4] == NULL ) ? "" : e[4].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 5 )
        lon = ( e[5] == NULL ) ? "" : e[5].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 6 )
        elevation = ( e[6] == NULL ) ? "" : e[6].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 2 )
        apt_type = ( e[2] == NULL ) ? "" : e[2].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 3 )
        apt_name = ( e[3] == NULL ) ? "" : e[3].Replace( $"\"", "" ).Replace( $"'", "" ).Trim( new char[] { ' ', '"' } );

      return new apRec( apt_icao_code, apt_iata_code, iso_country, iso_region , lat , lon , elevation , apt_type , apt_name );

    }

    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>A table or null</returns>
    private string ReadDbFile( ref apDatabase db, string fName )
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
    public string ReadDb( ref apDatabase db, string csvFile )
    {
      if ( !File.Exists( csvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, csvFile );
    }

  }
}
