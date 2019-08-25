using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace d1090dataLib.d1090fa_dblib
{
  internal class CsvTools
  {

    /// <summary>
    /// Split a line on delimiters accepting quoted input as entity
    /// </summary>
    /// <param name="csvLine">A csv line</param>
    /// <param name="delimiter">The delimiter char of the line</param>
    /// <returns>The array of string fields</returns>
    static public string[] Split( string csvLine, out bool qquoted, char delimiter = ',' )
    {
      // to catch..
      // plain,plain,..
      // plain,"quoted",..
      // "quoted",plain,..
      // "quoted","quoted",..
      // more strange things..
      // "comma,quoted",..
      // "quoted "innerQuote" ",..  / though no escapes 
      // "comma , quoted "inner , Quote" ,  ",..  / though no escapes - this will fail..
      // " SOMETHING "QUOTED" OTHER", more, end

      //  catch some EOF issues with old files here
      int nDelim = 0;
      qquoted = false;
      if ( csvLine.Length > 0 && csvLine[csvLine.Length - 1] != delimiter )
        csvLine += delimiter;
      var temp = new List<string>( );

      string buffer = "";
      int inQuote = 0;
      for ( int i = 0; i < csvLine.Length; i++ ) {
        // quote handling
        if ( csvLine[i] == '"' ) {
          if ( inQuote < 1 ) {
            buffer += '"'; // add only if first quote
          }
          inQuote++;
        }
        // delimiter handling
        else if ( csvLine[i] == delimiter ) {
          if ( ( inQuote % 2 ) > 0 ) {
            // odd number of quotes - within quotes a comma is treated as text
            buffer += delimiter;
          }
          else {
            // only out of quotes it is end of field
            if ( inQuote > 0 ) buffer += '"'; // add termination if quoted
            temp.Add( buffer ); // add collected field
            buffer = "";
            nDelim++; // count delimiters
            if ( inQuote > 2 ) qquoted = true; // report quotes in quote
            inQuote = 0; // not longer
          }
        }
        // everything else
        else {
          buffer += csvLine[i]; // just add to field
        }
      }

      // collected content must be added
      if ( !string.IsNullOrEmpty( buffer ) ) {
        temp.Add( buffer );
      }

      // fix last after comma if it was empty
      // now this is messy.. c6k provides a last comma, v8k and cPro not
      // we normalize internally for c8k and add the comma for c6k while writing out only
      // ... ,,X,   vs.  ... ,,X,Something 
      // count  = 3     count  = 4
      // nDelim = 3     nDelim = 3
      if (temp.Count<=nDelim) {
        temp.Add( "" ); // add an empty field to close 
      }
      return temp.ToArray( );
    }


    /// <summary>
    /// Merge an array of strings into a single comma separated string
    /// </summary>
    /// <param name="csvItems">The array of items to merge</param>
    /// <returns>The CSV string</returns>
    static public string Merge( string[] csvItems, char delimiter = ',' )
    {
      string ret = "";
      foreach ( var s in csvItems ) {
        ret += $"{s}{delimiter}";
      }
      return ret.Remove( ret.Length - 1 ); // remove last delimiter
    }


  }
}
