using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_rtlib
{
  public class rtTsvReader
  {

    private const string NULL = "NULL";

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static rtRec FromNative( string native )
    {
      // should be the TSV variant
      string[] e = native.Split( new char[] { '\t' } );
      //     0          1              2                 3                4              5            6
      // CallSign	Operator_ICAO	FromAirport_ICAO	FromAirport_Time	ToAirport_ICAO	ToAirport_Time	RouteStop
      string flight_code = "", from_apt_icao = "", to_apt_icao = "";

      flight_code = e[0].ToUpperInvariant( );
      if ( e.Length > 2 )
        from_apt_icao = ( e[2] == NULL ) ? "" : e[2];
      if ( e.Length > 4 )
        to_apt_icao = ( e[4] == NULL ) ? "" : e[4];

      if ( from_apt_icao == "NULL" ) flight_code = ""; // invalidate
      if ( from_apt_icao == "????" ) flight_code = ""; // invalidate
      if ( to_apt_icao == "NULL" ) flight_code = ""; // invalidate
      if ( to_apt_icao == "???" ) flight_code = ""; // invalidate

      // we have seen IATA codes in ICAO fields - correct if possible
      if ( from_apt_icao.Length < 4 ) from_apt_icao = d1090ext_aplib.apDatabase.GetICAOfromIATA( from_apt_icao );
      if ( to_apt_icao.Length < 4 ) to_apt_icao = d1090ext_aplib.apDatabase.GetICAOfromIATA( to_apt_icao );

      return new rtRec( flight_code, from_apt_icao, to_apt_icao );
    }



    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>A table or null</returns>
    private string ReadDbFile( ref rtDatabase db, string fName )
    {
      var icaoPre = Path.GetFileNameWithoutExtension( fName );
      string ret = "";
      using ( var sr = new StreamReader( fName ) ) {
        string buffer = sr.ReadLine( );
        buffer = sr.ReadLine( ); // header line
        while ( !sr.EndOfStream ) {
          if ( buffer.Contains( "." ) ) { buffer = sr.ReadLine( ); continue; }; // invalidate
          if ( buffer.Contains( "/" ) ) { buffer = sr.ReadLine( ); continue; }; // invalidate
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
    public string ReadDb( ref rtDatabase db, string tsvFile )
    {
      if ( !File.Exists( tsvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, tsvFile );
    }


  }
}
