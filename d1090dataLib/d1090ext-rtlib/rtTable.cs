using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace d1090dataLib.d1090ext_rtlib
{
  /// <summary>
  /// contains all icao route records
  /// </summary>
  public class rtTable : SortedDictionary<string, rtRec>
  {
    private string m_prefix = "X";
    public string DbPrefix { get => m_prefix; set => m_prefix = value; }


    /// <summary>
    /// cTor: the prefix of the stored items
    /// </summary>
    /// <param name="prefix"></param>
    public rtTable( string prefix )
    {
      m_prefix = prefix;
    }


    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="prefix">The prefix of the table</param>
    /// <param name="table">The source to fill from</param>
    public rtTable( string prefix, rtTable table )
    {
      m_prefix = prefix;
      this.AddSubtable( table );
    }


    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( rtRec rec )
    {
      string ret = "";
      if ( rec != null ) {
        if ( rec.flight_code.Length > 4 ) {
          if ( !this.ContainsKey( rec.flight_code ) ) {
            this.Add( rec.flight_code, rec );
          }
          else {
            // We don't overwrite existing ones
            // (I found some dupes located at the end are wrong)
            if ( string.IsNullOrEmpty( this[rec.flight_code].from_apt_icao ) ) {
              this[rec.flight_code].from_apt_icao = rec.from_apt_icao;
              this[rec.flight_code].to_apt_icao = rec.to_apt_icao;
            }
          }
        }
      }
      return ret;
    }


    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( rtTable subtable )
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

    private string AddSubtable( IEnumerable<KeyValuePair<string, rtRec>> selection )
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
    /// Returns a subtable of where the key starts with the given argument
    /// </summary>
    /// <param name="icaoPrefix">The leading part of the ICAO key</param>
    /// <returns>A subtable of entries</returns>
    public rtTable GetSubtable( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return null;
      string dbPrefix = icaoPrefix[0].ToString( );

      var selection = this.Where( x => x.Key.Substring( 0, icaoPrefix.Length ) == icaoPrefix );
      var subtable = new rtTable( icaoPrefix );
      subtable.AddSubtable( selection );
      return subtable;
    }

    /// <summary>
    /// Returns the number of entries matching a ICAO prefix
    /// </summary>
    /// <param name="icaoPrefix">The leading part of the ICAO key</param>
    /// <returns>The number of entries</returns>
    public int GetSubtableEntries( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return 0;
      string dbPrefix = icaoPrefix[0].ToString( );

      return this.Where( x => x.Key.Substring( 0, icaoPrefix.Length ) == icaoPrefix ).Count( ); ;
    }


  }
}
