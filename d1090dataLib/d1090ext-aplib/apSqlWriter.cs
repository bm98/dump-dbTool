using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// Creates SQLite Insert Statements for Airport records
  /// Assumes the db contains a table 'airports (apt_icao_code, apt_iata_code, iso_country, iso_region, lat, lon, elevation, apt_type, apt_name)'
  /// 
  /// </summary>
  public class apSqlWriter
  {

    /// <summary>
    /// Exec INSERT statements for an airport subtable
    /// </summary>
    /// <param name="sqConnection">The db connection</param>
    /// <param name="subTable">The subtable to dump</param>
    /// <returns>The result string, either empty or error</returns>
    private static string WriteFile( SQLiteConnection sqConnection, apTable subTable )
    {
      using ( SQLiteCommand sqlite_cmd = sqConnection.CreateCommand( ) ) {
        foreach ( var rec in subTable ) {
          try {
            sqlite_cmd.CommandText = "INSERT INTO airports (apt_icao_code, apt_iata_code, iso_country, iso_region, lat, lon, elevation, apt_type, apt_name)"
            + $" VALUES ('{rec.Value.apt_icao_code}','{rec.Value.apt_iata_code}','{rec.Value.iso_country}'" 
            + $",'{rec.Value.iso_region}','{rec.Value.lat}','{rec.Value.lon}','{rec.Value.elevation}'"
            + $",'{rec.Value.apt_type}','{rec.Value.apt_name}');";
          sqlite_cmd.ExecuteNonQuery( );
          }
          catch ( SQLiteException sqex ) {
            return $"ERROR - writing route: {sqex.Message}\n";
          }
        }
      }
      return "";
    }


    /// <summary>
    /// Write complete airport db into the supplied database as one transaction
    /// </summary>
    /// <param name="db">The airport db to dump</param>
    /// <param name="sqConnection">The db connection</param>
    /// <returns>The result string, either empty or error</returns>
    public static string WriteSqDB( apDatabase db, SQLiteConnection sqConnection )
    {
      string ret = "";
      using ( SQLiteCommand sqlite_cmd = sqConnection.CreateCommand( ) ) {
        sqlite_cmd.CommandText = "BEGIN TRANSACTION;";
        sqlite_cmd.ExecuteNonQuery( );
        try {
          ret = WriteFile( sqConnection, db.GetTable( ) );
          sqlite_cmd.CommandText = "COMMIT;";
          sqlite_cmd.ExecuteNonQuery( );
        }
        catch {
          sqlite_cmd.CommandText = "ROLLBACK;";
          sqlite_cmd.ExecuteNonQuery( );
        }
        finally {
          if ( !string.IsNullOrEmpty( ret ) ) {
            ret = $"ERROR - inserting rows failed: {ret}\n";
          }
        }
      }
      return ret;
    }

  }
}
