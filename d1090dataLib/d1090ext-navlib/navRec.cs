using System;
using System.Collections.Generic;
using System.Linq;

namespace d1090dataLib.d1090ext_navlib
{
  /// <summary>
  /// A Navaid Record
  /// </summary>
  public class navRec
  {
    public enum NavTypes
    {
      All = -1,
      Other = 0,
      NDB,
      DME,
      NDB_DME,
      VOR_DME,
      TACAN,
      VORTAC,
      VOR,
    }

    /*    
     *    "id","filename","ident","name","type","frequency_khz","latitude_deg","longitude_deg","elevation_ft","iso_country",
     *    "dme_frequency_khz","dme_channel","dme_latitude_deg","dme_longitude_deg","dme_elevation_ft","slaved_variation_deg","magnetic_variation_deg",
     *    "usageType","power","associated_airport",
    */
    // fields in db
    public string ident = "";
    public string nametype = ""; // filename
    public string type = "";
    public string name = "";
    public string freq_kHz = "";
    public string lat = "";
    public string lon = "";
    public string elevation = "";
    public string iso_country = "";
    public string dme_freq_kHz = "";
    public string dme_channel = "";
    public string dme_lat = "";
    public string dme_lon = "";
    public string dme_elevation = "";

    public string usageType = "";
    public string assoc_icaoApt = "";

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="id">ident (UPPERCASE)</param>
    public navRec( string id, string nt, string t, string n, string f, string la, string lo, string elev, string c,
                    string df, string dc, string dla, string dlo, string delev,
                    string ut, string aApt )
    {
      ident = id.ToUpperInvariant( );
      nametype = nt;
      type = t.ToUpperInvariant( );
      name = n;
      freq_kHz = f;
      lat = la;
      lon = lo;
      elevation = elev;
      iso_country = c;
      dme_freq_kHz = df;
      dme_channel = dc;
      dme_lat = dla;
      dme_lon = dlo;
      dme_elevation = delev;
      usageType = ut;
      assoc_icaoApt = aApt;
    }

    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => ( !string.IsNullOrEmpty( ident ) ); }

    /// <summary>
    /// Returns true if the record matches one of the given types
    /// </summary>
    /// <param name="navTypes">An array of NavTypes</param>
    /// <returns>True for a match, else false</returns>
    public bool IsTypeOf( NavTypes[] navTypes )
    {
      if ( navTypes.Contains( NavTypes.All ) ) return true;
      return navTypes.Contains( TypeAsEnum );
    }

    // GeoJson stuff
    // string propNav = $"\"properties\":{{\"marker-color\": \"#ff0000\", \"marker-size\": \"medium\", \"marker-symbol\": \"triangle-stroked\"}},";
    // private string symbols = "airport,square-stroked,triangle-stroked,star-stroked,circle-stroked";

    private Dictionary<string, string> m_symbols = new Dictionary<string, string>( ) {
            { "NDB", $"\"marker-color\": \"#0000ff\", \"marker-size\": \"medium\", \"marker-symbol\": \"circle\""}, // blue
            { "DME", $"\"marker-color\": \"#ff00ff\", \"marker-size\": \"medium\", \"marker-symbol\": \"triangle\""},// magenta
            { "NDB-DME", $"\"marker-color\": \"#ff0000\", \"marker-size\": \"medium\", \"marker-symbol\": \"circle-stroked\""},// orange
            { "VOR-DME", $"\"marker-color\": \"#e12713\", \"marker-size\": \"medium\", \"marker-symbol\": \"square-stroked\""},// red
            { "TACAN", $"\"marker-color\": \"#ff8000\", \"marker-size\": \"medium\", \"marker-symbol\": \"star\""}, // orange
            { "VORTAC", $"\"marker-color\": \"#ff8000\", \"marker-size\": \"medium\", \"marker-symbol\": \"star-stroked\""},// orange
            { "VOR", $"\"marker-color\": \"#e12713\", \"marker-size\": \"medium\", \"marker-symbol\": \"square\"" } }; // red


    private NavTypes TypeAsEnum
    {
      get {
        switch ( this.type ) {
          case "NDB": return NavTypes.NDB;
          case "DME": return NavTypes.DME;
          case "NDB-DME": return NavTypes.NDB_DME;
          case "VOR-DME": return NavTypes.VOR_DME;
          case "TACAN": return NavTypes.TACAN;
          case "VORTAC": return NavTypes.VORTAC;
          case "VOR": return NavTypes.VOR;
          default: return NavTypes.Other;
        }
      }
    }



    /// <summary>
    /// Returns the content in Csv Notation
    /// </summary>
    /// <returns>The CSV database record</returns>
    public string AsCsv()
    {
      string ret = $"{ident},";
      ret += ( !string.IsNullOrEmpty( nametype ) ) ? $"{nametype}," : ",";
      ret += ( !string.IsNullOrEmpty( type ) ) ? $"{type}," : ",";
      ret += ( !string.IsNullOrEmpty( name ) ) ? $"{name}," : ",";
      ret += ( !string.IsNullOrEmpty( freq_kHz ) ) ? $"{freq_kHz}," : ",";
      ret += ( !string.IsNullOrEmpty( lat ) ) ? $"{lat}," : ",";
      ret += ( !string.IsNullOrEmpty( lon ) ) ? $"{lon}," : ",";
      ret += ( !string.IsNullOrEmpty( elevation ) ) ? $"{elevation}," : ",";
      ret += ( !string.IsNullOrEmpty( iso_country ) ) ? $"{iso_country}," : ",";
      ret += ( !string.IsNullOrEmpty( dme_freq_kHz ) ) ? $"{dme_freq_kHz}," : ",";
      ret += ( !string.IsNullOrEmpty( dme_channel ) ) ? $"{dme_channel}," : ",";
      ret += ( !string.IsNullOrEmpty( dme_lat ) ) ? $"{dme_lat}," : ",";
      ret += ( !string.IsNullOrEmpty( dme_lon ) ) ? $"{dme_lon}," : ",";
      ret += ( !string.IsNullOrEmpty( dme_elevation ) ) ? $"{dme_elevation}," : ",";
      ret += ( !string.IsNullOrEmpty( usageType ) ) ? $"{usageType}," : ",";
      ret += ( !string.IsNullOrEmpty( assoc_icaoApt ) ) ? $"{assoc_icaoApt}," : ",";
      if ( ret.EndsWith( "," ) )
        ret = ret.Substring( 0, ret.Length - 1 ); // remove last comma
      return ret;
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
      string props = $"\"properties\":{{{m_symbols[type]},\"Name\":\"{nametype}\",\"Ident\":\"{ident}\",\"Lat\":{lat},\"Lon\":{lon}}}";
      string geo = $"\"geometry\":{{\"type\":\"Point\",\"coordinates\":[{lon},{lat}]}}";
      string ret = $"{{{feature},{props},{geo}}}";
      return ret;
    }

  }
}
