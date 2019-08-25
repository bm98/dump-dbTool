using System;
using System.Data;
using System.Globalization;
using d1090dataLib.d1090ext_aclib;

namespace d1090dataLib.d1090fa_dblib
{
/// <summary>
/// The dump1090-fa /db/xy.json record type
/// 
/// 	"01001": {
/// 	"r": "V5-NAM",		
/// 	"t": "F900"
/// 	},
/// </summary>
  public class icaoRec
  {
    // XSV divider - must not appear in any string read from icao, modeS and aircraft files
    const string XDIV = "¬"; // Alt+0172


    // fields in db
    // minimum mandatory
    public string Icao = "";              // ModeS ID (71002B)
    public string Registration = "";      // Registration No (HZ-ARC)
    public string AircTypeCode = "";      // Aircraft Type Code (B789)
    // optional if available
    public string ManufacturerName = "";  // Manufacturer Name (Boeing)
    public string AircTypeName = "";      // Aircraft Type Name (787 9)
    public string OperatorName = "";      // any of the operator designations

    /// <summary>
    /// Sanity check on icao (ModeS) codes
    /// </summary>
    /// <param name="icaoSrc">Input ModeS name (hex)</param>
    /// <returns>The UCase ModeS name or an empty string if not passed</returns>
    private static string icaoSanity(string icaoSrc )
    {
      string ret = icaoSrc.ToUpperInvariant( ); // use UCase only here
      if ( !long.TryParse( ret, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long test ) ) return ""; // FAIL not hex

      return ret;
    }

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="ic">icao Smode id (UPPERCASE)</param>
    /// <param name="r">registration</param>
    /// <param name="t">icao type code</param>
    public icaoRec( string ic, string r, string t )
    {
      Icao = icaoSanity(ic);
      Registration = r;  // remove null recs from aircraft db
      AircTypeCode = t.ToUpperInvariant();
    }

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="ic">icao Smode id (UPPERCASE)</param>
    /// <param name="r">registration</param>
    /// <param name="t">icao type code</param>
    /// <param name="m">Manufacturer</param>
    public icaoRec( string ic, string r, string t, string m )
    {
      Icao = icaoSanity( ic );
      Registration = r;
      AircTypeCode = t.ToUpperInvariant( );
      ManufacturerName = m;
    }

    /// <summary>
    /// Add the prefix to complete the ModeS name
    /// </summary>
    /// <param name="prefix">The filename prefix missing from the native record</param>
    public void AddPrefix(string prefix )
    {
      this.Icao = icaoSanity( prefix + this.Icao );
    }

    /// <summary>
    /// Update this rec with values from AC record
    /// </summary>
    /// <param name="ac"></param>
    public void Update( acRec ac )
    {
      if ( ac.icao_code != this.Icao ) return; // sanity

      Registration = ( string.IsNullOrEmpty( Registration ) ) ? ac.regid : Registration; // this has precedence
      AircTypeCode = ( string.IsNullOrEmpty( AircTypeCode ) ) ? ac.model : AircTypeCode; // this has precedence
      AircTypeName = ( string.IsNullOrEmpty( AircTypeName ) ) ? ac.typedesc : AircTypeName; // this has precedence
      OperatorName = ( string.IsNullOrEmpty( OperatorName ) ) ? ac.operator_ : OperatorName; // this has precedence
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( Icao.Length == 6 ); } // FAIL length does not match

    /// <summary>
    /// Returns the content in Json Notation for csv use
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson()
    {
      string ret = $"\"{Icao}\":{{";
      if ( !string.IsNullOrEmpty( Registration ) ) ret += $"\"r\":\"{Registration}\",";
      if ( !string.IsNullOrEmpty( AircTypeCode ) ) ret += $"\"t\":\"{AircTypeCode}\",";
      if ( !string.IsNullOrEmpty( ManufacturerName ) ) ret += $"\"m\":\"{ManufacturerName}\",";
      if ( !string.IsNullOrEmpty( AircTypeName ) ) ret += $"\"td\":\"{AircTypeName}\",";
      if ( !string.IsNullOrEmpty( OperatorName ) ) ret += $"\"o\":\"{OperatorName}\",";

      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      ret += $"}}";
      return ret;
    }

    /// <summary>
    /// Returns the content in Json Notation for the FA db- shortened icao entry
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson( string prefix )
    {
      if ( !Icao.StartsWith( prefix ) ) {
        // PROGRAM ERROR...
        ; // stop in debugger
      }
      var tIcao = Icao.Substring( prefix.Length ); // cut the prefix from the record


      string ret = $"\"{tIcao}\":{{";
      if ( !string.IsNullOrEmpty( Registration ) ) ret += $"\"r\":\"{Registration}\",";
      if ( !string.IsNullOrEmpty( AircTypeCode ) ) ret += $"\"t\":\"{AircTypeCode}\",";

      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      ret += $"}}";
      return ret;
      //      return $"\"{icao}\":{{\"r\":\"{regName}\",\"t\":\"{airctype}\"}}"; // should be compliant
    }

    /// <summary>
    /// Returns the Csv header line
    /// </summary>
    public static string JsonXsvHeader { get => "icao" + XDIV + "json"; }

    /// <summary>
    /// Returns the content in Csv Notation
    /// </summary>
    /// <returns>The CSV database record</returns>
    public string AsJsonXsv()
    {
      string ret = $"{Icao}{XDIV}{AsJson( )}";
      return ret;
    }

    /// <summary>
    /// Returns the Csv header line
    /// </summary>
    public static string CsvHeader { get => "icao,registration,airctype,manufacturer,airctypedesc,operator_"; }

    /// <summary>
    /// Returns the content in Csv Notation
    /// </summary>
    /// <returns>The CSV database record</returns>
    public string AsCsv()
    {
      string ret = $"{Icao},";
      ret += ( !string.IsNullOrEmpty( Registration ) ) ? $"{Registration}," : ",";
      ret += ( !string.IsNullOrEmpty( AircTypeCode ) ) ? $"{AircTypeCode}," : ",";
      ret += ( !string.IsNullOrEmpty( ManufacturerName ) ) ? $"{ManufacturerName}," : ",";
      ret += ( !string.IsNullOrEmpty( AircTypeName ) ) ? $"{AircTypeName}," : ",";
      ret += ( !string.IsNullOrEmpty( OperatorName ) ) ? $"{OperatorName}," : ",";
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      return ret;
    }


  }
}

