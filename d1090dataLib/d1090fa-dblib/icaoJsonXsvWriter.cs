using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Writes the records as Json XSV files where the dividing X is a strange symbol (¦) to allow for columns...
  /// (may be not used anymore..)
  /// </summary>
  public class icaoJsonXsvWriter
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
        sw.WriteLine( rec.Value.AsJsonXsv( ) );
      }
    }


    /// <summary>
    /// Write the aircraft db as XSV formatted file
    /// </summary>
    /// <param name="db">The database to dump</param>
    /// <param name="csvOutStream">The stream to write to</param>
    /// <returns>True for success</returns>
    public static bool WriteCsv( icaoDatabase db, Stream jsonOutStream )
    {
      using ( var sw = new StreamWriter( jsonOutStream, Encoding.UTF8 ) ) {
        sw.WriteLine( icaoRec.JsonXsvHeader );
        foreach ( var c in PREFIXES ) {
          WriteFile( sw, db.GetSubtable( c.ToString( ) ) );
        }
      }
      return true;
    }

  }
}
