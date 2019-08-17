using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace d1090dataLib.d1090ext_aclib
{
  /// <summary>
  /// contains all icao aircraft records
  /// </summary>
  public class acTable : SortedDictionary<string, acRec>
  {
    private string m_prefix = "X";
    public string DbPrefix { get => m_prefix; set => m_prefix = value; }



    /// <summary>
    /// cTor: the prefix of the stored items
    /// </summary>
    /// <param name="prefix"></param>
    public acTable( string prefix )
    {
      m_prefix = prefix;
    }


    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="prefix">The prefix of the table</param>
    /// <param name="table">The source to fill from</param>
    public acTable( string prefix, acTable table )
    {
      m_prefix = prefix;
      this.AddSubtable( table );
    }


    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( acRec rec )
    {
      string ret = "";
      if ( rec != null ) {
        if ( rec.icao_code.Length == 6 ) {
          if ( !this.ContainsKey( rec.icao_code ) ) {
            this.Add( rec.icao_code, rec );
          }
          else {
            /*
              if ( this[rec.icao].regName != rec.regName )
                ret += $"reg: old:{this[rec.icao].regName}, new: {rec.regName},";
              if ( this[rec.icao].airctype != rec.airctype )
                ret += $"airc: old:{this[rec.icao].airctype}, new: {rec.airctype},";
              if ( !string.IsNullOrEmpty( ret ) )
                ret = $"existing:{this[rec.icao].icao}," + ret + $"\n";
            */
            this[rec.icao_code].regid = rec.regid;
            this[rec.icao_code].model = rec.model;
            this[rec.icao_code].typedesc = rec.typedesc;
            this[rec.icao_code].operator_ = rec.operator_;
          }
        }
      }
      return ret;
    }


    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( acTable subtable )
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

    private string AddSubtable( IEnumerable<KeyValuePair<string, acRec>> selection )
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
    public acTable GetSubtable( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return null;
      string dbPrefix = icaoPrefix[0].ToString( );

      var selection = this.Where( x => x.Key.Substring( 0, icaoPrefix.Length ) == icaoPrefix );
      var subtable = new acTable( icaoPrefix );
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
