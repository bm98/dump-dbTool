using System;
using System.Collections.Generic;
using System.Text;
using static d1090dataLib.xp11_navlib.navRec;

namespace d1090dataLib.xp11_navlib
{
  public class navDatabase
  {

    private navTable m_db = null;

    /// <summary>
    /// cTor: init the database
    /// </summary>
    public navDatabase()
    {
      m_db = new navTable( );
    }

    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( navRec rec )
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
    public navTable GetSubtable()
    {
      return m_db;
    }


    public navTable GetSubtable( double rangeLimitNm, double Lat, double Lon, NavTypes[] navTypes = null )
    {
      return m_db.GetSubtable( rangeLimitNm, Lat, Lon, navTypes );
    }


  }
}
