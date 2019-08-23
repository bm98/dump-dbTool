﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Writes the database to a CSV file
  /// </summary>
  public class icaoCsvWriter
  {

    const string PREFIXES = "0123456789ABCDEF";

    /// <summary>
    /// Writes one file from the given sub table
    /// </summary>
    /// <param name="sw">Stream to write to</param>
    /// <param name="subTable">The subtable to write out</param>
    private static void WriteFile( StreamWriter sw, icaoTable subTable )
    {
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsCsv( ) );
      }
    }


    /// <summary>
    /// Write the ModeS db as CSV formatted file
    /// </summary>
    /// <param name="db">The database to dump</param>
    /// <param name="csvOutStream">The stream to write to</param>
    /// <returns>True for success</returns>
    public static bool WriteCsv( icaoDatabase db, Stream csvOutStream )
    {
      using ( var sw = new StreamWriter( csvOutStream, Encoding.UTF8 ) ) {
        sw.WriteLine( icaoRec.CsvHeader );
        foreach ( var c in PREFIXES ) {
          WriteFile( sw, db.GetSubtable( c.ToString( ) ) );
        }
      }
      return true;
    }

  }
}
