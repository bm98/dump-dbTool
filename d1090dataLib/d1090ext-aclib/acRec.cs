using System;

namespace d1090dataLib.d1090ext_aclib
{
  /// <summary>
  /// An Aircraft Record (our own format - may be no longer used)
  /// </summary>
  public class acRec
  {

    // fields in db
    public string icao_code = "";
    public string regid = "";
    public string model = ""; // icaoType (model)
    public string typedesc = "";  // Type description
    public string operator_ = "";

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="ic">icao Smode id (UPPERCASE)</param>
    /// <param name="r">Registration</param>
    /// <param name="m">icaoType (model)</param>
    /// <param name="td">Type description</param>
    /// <param name="o">Operator</param>
    public acRec( string ic, string r, string m, string td, string o )
    {
      icao_code = ic.ToUpperInvariant( );
      regid = r.ToUpperInvariant();
      model = m.ToUpperInvariant( );
      typedesc = td;
      operator_ = o;
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( !string.IsNullOrEmpty( icao_code ) ); }

    /// <summary>
    /// Returns the content in Json Notation
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson()
    {
      string ret = $"\"{icao_code}\":{{";
      if ( !string.IsNullOrEmpty( regid ) ) {
        ret += $"\"r\":\"{regid}\",";
      }
      if ( !string.IsNullOrEmpty( model ) ) {
        ret += $"\"t\":\"{model}\"";
      }
      if ( !string.IsNullOrEmpty( typedesc ) ) {
        ret += $"\"td\":\"{typedesc}\"";
      }
      if ( !string.IsNullOrEmpty( operator_ ) ) {
        ret += $"\"o\":\"{operator_}\"";
      }
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      ret += $"}}";
      return ret;
    }

    /// <summary>
    /// Returns the content in Jason Notation - shortened icao entry
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson( string prefix )
    {
      if ( !icao_code.StartsWith( prefix ) ) {
        // PROGRAM ERROR...
        ; // stop in debugger
      }
      var tIcao = icao_code.Substring( prefix.Length ); // cut the prefix from the record
      string ret = $"\"{tIcao}\":{{";
      if ( !string.IsNullOrEmpty( regid ) ) ret += $"\"r\":\"{regid}\",";         // element name as icaoRec !!!
      if ( !string.IsNullOrEmpty( model ) ) ret += $"\"t\":\"{model}\"";          // element name as icaoRec !!!
      if ( !string.IsNullOrEmpty( typedesc ) ) ret += $"\"td\":\"{typedesc}\"";   // element name as icaoRec !!!
      if ( !string.IsNullOrEmpty( operator_ ) ) ret += $"\"o\":\"{operator_}\"";  // element name as icaoRec !!!

      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      ret += $"}}";
      return ret;
    }

    /// <summary>
    /// Returns the Csv header line
    /// </summary>
    public static string CsvHeader { get => "icao_code,regid,model,typedesc,operator_"; }

    /// <summary>
    /// Returns the content in Csv Notation
    /// </summary>
    /// <returns>The CSV database record</returns>
    public string AsCsv()
    {
      string ret = $"{icao_code},";
      ret += ( !string.IsNullOrEmpty( regid ) ) ? $"{regid}," : ",";
      ret += ( !string.IsNullOrEmpty( model ) ) ? $"{model}," : ",";
      ret += ( !string.IsNullOrEmpty( typedesc ) ) ? $"{typedesc}," : ",";
      ret += ( !string.IsNullOrEmpty( operator_ ) ) ? $"{operator_}," : ",";
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      return ret;
    }

  }
}
