using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace d1090dataLib.xp11_awylib
{
  public class awyDatabase
  {

    private awyTable m_db = null;

    /// <summary>
    /// cTor: init the database
    /// </summary>
    public awyDatabase()
    {
      m_db = new awyTable( );
    }

    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( awyRec rec )
    {
      if ( rec != null ) {
        return m_db.Add( rec );
      }
      return "";
    }

    public int Count
    {
      get {
        return m_db.Count;
      }
    }

    /// <summary>
    /// Return the complete table
    /// </summary>
    /// <returns></returns>
    public awyTable GetSubtable()
    {
      return m_db;
    }

    public awyTable GetSubtable( string icao_key )
    {
      return m_db.GetSubtable( icao_key );
    }

    public awyTable GetSortedSubtable( string icao_key )
    {
      return m_db.GetSortedSubtable( icao_key );
    }

  }
}
