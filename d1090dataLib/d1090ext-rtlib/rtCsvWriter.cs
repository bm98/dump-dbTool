using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_rtlib
{
  public class rtCsvWriter
  {
    const string PREFIXES = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private void WriteFile( StreamWriter sw, rtTable subTable )
    {
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsCsv( ) );
      }
    }


    public bool WriteCsv( rtDatabase db, Stream csvOutStream )
    {
      using ( var sw = new StreamWriter( csvOutStream, Encoding.UTF8 ) ) {
        sw.WriteLine( rtRec.CsvHeader );
        foreach ( var c in PREFIXES ) {
          WriteFile( sw, db.GetSubtable( c.ToString( ) ) );
        }
      }
      return true;
    }

  }
}
