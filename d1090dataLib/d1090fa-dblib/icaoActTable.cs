using System;
using System.Collections.Generic;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// contains all icao records
  /// </summary>
  public class icaoActTable : SortedDictionary<string, icaoActRec>
  {

    /// <summary>
    /// cTor: empty
    /// </summary>
    public icaoActTable(  )
    {
    }

    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="table">The source to fill from</param>
    public icaoActTable( icaoActTable table )
    {
      this.AddSubtable( table );
    }


    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( icaoActRec rec )
    {
      string ret = "";
      if ( rec != null ) {
        if ( !this.ContainsKey( rec.icaotype ) ) {
          this.Add( rec.icaotype, rec );
        }
        else {
          //update existing
          this[rec.icaotype].description = rec.description;
          this[rec.icaotype].wtccode = rec.wtccode;
        }
      }
      return ret;
    }

    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( icaoActTable subtable )
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
    private string AddSubtable( IEnumerable<KeyValuePair<string, icaoActRec>> selection )
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

  }
}
