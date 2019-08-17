using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace d1090dataLib.d1090fa_dblib
{
  /// <summary>
  /// Reads a FlightAware Icao Database into a IcaoTable
  /// </summary>
  public class icaoDbReader
  {
    /// <summary>
    /// Returns a new Icao Record from given Jason
    /// </summary>
    /// <param name="js">The record as Jason fragment</param>
    private static icaoRec FromNative( string js )
    {
      JsonRecord jRec = JsonParser.Decompose( js );
      if ( jRec?.Count > 0 ) {
        var reg = !jRec.Values[0].ContainsKey( "r" ) ? "" : jRec.Values[0]["r"];
        var typ = !jRec.Values[0].ContainsKey( "t" ) ? "" : jRec.Values[0]["t"];
        var iRec = new icaoRec( jRec.Keys[0].ToUpperInvariant(), reg, typ );
        return iRec;
      }
      else {
        return null;
      }
    }


    /// <summary>
    /// Reads one db file
    /// </summary>
    /// <param name="fName">The qualified filename</param>
    /// <returns>A table or null</returns>
    private string ReadDbFile(ref icaoDatabase db, string fName )
    {
      if ( !File.Exists( fName ) ) {
        return $"File {fName} does not exist\n";
      }

      var icaoPre = Path.GetFileNameWithoutExtension( fName );
      string ret = "";
      using ( var sr = new StreamReader( fName ) ) {
        string buffer = sr.ReadToEnd( );
        buffer = buffer.Replace( "\n", "" ).Replace( "\r", "" ).Trim( ); // cleanup any CR, LFs and whitespaces
        buffer = buffer.Substring( 1 ); // skip enclosing {
        var fragment = JsonParser.ExtractFragment( buffer );
        while (!string.IsNullOrEmpty(fragment)) {
          buffer = buffer.Substring( fragment.Length+1 ); // remove extracted + comma
          var rec = FromNative( fragment );
          if ( rec.IsValid ) {
            rec.icao = icaoPre.ToUpperInvariant( ) + rec.icao; // make it a valid one
            ret += db.Add( rec ); // collecting add information
          }
          fragment = JsonParser.ExtractFragment( buffer );
        }
      }
      return ret;
    }


    /// <summary>
    /// Reads all data from the given folder
    /// </summary>
    /// <param name="dbFolder">A fully qualified path to where the db files are located</param>
    /// <returns>A populated table or null</returns>
    public string ReadDb( ref icaoDatabase db, string dbFolder )
    {
      if ( !Directory.Exists( dbFolder ) ) return $"Folder {dbFolder} does not exist\n"; ;
      string ret = "";

      IEnumerable<string> dbFiles = Directory.EnumerateFiles( dbFolder, "*.json", SearchOption.TopDirectoryOnly );
      foreach ( var dbfile in dbFiles ) {
        ret += ReadDbFile( ref db, dbfile );
      }
      return ret;
    }


  }
}
