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
    private void WriteFile( StreamWriter sw, apTable subTable )
    {
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsCsv( ) );
      }
    }


    public bool WriteCsv( apDatabase db, Stream csvOutStream )
    {
      using ( var sw = new StreamWriter( csvOutStream , Encoding.UTF8 ) ) {
        sw.WriteLine( apRec.CsvHeader );
        WriteFile( sw, db.GetSubtable(  ) );
      }
      return true;
    }

  }
}
