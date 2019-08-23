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
    public string icao = "";
    public string registration = "";
    public string airctype = "";
    public string manufacturer = "";
    public string airctypedesc = "";
    public string operator_ = "";

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
      icao = icaoSanity(ic);
      registration = r;  // remove null recs from aircraft db
      airctype = t.ToUpperInvariant();
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
      icao = icaoSanity( ic );
      registration = r;
      airctype = t.ToUpperInvariant( );
      manufacturer = m;
    }

    /// <summary>
    /// Add the prefix to complete the ModeS name
    /// </summary>
    /// <param name="prefix">The filename prefix missing from the native record</param>
    public void AddPrefix(string prefix )
    {
      this.icao = icaoSanity( prefix + this.icao );
    }

    /// <summary>
    /// Update this rec with values from AC record
    /// </summary>
    /// <param name="ac"></param>
    public void Update( acRec ac )
    {
      if ( ac.icao_code != this.icao ) return; // sanity

      registration = ( string.IsNullOrEmpty( registration ) ) ? ac.regid : registration; // this has precedence
      airctype = ( string.IsNullOrEmpty( airctype ) ) ? ac.model : airctype; // this has precedence
      airctypedesc = ( string.IsNullOrEmpty( airctypedesc ) ) ? ac.typedesc : airctypedesc; // this has precedence
      operator_ = ( string.IsNullOrEmpty( operator_ ) ) ? ac.operator_ : operator_; // this has precedence
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( icao.Length == 6 ); } // FAIL length does not match

    /// <summary>
    /// Returns the content in Json Notation for csv use
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson()
    {
      string ret = $"\"{icao}\":{{";
      if ( !string.IsNullOrEmpty( registration ) ) ret += $"\"r\":\"{registration}\",";
      if ( !string.IsNullOrEmpty( airctype ) ) ret += $"\"t\":\"{airctype}\",";
      if ( !string.IsNullOrEmpty( manufacturer ) ) ret += $"\"m\":\"{manufacturer}\",";
      if ( !string.IsNullOrEmpty( airctypedesc ) ) ret += $"\"td\":\"{airctypedesc}\",";
      if ( !string.IsNullOrEmpty( operator_ ) ) ret += $"\"o\":\"{operator_}\",";

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
      if ( !icao.StartsWith( prefix ) ) {
        // PROGRAM ERROR...
        ; // stop in debugger
      }
      var tIcao = icao.Substring( prefix.Length ); // cut the prefix from the record


      string ret = $"\"{tIcao}\":{{";
      if ( !string.IsNullOrEmpty( registration ) ) ret += $"\"r\":\"{registration}\",";
      if ( !string.IsNullOrEmpty( airctype ) ) ret += $"\"t\":\"{airctype}\",";

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
      string ret = $"{icao}{XDIV}{AsJson( )}";
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
      string ret = $"{icao},";
      ret += ( !string.IsNullOrEmpty( registration ) ) ? $"{registration}," : ",";
      ret += ( !string.IsNullOrEmpty( airctype ) ) ? $"{airctype}," : ",";
      ret += ( !string.IsNullOrEmpty( manufacturer ) ) ? $"{manufacturer}," : ",";
      ret += ( !string.IsNullOrEmpty( airctypedesc ) ) ? $"{airctypedesc}," : ",";
      ret += ( !string.IsNullOrEmpty( operator_ ) ) ? $"{operator_}," : ",";
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      return ret;
    }


  }
}

