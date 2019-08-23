using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Writes the Act DB in FA format
  ///    /// 	{"A002":
  /// 	  {"desc":"G1P",
  ///     "wtc":"L"},
  ///    ..}
  /// </summary>
  public class icaoActDbWriter
  {

    /// <summary>
    /// Writes one file from the given sub table
    /// </summary>
    /// <param name="dbFile">The file to write to</param>
    /// <param name="subTable">The subtable to write out</param>
    private static void WriteFile( string dbFile, icaoActTable subTable)
    {
      string fName = dbFile;
      using ( var sw = new StreamWriter( fName ) ) {
        string buffer = "";
        foreach ( var rec in subTable ) {
          buffer += rec.Value.AsJson( ) + ","; // delete prefix from the record
        }
        if ( buffer.EndsWith( "," ) )
          buffer = buffer.Substring( 0, buffer.Length - 1 ); // cut last comma

        sw.Write( $"{{{buffer}}}" ); //  { buffer }
      }
    }

    // writes the README file
    private static void WriteReadme(string folder )
    {
      string fName = Path.Combine( folder, "README" );
      using ( var sw = new StreamWriter( fName ) ) {
        sw.WriteLine( $"ICAO Aircraft type designation written {DateTime.Now.ToLongDateString( )}" );
        sw.WriteLine( $"see https://www.icao.int/publications/DOC8643" );
      }
    }

    /// <summary>
    /// Write the ICAO Aircraft Type Designator database file
    /// </summary>
    /// <param name="db">The ACT database</param>
    /// <param name="dbFile">Absolute path to the db file to write</param>
    /// <returns>True if OK</returns>
    public static bool WriteDb( icaoActDatabase db, string dbFile )
    {
      string dbFolder = Path.GetDirectoryName( dbFile );
      if ( !Directory.Exists( dbFolder ) ) Directory.CreateDirectory( dbFolder );

      WriteFile( dbFile, db.GetTable() );
      WriteReadme( dbFolder );
      return true;
    }


  }
}
