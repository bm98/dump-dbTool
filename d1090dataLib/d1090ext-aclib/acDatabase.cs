using System;
using System.Collections.Generic;
using System.Text;

namespace d1090dataLib.d1090ext_aclib
{
  /// <summary>
  /// The icao Aircraft database (our own format - may be no longer used)
  /// Handles the FA db with ModeS prefix for their Json file database
  /// </summary>
  public class acDatabase
  {
    private const string m_PREFIX = "0123456789ABCDEF"; // Valid Hex string ModeS prefixes
    // number index by prefix character translation table
    private readonly byte[] m_INDEX = new byte[] {0,1,2,3,4,5,6,7,8,9, // 0..9
                                                  0,0,0,0,0,0,0,
                                                  10,11,12,13,14,15}; // A..F
    private int dbIndex( char dbPrefix ) { return m_INDEX[dbPrefix - m_PREFIX[0]]; }

    // Array of aircraft subtables (prefixed)
    private acTable[] m_db = null;

    /// <summary>
    /// cTor: init the database
    /// </summary>
    public acDatabase()
    {
      m_db = new acTable[m_PREFIX.Length];
      for ( int i = 0; i < m_PREFIX.Length; i++ ) {
        m_db[i] = new acTable( m_PREFIX[i].ToString( ) );
      }
    }

    /// <summary>
    /// Add one record to the database
    /// </summary>
    /// <param name="rec">A new record to add</param>
    public string Add( acRec rec )
    {
      if ( rec != null ) {
        if ( rec.icao_code == "children" ) return ""; // get rid of special element

        char dbPrefix = rec.icao_code[0];
        return m_db[dbIndex( dbPrefix )].Add( rec );
      }
      return "";
    }

    /// <summary>
    /// Returns the number of records in the database
    /// </summary>
    public int Count
    {
      get {
        int cnt = 0;
        for ( int i = 0; i < 16; i++ ) {
          cnt += m_db[i].Count;
        }
        return cnt;
      }
    }

    /// <summary>
    /// Returns a subtable of where the key starts with the given argument
    /// </summary>
    /// <param name="icaoPrefix">The leading part of the ICAO key</param>
    /// <returns>A subtable of entries</returns>
    public acTable GetSubtable( string icaoPrefix )
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
    public int GetSubtableEntries( string icaoPrefix )
    {
      if ( string.IsNullOrEmpty( icaoPrefix ) ) return 0;
      char dbPrefix = icaoPrefix[0];

      return m_db[dbIndex( dbPrefix )].GetSubtableCount( icaoPrefix );
    }


  }
}
