using System;
using System.Collections.Generic;
using System.Text;
using static d1090dataLib.d1090ext_aplib.apRec;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// The Airport database
  /// </summary>
  public class apDatabase
  {
    private static Dictionary<string, string> m_lookupICAO = new Dictionary<string, string>( );
    public static string GetICAOfromIATA( string iata )
    {
      string ret = iata;
      if ( m_lookupICAO.ContainsKey( iata ) ) return m_lookupICAO[iata];

      return ret;
    }
    public static void AddICAOfromIATA( string iata, string icao )
    {
      if ( iata.Length == 3 ) {
        if ( !m_lookupICAO.ContainsKey( iata ) )
          m_lookupICAO.Add( iata, icao );
      }
    }


    private apTable m_db = null;


    /// <summary>
    /// cTor: init the database
    /// </summary>
    public apDatabase()
    {
      m_db = new apTable( );
    }

    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( apRec rec )
    {
      if ( rec != null ) {
        if ( rec.apt_icao_code == "children" ) return ""; // get rid of special element

        AddICAOfromIATA( rec.apt_iata_code, rec.apt_icao_code ); // helper to lookup ICAOs from IATA
        return m_db.Add( rec );
      }
      return "";
    }

    /// <summary>
    /// Returns the number of records in the database
    /// </summary>
    public int Count
    {
      get {
        return m_db.Count;
      }
    }

    /// <summary>
    /// Return the complete table
    /// </summary>
    /// <returns>The complete table</returns>
    public apTable GetTable()
    {
      return m_db;
    }

    /// <summary>
    /// Returns a subtable with items that match the given criteria
    /// </summary>
    /// <param name="rangeLimitNm">Range Limit in nm</param>
    /// <param name="Lat">Center Lat (decimal)</param>
    /// <param name="Lon">Center Lon (decimal)</param>
    /// <param name="aptTypes">Type of airport items to include</param>
    /// <returns>A table with selected records</returns>
    public apTable GetSubtable( double rangeLimitNm, double Lat, double Lon, AptTypes[] aptTypes = null )
    {
      return m_db.GetSubtable( rangeLimitNm, Lat, Lon, aptTypes );
    }

  }
}
