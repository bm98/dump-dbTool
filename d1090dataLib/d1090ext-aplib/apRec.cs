using System;
using System.Collections.Generic;
using System.Linq;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// An Airport Record
  /// </summary>
  public class apRec
  {
    public enum AptTypes
    {
      All = -1,
      Other = 0,
      small_airport,
      medium_airport,
      large_airport,
      heliport,
      balloonport,
      seaplane_base,
      closed,
    }


    // fields in db
    public string apt_icao_code = "";
    public string apt_iata_code = "";
    public string iso_country = "";
    public string iso_region = "";
    public string lat = "";
    public string lon = "";
    public string elevation = "";
    public string apt_type = "";
    public string apt_name = "";

    /*
"id","ident","type","name","latitude_deg","longitude_deg","elevation_ft","continent","iso_country","iso_region","municipality","scheduled_service","gps_code","iata_code","local_code","home_link","wikipedia_link","keywords"

 4505,"LSZH","large_airport","Zürich Airport",47.464699,8.54917,1416,"EU","CH","CH-ZH","Zurich","yes","LSZH","ZRH",,"http://www.zurich-airport.com/","http://en.wikipedia.org/wiki/Z%C3%BCrich_Airport",

 */

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="ic">icao id (UPPERCASE)</param>
    public apRec( string ic, string ia, string c, string r, string la, string lo, string elev, string t, string n )
    {
      apt_icao_code = ic.ToUpperInvariant( );
      apt_iata_code = ia.ToUpperInvariant( );
      iso_country = c.ToUpperInvariant( );
      iso_region = r.ToUpperInvariant( );
      lat = la;
      lon = lo;
      elevation = elev;
      apt_type = t;
      apt_name = n;
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( !string.IsNullOrEmpty( apt_icao_code ) ); }

    /// <summary>
    /// Returns true if the record matches one of the given types
    /// </summary>
    /// <param name="aptTypes">An array of NavTypes</param>
    /// <returns>True for a match, else false</returns>
    public bool IsTypeOf( AptTypes[] aptTypes )
    {
      if ( aptTypes.Contains( AptTypes.All ) ) return true;
      return aptTypes.Contains( TypeAsEnum );
    }


    /// <summary>
    /// Returns the Csv header line
    /// </summary>
    public static string CsvHeader { get => "apt_icao_code,apt_iata_code,iso_country,iso_region,lat,lon,elevation,apt_type,apt_name"; }

    /// <summary>
    /// Returns the content in Csv Notation
    /// </summary>
    /// <returns>The CSV database record</returns>
    public string AsCsv()
    {
      string ret = $"{apt_icao_code},";
      ret += ( !string.IsNullOrEmpty( apt_iata_code ) ) ? $"{apt_iata_code}," : ",";
      ret += ( !string.IsNullOrEmpty( iso_country ) ) ? $"{iso_country}," : ",";
      ret += ( !string.IsNullOrEmpty( iso_region ) ) ? $"{iso_region}," : ",";
      ret += ( !string.IsNullOrEmpty( lat ) ) ? $"{lat}," : ",";
      ret += ( !string.IsNullOrEmpty( lon ) ) ? $"{lon}," : ",";
      ret += ( !string.IsNullOrEmpty( elevation ) ) ? $"{elevation}," : ",";
      ret += ( !string.IsNullOrEmpty( apt_type ) ) ? $"{apt_type}," : ",";
      ret += ( !string.IsNullOrEmpty( apt_name ) ) ? $"{apt_name}," : ",";
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      return ret;
    }


    private Dictionary<string, string> m_symbols = new Dictionary<string, string>( ) {
            { "heliport", $"\"marker-color\": \"#626586\", \"marker-size\": \"small\", \"marker-symbol\": \"heliport\""}, // greyish
            { "small_airport", $"\"marker-color\": \"#2de13a\", \"marker-size\": \"small\", \"marker-symbol\": \"airfield\""},// greenish
            { "closed", $"\"marker-color\": \"#919293\", \"marker-size\": \"small\", \"marker-symbol\": \"cross\""},// grey
            { "seaplane_base", $"\"marker-color\": \"#458ade\", \"marker-size\": \"medium\", \"marker-symbol\": \"wetland\""},// blueish
            { "balloonport", $"\"marker-color\": \"#c8b422\", \"marker-size\": \"small\", \"marker-symbol\": \"america-football\""}, // yellowish
            { "medium_airport", $"\"marker-color\": \"#a77c84\", \"marker-size\": \"medium\", \"marker-symbol\": \"airport\""},// redish
            { "large_airport", $"\"marker-color\": \"#626586\", \"marker-size\": \"large\", \"marker-symbol\": \"airport\"" } }; // greyish

    private AptTypes TypeAsEnum
    {
      get {
        switch ( this.apt_type ) {
          case "heliport": return AptTypes.heliport;
          case "small_airport": return AptTypes.small_airport;
          case "medium_airport": return AptTypes.medium_airport;
          case "large_airport": return AptTypes.large_airport;
          case "balloonport": return AptTypes.balloonport;
          case "seaplane_base": return AptTypes.seaplane_base;
          case "closed": return AptTypes.closed;
          default: return AptTypes.Other;
        }
      }
    }


    /// <summary>
    /// Returns the content in GeoJson Notation
    /// </summary>
    /// <returns>The GeoJson database record</returns>
    public string AsGeoJson()
    {
      // Example:
      // { "type": "Feature", "properties": { "field_1": "ABERDEEN VOR\/DME", "field_2": "ADN", "field_3": 57.310555,
      //       "field_4": -2.267222 }, "geometry": { "type": "Point", "coordinates": [ -2.267222, 57.310555 ] } }
      string feature = $"\"type\":\"Feature\"";
      string props = $"\"properties\":{{{m_symbols[apt_type]},\"Name\":\"{apt_name}\",\"Ident\":\"{apt_icao_code}\",\"Lat\":{lat},\"Lon\":{lon}}}";
      string geo = $"\"geometry\":{{\"type\":\"Point\",\"coordinates\":[{lon},{lat}]}}";
      string ret = $"{{{feature},{props},{geo}}}";
      return ret;
    }


  }
}
