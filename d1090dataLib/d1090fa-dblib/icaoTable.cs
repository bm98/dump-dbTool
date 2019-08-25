using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using d1090dataLib.d1090ext_aclib;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// contains all icao (ModeS) records
  /// </summary>
  public class icaoTable : SortedDictionary<string, icaoRec>
  {
    private string m_prefix = "X";
    public string DbPrefix { get => m_prefix; set => m_prefix = value; }


    /// <summary>
    /// cTor: the prefix of the stored items
    /// </summary>
    /// <param name="prefix"></param>
    public icaoTable( string prefix )
    {
      m_prefix = prefix;
    }

    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="prefix">The prefix of the table</param>
    /// <param name="table">The source to fill from</param>
    public icaoTable( string prefix, icaoTable table )
    {
      m_prefix = prefix;
      this.AddSubtable( table );
    }

    /// <summary>
    /// Add one record to the table
    /// For existing records overwrite if data is provided (reg and type is expected to be delivered anyway)
    /// </summary>
    /// <param name="rec">The record to be added/updated</param>
    public string Add( icaoRec rec )
    {
      string ret = "";
      if ( rec != null ) {
        // sanity.. modeS ID must be 6 chars
        if ( rec.Icao.Length == 6 ) {
          if ( !this.ContainsKey( rec.Icao ) ) {
            this.Add( rec.Icao, rec );
          }
          else {
            // For existing records overwrite if data is provided (reg and type is expected to be delivered anyway)
            this[rec.Icao].Registration = rec.Registration;
            this[rec.Icao].AircTypeCode = rec.AircTypeCode;
            this[rec.Icao].ManufacturerName = string.IsNullOrEmpty( rec.ManufacturerName ) ? this[rec.Icao].ManufacturerName : rec.ManufacturerName;
            this[rec.Icao].AircTypeName = string.IsNullOrEmpty( rec.AircTypeName ) ? this[rec.Icao].AircTypeName : rec.AircTypeName;
            this[rec.Icao].OperatorName = string.IsNullOrEmpty( rec.OperatorName ) ? this[rec.Icao].OperatorName : rec.OperatorName;
          }
        }
      }
      return ret;
    }

    /// <summary>
    /// Update the table with Aircraft DB data
    /// </summary>
    /// <param name="acT"></param>
    public void UpdateTable( acTable acT )
    {
      foreach ( var ac in acT ) {
        if ( !this.ContainsKey( ac.Key ) ) {
          // add
          var rec = new icaoRec( ac.Value.icao_code, ac.Value.regid, ac.Value.model );
          this.Add( rec );
        }
        this[ac.Key].Update( ac.Value );
      }
    }

    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( icaoTable subtable )
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
    private string AddSubtable( IEnumerable<KeyValuePair<string, icaoRec>> selection )
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
    public icaoTable GetSubtable( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return null;
      string dbPrefix = icaoPrefix[0].ToString( );

      var selection = this.Where( x => x.Key.Substring( 0, icaoPrefix.Length ) == icaoPrefix );
      var subtable = new icaoTable( icaoPrefix );
      subtable.AddSubtable( selection );
      return subtable;
    }

    /// <summary>
    /// Returns the number of entries matching a ICAO prefix
    /// </summary>
    /// <param name="icaoPrefix">The leading part of the ICAO key</param>
    /// <returns>The number of entries</returns>
    public int GetSubtableCount( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return 0;
      string dbPrefix = icaoPrefix[0].ToString( );

      return this.Where( x => x.Key.Substring( 0, icaoPrefix.Length ) == icaoPrefix ).Count( ); ;
    }


  }
}
