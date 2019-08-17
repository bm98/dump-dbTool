using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// The dump1090-fa /db/icao_aircraft_types.json record type
  /// 
  /// 	{"A002":
  /// 	  {"desc":"G1P",
  ///     "wtc":"L"},
  ///    ..}
  /// </summary>
  public class icaoActRec
  {
    // XSV divider - must not appear in any string read from icao, modeS and aircraft files
    const string XDIV = "¬"; // Alt+0172

    ///    "ModelFullName":"Dornier 328JET",
    ///    "Description":"L2J",
    ///    "WTC":"M",
    ///    "Designator":"J328",
    ///    "ManufacturerCode":"328 SUPPORT SERVICES",
    ///    "AircraftDescription":"LandPlane",
    ///    "EngineCount":"2",
    ///    "EngineType":"Jet"     
    ///    
    // fields in db
    public string icaotype = ""; // The ICAO aircraft type designator is a two-, three- or four-character alphanumeric code
    public string description = "";
    public string wtccode = "";
    // available too...
    public string fullname = "";
    public string manufacturer = "";
    public string acdesc = "";
    public string engcount = "";
    public string engtype = "";



    /// <summary>
    /// Sanity check on icao (ModeS) codes
    /// </summary>
    /// <param name="icaoSrc">Input ModeS name (hex)</param>
    /// <returns>The UCase ModeS name or an empty string if not passed</returns>
    private static string icaoSanity( string icaoSrc )
    {
      string ret = icaoSrc.ToUpperInvariant( ); // use UCase only here
      if ( ret.Length < 2 ) return ""; // FAIL length does not match
      if ( ret.Length > 4 ) return ""; // FAIL length does not match

      return ret;
    }

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="ic">Designator</param>
    /// <param name="desc">Description</param>
    /// <param name="wtc">WTC</param>
    /// <param name="fname">ModelFullName</param>
    /// <param name="man">ManufacturerCode</param>
    /// <param name="acd">AircraftDescription</param>
    /// <param name="ecnt">EngineCount</param>
    /// <param name="etyp">EngineType</param>
    public icaoActRec( string ic, string desc, string wtc, string fname, string man, string acd, string ecnt, string etyp )
    {
      icaotype = icaoSanity( ic );
      description = desc; 
      wtccode = wtc;
      fullname = fname;
      manufacturer = man;
      acdesc = acd;
      engcount = ecnt;
      engtype = etyp;
    }

    /// <summary>
    /// cTor: populate the record (minimum record)
    /// </summary>
    /// <param name="ic">Designator</param>
    /// <param name="desc">Description</param>
    /// <param name="wtc">WTC</param>
    public icaoActRec( string ic, string desc, string wtc )
    {
      icaotype = icaoSanity( ic );
      description = desc;
      wtccode = wtc;
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( !string.IsNullOrEmpty( icaotype ) ); }

    /// 	{"A002":
    /// 	  {"desc":"G1P",
    ///     "wtc":"L"},
    ///    ..}


    /// <summary>
    /// Returns the content in Json Notation for csv use
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson()
    {
      string ret = $"\"{icaotype}\":{{";
      if ( !string.IsNullOrEmpty( description ) ) ret += $"\"desc\":\"{description}\",";
      if ( !string.IsNullOrEmpty( wtccode ) ) ret += $"\"wtc\":\"{wtccode}\",";

      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      ret += $"}}";
      return ret;
    }

  }
}

