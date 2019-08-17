using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Creates SQLite Insert Statements
  /// </summary>
  public class icaoSqlWriter
  {
    private const string PREFIXES = "0123456789ABCDEF";

    // write prefixed section
    // <returns>String as result either empty or error</returns>
    private static void WriteFile( SQLiteConnection sqConnection, icaoTable subTable )
    {
      using ( SQLiteCommand sqlite_cmd = sqConnection.CreateCommand( ) ) {
        foreach ( var rec in subTable ) {
          sqlite_cmd.CommandText = "INSERT INTO fa_modes (icao, registration,airctype,manufacturer,aircname,operator_)"
            + $" VALUES ('{rec.Value.icao}','{rec.Value.registration}','{rec.Value.airctype}','{rec.Value.manufacturer}','{rec.Value.airctypedesc}','{rec.Value.operator_}');";
          sqlite_cmd.ExecuteNonQuery( );
        }
      }
    }

    /// <summary>
    /// Write complete db as INSERT statements
    /// </summary>
    /// <param name="db">The icao db</param>
    /// <param name="csvOutStream"></param>
    /// <returns>String as result either empty or error</returns>
    public static string WriteSqDB( icaoDatabase db, SQLiteConnection sqConnection)
    {
      using ( SQLiteCommand sqlite_cmd = sqConnection.CreateCommand( ) ) {
        sqlite_cmd.CommandText = "BEGIN TRANSACTION;";
        sqlite_cmd.ExecuteNonQuery( );
        try {
          foreach ( var c in PREFIXES ) {
            WriteFile( sqConnection, db.GetSubtable( c.ToString( ) ) );
          }
          sqlite_cmd.CommandText = "COMMIT;";
          sqlite_cmd.ExecuteNonQuery( );
        }
        catch (SQLiteException sqex) {
          sqlite_cmd.CommandText = "ROLLBACK;";
          sqlite_cmd.ExecuteNonQuery( );
          return $"ERROR - inserting rows failed: {sqex.Message}\n";
        }

      }
      return "";
    }


  }
}
