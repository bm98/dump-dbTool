using System;
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

    private void WriteFile( StreamWriter sw, icaoTable subTable )
    {
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsCsv( ) );
      }
    }


    public bool WriteCsv( icaoDatabase db, Stream csvOutStream )
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
