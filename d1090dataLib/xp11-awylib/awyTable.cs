using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace d1090dataLib.xp11_awylib
{
  /// <summary>
  ///  key: start_reg_end_reg
  /// </summary>
  public class awyTable : Dictionary<string, awyRec>
  {

    public awyTable()
    {
    }

    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="prefix">The prefix of the table</param>
    /// <param name="table">The source to fill from</param>
    public awyTable( awyTable table )
    {
      this.AddSubtable( table );
    }


    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( awyRec rec )
    {
      string ret = "";
      if ( rec != null ) {
        if ( rec.ident.Length >= 5 ) {
          if ( !this.ContainsKey( rec.ident ) ) {
            this.Add( rec.ident, rec );
          }
          else {
            // overwite ?? NO 
          }
        }
      }
      return ret;
    }


    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( awyTable subtable )
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


    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="selection">Enumerated Key Value pairs to add to this table</param>
    private string AddSubtable( IEnumerable<KeyValuePair<string, awyRec>> selection )
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

    /// <summary>
    /// Return an Airway subtable where either start or end ICAO designator matches
    /// </summary>
    /// <param name="icao_key">The icao to match</param>
    /// <returns>An awyTable</returns>
    public awyTable GetSubtable( string icao_key )
    {
      var nT = new awyTable( );
      foreach ( var rec in this ) {
        // key = ident => "icao_region_icao_region"  (so find is Contains(icao), which is expensive...)
        if ( rec.Key.Contains( icao_key ) ) {
          nT.Add( rec.Value );
        }
      }
      return nT;
    }

    /// <summary>
    /// Return a sorted Airway subtable where either start or end ICAO designator matches
    /// </summary>
    /// <param name="icao_key">The icao to match</param>
    /// <returns>A sorted awyTable</returns>
    public awyTable GetSortedSubtable( string icao_key )
    {
      var l = GetSubtable( icao_key ).OrderBy( key => key.Key );
      return ( l.ToDictionary( ( keyItem ) => keyItem.Key, ( valueItem ) => valueItem.Value ) as awyTable );
    }


  }
}

