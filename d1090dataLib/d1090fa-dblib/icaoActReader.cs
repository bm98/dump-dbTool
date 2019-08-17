using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Reads the ICAO TypeDesignator Json File (captured from http response...)
  /// The ICAO aircraft type designator is a two-, three- or four-character alphanumeric code designating every aircraft type (and some sub-types) that may appear in flight planning.
  /// sample:
  /// [
  ///   {
  ///    "ModelFullName":"Dornier 328JET",
  ///    "Description":"L2J",
  ///    "WTC":"M",
  ///    "Designator":"J328",
  ///    "ManufacturerCode":"328 SUPPORT SERVICES",
  ///    "AircraftDescription":"LandPlane",
  ///    "EngineCount":"2",
  ///    "EngineType":"Jet"
  ///    }, 
  ///    ...
  ///  ]
  /// </summary>
  public class icaoActReader
  {
    /// <summary>
    /// Returns a new Icao Record from given Jason
    /// </summary>
    /// <param name="js">The record as Jason fragment</param>
    private static icaoActRec FromNative( string js )
    {
      JsonRecord jRec = JsonParser.Decompose( js );
      if ( jRec?.Count > 0 ) {
        var icao = !jRec.Values[0].ContainsKey( "Designator" ) ? "" : jRec.Values[0]["Designator"];
        var desc = !jRec.Values[0].ContainsKey( "Description" ) ? "" : jRec.Values[0]["Description"];
        var wtc = !jRec.Values[0].ContainsKey( "WTC" ) ? "" : jRec.Values[0]["WTC"];
        var mfn = !jRec.Values[0].ContainsKey( "ModelFullName" ) ? "" : jRec.Values[0]["ModelFullName"];
        var mcod = !jRec.Values[0].ContainsKey( "ManufacturerCode" ) ? "" : jRec.Values[0]["ManufacturerCode"];
        var acd = !jRec.Values[0].ContainsKey( "AircraftDescription" ) ? "" : jRec.Values[0]["AircraftDescription"];
        var ec = !jRec.Values[0].ContainsKey( "EngineCount" ) ? "" : jRec.Values[0]["EngineCount"];
        var et = !jRec.Values[0].ContainsKey( "EngineType" ) ? "" : jRec.Values[0]["EngineType"];
        var iRec = new icaoActRec( icao.ToUpperInvariant( ), desc, wtc, mfn, mcod, acd, ec, et );
        return iRec;
      }
      else {
        return null;
      }
    }


    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>A table or null</returns>
    public string ReadDb( ref icaoActDatabase db, string fName )
    {
      if ( !File.Exists( fName ) ) {
        return $"File {fName} does not exist\n";
      }

      string ret = "";
      using ( var sr = new StreamReader( fName ) ) {
        string buffer = sr.ReadToEnd( );
        buffer = buffer.Replace( "\n", "" ).Replace( "\r", "" ).Trim( ); // cleanup any CR, LFs and whitespaces
        buffer = buffer.Substring( 1 ); // skip enclosing [
        var fragment = JsonParser.ExtractFragment( buffer );
        while ( !string.IsNullOrEmpty( fragment ) ) {
          buffer = buffer.Substring( fragment.Length + 1 ); // remove extracted + comma
          var rec = FromNative( fragment );
          if ( rec.IsValid ) {
            ret += db.Add( rec ); // collecting add information
          }
          fragment = JsonParser.ExtractFragment( buffer );
        }
      }
      return ret;
    }


  }
}
