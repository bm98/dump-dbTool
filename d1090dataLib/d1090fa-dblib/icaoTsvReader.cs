using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  public class icaoTsvReader
  {

    private const string NULL = "NULL";

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static icaoRec FromNative( string native )
    {
      // should be the TSV variant
      string[] e = native.Split( new char[] { '\t' } );
      //     0             1          2        3            4              5            6
      // FirstCreated	LastModified	ModeS	ModeSCountry	Registration	ICAOTypeCode	type_flight
      string icao = "", regName = "", airctype = "";

      icao = e[2].ToUpperInvariant( );
      if ( e.Length > 4 )
        regName = ( e[4] == NULL ) ? "" : e[4];
      if ( e.Length > 5 )
        airctype = ( e[5] == NULL ) ? "" : e[5];

      return new icaoRec( icao, regName, airctype );
    }



    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>Result as string or empty if ok</returns>
    private string ReadDbFile( ref icaoDatabase db, string fName )
    {
      var icaoPre = Path.GetFileNameWithoutExtension( fName );
      string ret = "";
      using ( var sr = new StreamReader( fName ) ) {
        string buffer = sr.ReadLine( );
        buffer = sr.ReadLine( ); // header line
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
    /// <param name="tsvFile">A fully qualified path to where the db files are located</param>
    /// <returns>Result as string or empty if ok</returns>
    public string ReadDb( ref icaoDatabase db, string tsvFile )
    {
      if ( !File.Exists( tsvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, tsvFile );
    }

  }
}
