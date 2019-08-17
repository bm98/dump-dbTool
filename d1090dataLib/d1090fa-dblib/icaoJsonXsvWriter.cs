using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Writes the records as Json XSV files where the dividing X is a strange symbol (¦) to allow for columns...
  /// </summary>
  public class icaoJsonXsvWriter
  {
    const string PREFIXES = "0123456789ABCDEF";
    
    private void WriteFile( StreamWriter sw, icaoTable subTable )
    {
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsJsonXsv( ) );
      }
    }


    public bool WriteCsv( icaoDatabase db, Stream jsonOutStream )
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
