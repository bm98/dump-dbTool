using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// Writes the database to a CSV file
  /// </summary>
  public class apCsvWriter
  {
    /// <summary>
    /// Writes one file from the given sub table
    /// </summary>
    /// <param name="sw">Stream to write to</param>
    /// <param name="subTable">The subtable to write out</param>
    private static void WriteFile( StreamWriter sw, apTable subTable )
    {
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsCsv( ) );
      }
    }


    /// <summary>
    /// Write the airport db as CSV formatted file
    /// </summary>
    /// <param name="db">The database to dump</param>
    /// <param name="csvOutStream">The stream to write to</param>
    /// <returns>True for success</returns>
    public static bool WriteCsv( apDatabase db, Stream csvOutStream )
    {
      using ( var sw = new StreamWriter( csvOutStream , Encoding.UTF8 ) ) {
        sw.WriteLine( apRec.CsvHeader );
        WriteFile( sw, db.GetTable(  ) );
      }
      return true;
    }

  }
}
