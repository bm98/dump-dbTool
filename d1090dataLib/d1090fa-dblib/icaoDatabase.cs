using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using d1090dataLib.d1090ext_aclib;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// The icao database
  /// </summary>
  public class icaoDatabase
  {
    private const string m_PREFIX = "0123456789ABCDEF"; // Valid Hex string ModeS prefixes
    // number index by prefix character translation table
    private readonly byte[] m_INDEX = new byte[] {0,1,2,3,4,5,6,7,8,9, // 0..9
                                                  0,0,0,0,0,0,0,
                                                  10,11,12,13,14,15}; // A..F
    private int dbIndex( char dbPrefix ) { return m_INDEX[dbPrefix - m_PREFIX[0]]; } // UCase HEX Prefix only

    // Array of modeS subtables (prefixed)
    private icaoTable[] m_db = null;

    /// <summary>
    /// cTor: init the database
    /// </summary>
    public icaoDatabase()
    {
      m_db = new icaoTable[m_PREFIX.Length];
      for ( int i = 0; i < m_PREFIX.Length; i++ ) {
        m_db[i] = new icaoTable( m_PREFIX[i].ToString( ) );
      }
    }

    /// <summary>
    /// Add one record to the database
    /// </summary>
    /// <param name="rec">A new record to add</param>
    public string Add( icaoRec rec )
    {
      if ( rec != null ) {
        if ( rec.Icao == "children" ) return ""; // get rid of special element

        char dbPrefix = rec.Icao[0];
        return m_db[dbIndex( dbPrefix )].Add( rec );
      }
      return "";
    }

    /// <summary>
    /// Return the number of entries in the database
    /// </summary>
    public int Count
    {
      get {
        int cnt = 0;
        for ( int i = 0; i < m_PREFIX.Length; i++ ) {
          cnt += m_db[i].Count;
        }
        return cnt;
      }
    }

    /// <summary>
    /// Update this database with values from the Aircraft database
    /// </summary>
    /// <param name="adb"></param>
    public void Update( acDatabase adb)
    {
      for ( int i = 0; i < m_PREFIX.Length; i++ ) {
        char dbPrefix = m_PREFIX[i];
        var acT = adb.GetSubtable( dbPrefix.ToString( ) );
        m_db[dbIndex( dbPrefix )].UpdateTable( acT );
      }
    }


    /* NOT USED SO FAR
    /// <summary>
    /// Adds a table to this database (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    /// <returns>Information about the add process</returns>
    public string AddSubtable( icaoTable subtable )
    {
      char dbPrefix = subtable.DbPrefix[0];
      return m_db[dbIndex( dbPrefix )].AddSubtable( subtable );
    }
    */

    /// <summary>
    /// Returns a subtable of where the key starts with the given argument
    /// </summary>
    /// <param name="icaoPrefix">The leading part of the ICAO key</param>
    /// <returns>A subtable of entries</returns>
    public icaoTable GetSubtable( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return null;

      char dbPrefix = icaoPrefix[0];
      return m_db[dbIndex( dbPrefix )].GetSubtable( icaoPrefix );
    }

    /// <summary>
    /// Returns the number of entries matching a ICAO prefix
    /// </summary>
    /// <param name="icaoPrefix">The leading part of the ICAO key</param>
    /// <returns>The number of entries</returns>
    public int GetSubtableCount( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return 0;
      char dbPrefix = icaoPrefix[0];

      return m_db[dbIndex( dbPrefix )].GetSubtableCount( icaoPrefix );
    }

  }
}
