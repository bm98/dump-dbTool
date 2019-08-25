using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Reader to get data from: "doc8643AircraftTypes.csv" from (https://opensky-network.org/datasets/metadata/)
  /// CSV: "AircraftDescription","Description","Designator","EngineCount","EngineType","ManufacturerCode","ModelFullName","WTC"
  ///       "LandPlane","L2J","J328","2","Jet","328 SUPPORT SERVICES","Dornier 328JET","M"
  /// </summary>
  public class icaoActCsvReader
  {
    /// <summary>
    /// Returns a new Icao Record from given CSV
    /// </summary>
    /// <param name="js">The record as CSV line</param>
    private static icaoActRec FromNative( string native )
    {
      // should be the CSV variant
      string[] e = native.Split( new char[] { ',', ';' } ); // either comma or semi separated
      //             0                1             2             3            4              5                6            7
      //   "AircraftDescription","Description","Designator","EngineCount","EngineType","ManufacturerCode","ModelFullName","WTC"

      if ( e.Length < 8 ) return null; // Must include WTC to create a valid record..

      var acd = e[0];
      var desc = e[1].ToUpperInvariant( );
      var icao = e[2].ToUpperInvariant( );
      var ec = e[3];
      var et = e[4];
      var mcod = e[5].ToUpperInvariant( );
      var mfn = e[6];
      var wtc = e[7].ToUpperInvariant( );

      return new icaoActRec( icao, desc, wtc, mfn, mcod, acd, ec, et );
    }


    /// <summary>
    /// Reads all data from the given file
    /// </summary>
    /// <param name="db">The icaoActDatabase to fill</param>
    /// <param name="fName">A fully qualified name</param>
    /// <returns>The result string, either empty or error</returns>
    public static string ReadDb( ref icaoActDatabase db, string fName )
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
