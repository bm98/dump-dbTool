using System;

namespace d1090dataLib.d1090ext_rtlib
{
  /// <summary>
  /// A Route Record
  /// </summary>
  public class rtRec
  {

    // fields in db
    public string flight_code = "";
    public string from_apt_icao = "";
    public string to_apt_icao = "";

    /// <summary>
    /// cTor: populate the record
    /// </summary>

    public rtRec( string fc, string from, string to )
    {
      flight_code = fc;
      from_apt_icao = from;
      to_apt_icao = to;
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( !string.IsNullOrEmpty( flight_code ) ); }

    /// <summary>
    /// Returns the content in Json Notation
    /// NOTE this will not return the dividing comma
    /// </summary>
    /// <returns>The Json database record</returns>
    public string AsJson()
    {
      string ret = $"\"{flight_code}\":{{";
      if ( !string.IsNullOrEmpty( from_apt_icao ) ) {
        ret += $"\"f\":\"{from_apt_icao}\",";
      }
      if ( !string.IsNullOrEmpty( to_apt_icao ) ) {
        ret += $"\"t\":\"{to_apt_icao}\"";
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
      if ( !flight_code.StartsWith( prefix ) ) {
        // PROGRAM ERROR...
        ; // stop in debugger
      }
      var tIcao = flight_code.Substring( prefix.Length ); // cut the prefix from the record
      string ret = $"\"{tIcao}\":{{";
      if ( !string.IsNullOrEmpty( from_apt_icao ) ) {
        ret += $"\"f\":\"{from_apt_icao}\",";
      }
      if ( !string.IsNullOrEmpty( to_apt_icao ) ) {
        ret += $"\"t\":\"{to_apt_icao}\"";
      }
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      ret += $"}}";
      return ret;
    }

    /// <summary>
    /// Returns the Csv header line
    /// </summary>
    public static string CsvHeader { get => "flight_code,from_apt_icao,to_apt_icao"; }

    /// <summary>
    /// Returns the content in Csv Notation
    /// </summary>
    /// <returns>The CSV database record</returns>
    public string AsCsv()
    {
      string ret = $"{flight_code},";
      ret += ( !string.IsNullOrEmpty( from_apt_icao ) ) ? $"{from_apt_icao}," : ",";
      ret += ( !string.IsNullOrEmpty( to_apt_icao ) ) ? $"{to_apt_icao}," : ",";
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      return ret;
    }

  }
}
