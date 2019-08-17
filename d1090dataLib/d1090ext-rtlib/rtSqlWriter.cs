using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace d1090dataLib.d1090ext_rtlib
{
  /// <summary>
  /// Creates SQLite Insert Statements
  /// </summary>
  public class rtSqlWriter
  {
    private const string PREFIXES = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static string WriteFile( SQLiteConnection sqConnection, rtTable subTable )
    {
      using ( SQLiteCommand sqlite_cmd = sqConnection.CreateCommand( ) ) {
        foreach ( var rec in subTable ) {
          try {
            sqlite_cmd.CommandText = "INSERT INTO routes (flight_code, from_apt_icao, to_apt_icao)"
              + $" VALUES ('{rec.Value.flight_code}','{rec.Value.from_apt_icao}','{rec.Value.to_apt_icao}');";
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
    /// Write complete db as INSERT statements
    /// </summary>
    /// <param name="db">The airport db</param>
    /// <returns>String as result either empty or error</returns>
    public static string WriteSqDB( rtDatabase db, SQLiteConnection sqConnection )
    {
      string ret = "";
      using ( SQLiteCommand sqlite_cmd = sqConnection.CreateCommand( ) ) {
        sqlite_cmd.CommandText = "BEGIN TRANSACTION;";
        sqlite_cmd.ExecuteNonQuery( );
        try {
          foreach ( var c in PREFIXES ) {
            ret = WriteFile( sqConnection, db.GetSubtable( c.ToString( ) ) );
          }
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
