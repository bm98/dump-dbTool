using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Reader to get data from: "aircraftDatabase.csv" from (https://opensky-network.org/datasets/metadata/)
  /// 
  /// CSV: "icao24","registration","manufacturericao","manufacturername","model","typecode",
  /// "serialnumber","linenumber","icaoaircrafttype","operator","operatorcallsign","operatoricao",
  /// "operatoriata","owner","testreg","registered","reguntil","status","built","firstflightdate",
  /// "seatconfiguration","engines","modes","adsb","acars","notes","categoryDescription"
  /// 
  /// "ae267b","6533","VOUGHT","Aerospatiale","MH-65C Dolphin","AS65",
  /// "6182","","H2T","","","","","United States Coast Guard","","","","","","",
  /// "","","false","false","false","","No ADS-B Emitter Category Information"

  /// </summary>
  public class icaoCsvOpenSkyReader
  {

    private static char[] WS = new char[] { ' ', '"' };

    /// <summary>
    /// Returns a new Icao Record from given CSV
    /// </summary>
    /// <param name="js">The record as CSV line</param>
    private static icaoRec FromNative( string native )
    {
      // should be the CSV variant
      string[] e = CsvTools.Split( native, out bool qquoted, ',' ); // either comma separated - from example..
      //     0           1                 2                 3            4         5
      // "icao24","registration","manufacturericao","manufacturername","model","typecode",
      //     6               7                 8            9           10                 11
      // "serialnumber","linenumber","icaoaircrafttype","operator","operatorcallsign","operatoricao",
      //     12           13       14          15          16        17      18          19
      // "operatoriata","owner","testreg","registered","reguntil","status","built","firstflightdate",
      //         20             21       22      23     24      25         26
      // "seatconfiguration","engines","modes","adsb","acars","notes","categoryDescription"

      if ( e.Length < 6 ) return new icaoRec( "", "", "" ); // Must include "typecode" to create a valid record..

      var icao = e[0].Trim( WS ).ToUpperInvariant( );         // icao24
      var regName = e[1].Trim( WS ).ToUpperInvariant( );      // registration
      var airctype = e[5].Trim( WS ).ToUpperInvariant( );     // typecode

      var manufacturer = e[3].Trim( WS );     // manufacturername
      var airctypename = e[4].Trim( WS );     // model

      airctype = ( airctype == "0000" ) ? "" : airctype; // fix NULL
      manufacturer = manufacturer.Replace( "'", "`" );  // cannot have single quotes for SQL (and don't want to escape...)
      airctypename = airctypename.Replace( "'", "`" );  // cannot have single quotes for SQL (and don't want to escape...)

      return new icaoRec( icao, regName, airctype, manufacturer );
    }


    /// <summary>
    /// Reads all data from the given file
    /// </summary>
    /// <param name="db">The icaoDatabase to fill</param>
    /// <param name="fName">A fully qualified name</param>
    /// <returns>The result string, either empty or error</returns>
    public static string ReadDb( ref icaoDatabase db, string fName )
    {
      if ( !File.Exists( fName ) ) {
        return $"File {fName} does not exist\n";
      }

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

  }
}
