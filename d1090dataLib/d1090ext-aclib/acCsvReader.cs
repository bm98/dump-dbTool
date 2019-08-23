using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_aclib
{
  /// <summary>
  /// Reads aircraft database in CSV format 
  /// </summary>
  public class acCsvReader
  {
    private const string NULL = "NULL";

    /// <summary>
    /// Translates from native to generic record format
    /// </summary>
    /// <param name="native"></param>
    /// <returns></returns>
    private static acRec FromNative( string native )
    {
      /*   0    1    2   3     4
         icao,regid,mdl,type,operator   (note: mdl is the icao type)
       */
      // should be the CSV variant
      string[] e = native.Split( new char[] { ',' } );
      string icao = "", regid = "", mdl = "", type = "", operator_ = "";

      icao = e[0].Trim( new char[] { ' ', '"' } ).ToUpperInvariant( );
      if ( e.Length > 1 )
        regid = e[1].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 2 )
        mdl = e[2].Trim( new char[] { ' ', '"' } ); // fix null recs
      if ( e.Length > 3 )
        type = e[3].Trim( new char[] { ' ', '"' } );
      if ( e.Length > 4 )
        operator_ = e[4].Trim( new char[] { ' ', '"' } );

      regid = ( regid == "00000000" ) ? "" : regid; // fix null recs
      mdl = ( mdl == "0000" ) ? "" : mdl; // fix null recs

      return new acRec( icao, regid, mdl, type, operator_ );
    }

    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="db">The acDatabase to fill from the file</param>
    /// <param name="fName">The qualified filename</param>
    /// <returns>The result string, either empty or error</returns>
    private static string ReadDbFile( ref acDatabase db, string fName )
    {
      var icaoPre = Path.GetFileNameWithoutExtension( fName );
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
    /// Reads all data from the given file
    /// </summary>
    /// <param name="db">The acDatabase to fill from the file</param>
    /// <param name="csvFile">The file to read</param>
    /// <returns>The result string, either empty or error</returns>
    public static string ReadDb( ref acDatabase db, string csvFile )
    {
      if ( !File.Exists( csvFile ) ) return $"File does not exist\n";

      return ReadDbFile( ref db, csvFile );
    }




  }
}
