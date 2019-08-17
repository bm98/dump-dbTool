using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Reads minimum CSV aircraft data 
  /// </summary>
  public class icaoCsvReader
  {

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static icaoRec FromNative( string native )
    {
      // should be the TSV variant
      string[] e = native.Split( new char[] { ',', ';' } ); // either comma or semi separated
      //     0       1          2    
      //   icao,registration,airctype
      string icao = "", regName = "", airctype = "";

      if ( e.Length >= 3 ) {
        icao = e[0].ToUpperInvariant();
        regName = e[1];
        airctype = e[2].ToUpperInvariant();
        return new icaoRec( icao, regName, airctype );
      }
      else {
        return new icaoRec( "", "", "" ); // invalid one
      }

    }



    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>Result as string or empty if ok</returns>
    private string ReadDbFile( ref icaoDatabase db, string fName )
    {
      string ret = "";
      using ( var sr = new StreamReader( fName ) ) {
        string buffer = sr.ReadLine( ); // header line
        buffer = sr.ReadLine( ); 
        while ( !sr.EndOfStream ) {
          var rec = FromNative( buffer );
          if ( rec.IsValid ) {
            ret += db.Add( rec ); // collect adding information
          }
          buffer = sr.ReadLine( );
        }
        //
      }
      return ret;
    }

    /// <summary>
    /// Reads all data from the given folder
    /// </summary>
    /// <param name="csvFile">A fully qualified path to where the db files are located</param>
    /// <returns>Result as string or empty if ok</returns>
    public string ReadDb( ref icaoDatabase db, string csvFile )
    {
      if ( !File.Exists( csvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, csvFile );
    }

  }
}
