﻿using System;
using System.Collections.Generic;
using System.Text;

namespace d1090dataLib.d1090ext_rtlib
{
  /// <summary>
  /// The Route database
  /// </summary>
  public class rtDatabase
  {
    private const string m_PREFIX = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // Valid Hex string Name prefixes
    // number index by prefix character translation table
    private readonly byte[] m_INDEX = new byte[] {0,1,2,3,4,5,6,7,8,9, // 0..9
                                                  0,0,0,0,0,0,0,
                                                  10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35}; // A..Z
    private int dbIndex( char dbPrefix ) { return m_INDEX[dbPrefix - m_PREFIX[0]]; }

    // Array of route subtables (prefixed)
    private rtTable[] m_db = null;

    /// <summary>
    /// cTor: init the database
    /// </summary>
    public rtDatabase()
    {
      m_db = new rtTable[m_PREFIX.Length];
      for ( int i = 0; i < m_PREFIX.Length; i++ ) {
        m_db[i] = new rtTable( m_PREFIX[i].ToString( ) );
      }
    }

    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec">Record to add</param>
    public string Add( rtRec rec )
    {
      if ( rec != null ) {
        if ( rec.flight_code == "children" ) return ""; // get rid of special element

        char dbPrefix = rec.flight_code[0];
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
        for ( int i = 0; i < m_PREFIX.Length; i++ ) {
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
    public rtTable GetSubtable( string icaoPrefix )
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

      return m_db[dbIndex( dbPrefix )].GetSubtableEntries( icaoPrefix );
    }

  }
}
