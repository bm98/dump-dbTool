using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using d1090dataLib.d1090ext_coordlib;
using static d1090dataLib.d1090ext_aplib.apRec;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// contains all icao airport records
  /// </summary>
  public class apTable : SortedDictionary<string, apRec>
  {
    /// <summary>
    /// cTor: the prefix of the stored items
    /// </summary>
    /// <param name="prefix"></param>
    public apTable()
    {
    }


    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="table">The source to fill from</param>
    public apTable( apTable table )
    {
      this.AddSubtable( table );
    }


    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( apRec rec )
    {
      string ret = "";
      // sanity checks... (we have seen crap here..)
      if ( !decimal.TryParse( rec.lat, out decimal testLat ) ) return "";
      if ( !decimal.TryParse( rec.lon, out decimal testLon ) ) return "";

      if ( rec != null ) {
        if ( rec.apt_icao_code.Length == 4 ) {
          if ( !this.ContainsKey( rec.apt_icao_code ) ) {
            this.Add( rec.apt_icao_code, rec );
          }
          else {
            this[rec.apt_icao_code].apt_iata_code = rec.apt_iata_code;
            this[rec.apt_icao_code].iso_country = rec.iso_country;
            this[rec.apt_icao_code].iso_region = rec.iso_region;
            this[rec.apt_icao_code].lat = rec.lat;
            this[rec.apt_icao_code].lon = rec.lon;
            this[rec.apt_icao_code].elevation = rec.elevation;
            this[rec.apt_icao_code].apt_type = rec.apt_type;
            this[rec.apt_icao_code].apt_name = rec.apt_name.Replace( $"\"", "" ); // dQuotes intimidates geoJson..
          }
        }
      }
      return ret;
    }


    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( apTable subtable )
    {
      string ret = "";
      foreach ( var rec in subtable ) {
        try {
          ret += this.Add( rec.Value );
        }
        catch { }
      }
      return ret;
    }

    private string AddSubtable( IEnumerable<KeyValuePair<string, apRec>> selection )
    {
      string ret = "";
      foreach ( var rec in selection ) {
        try {
          ret += this.Add( rec.Value );
        }
        catch { }
      }
      return ret;
    }


    public apTable GetSubtable( double rangeLimitNm, double Lat, double Lon, AptTypes[] aptTypes = null )
    {
      if ( aptTypes == null ) aptTypes = new AptTypes[] { AptTypes.All };

      var nT = new apTable( );
      var myLoc = new LatLon( Lat, Lon );
      foreach ( var rec in this ) {
        var dist = myLoc.DistanceTo( new LatLon( double.Parse( rec.Value.lat ), double.Parse( rec.Value.lon ) ), ConvConsts.EarthRadiusNm );
        if ( ( dist <= rangeLimitNm ) && ( rec.Value.IsTypeOf( aptTypes ) ) ) {
          nT.Add( rec.Value );
        }
      }
      return nT;
    }

  }
}
