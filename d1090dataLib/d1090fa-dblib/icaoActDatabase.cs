using System;
using System.Collections.Generic;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// The icao type designation database
  /// </summary>
  public class icaoActDatabase
  {
    private icaoActTable m_db = null;

    /// <summary>
    /// cTor: init the database
    /// </summary>
    public icaoActDatabase()
    {
      m_db = new icaoActTable( );
    }

    /// <summary>
    /// Add one record to the database
    /// </summary>
    /// <param name="rec">A new record</param>
    public string Add( icaoActRec rec )
    {
      if ( rec != null ) {
        return m_db.Add( rec );
      }
      return "";
    }

    /// <summary>
    /// Return the number of entries in the database
    /// </summary>
    public int Count { get {return m_db.Count;} }

    /// <summary>
    /// Return the db table
    /// </summary>
    /// <returns>The db Table</returns>
    public icaoActTable GetTable()
    {
      return m_db;
    }


  }
}
