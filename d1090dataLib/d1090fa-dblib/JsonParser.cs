using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Writing a small Json parser in order to comply with .NetStandard 1.4
/// </summary>
namespace d1090dataLib.d1090fa_dblib
{
  internal class JsonRecord : SortedList<string, JsonContent>
  {

  }

  internal class JsonContent : SortedList<string, string>
  {

  }

  /// <summary>
  /// Build a Dictionary of content from a simple Json fragment
  /// </summary>
  internal class JsonParser
  {
    /* To be taken apart..
     * 
      /// 	"01001": {
      /// 	"r": "V5-NAM",		
      /// 	"t": "F900"
      /// 	},     
       */

    /// <summary>
    /// Split a comma separated line - take care of comas in strings
    /// Note: no whitespace treatment whatsoever
    /// </summary>
    /// <param name="csvLine"></param>
    /// <returns>A list of separated items</returns>
    internal static IList<string> Split( string csvLine, char splitchar = ',' )
    {
      var elements = new List<string>( );
      bool ins = false;
      string cap = "";
      for ( int i = 0; i < csvLine.Length; i++ ) {
        if ( csvLine[i] == '"' ) {
          if ( ins ) {
            // end of string
            ins = false;
          }
          else {
            // start of string
            ins = true;
          }
          cap += csvLine[i]; // collect
        }
        else if ( csvLine[i] == splitchar ) {
          if ( !ins ) {
            // end capture
            elements.Add( cap );
            cap = "";
          }
          else {
            cap += csvLine[i]; // collect
          }
        }
        else {
          // neither , nor "
          cap += csvLine[i]; // collect
        }
      }
      if ( !string.IsNullOrEmpty( cap ) ) {
        elements.Add( cap );
      }
      return elements;
    }

    // expecting something like "key":{"item":"content"[, ...]}
    // or {"item":"content"[, ...]}
    // straight forward and not really nice - should do it..
    public static JsonRecord Decompose( string js )
    {
      var record = new JsonRecord( );
      // do some houskeeping first
      if ( string.IsNullOrWhiteSpace( js ) ) return null; // no usable content
      js = js.Replace( "\n", "" ).Replace( "\r", "" ).Trim( ); // cleanup any CR, LFs and whitespaces
      if ( js.EndsWith( "," ) )
        js = js.Substring( 0, js.Length - 1 ).TrimEnd( ); // cut end comma and clean

      // divide key and content - if needed
      string key = "";
      int colPos = js.IndexOf( '{' ); 
      if ( colPos > 1 ) {
        // would be key..
        colPos = js.IndexOf( ':' );
        if ( colPos < 1 ) return null; // no key element at all
        key = RemoveApo( js.Substring( 0, colPos ) );
        js = js.Substring( colPos + 1 ).TrimStart( ); // cut the key part and clean
      }
      // starting brace must be at the beginning now
      if ( !js.StartsWith( "{" ) ) return null; // no content at all ?? key only is disregarded here
      if ( !js.EndsWith( "}" ) ) return null; // misalinged content not { something }
      js = js.Substring( 1, js.Length - 2 ).Trim( );
      // we should be left with 'content, content' here
      IList<string> contList = Split( js, ',' );
      foreach ( var item in contList ) {
        IList<string> itemPairs = Split( item, ':' );
        if ( !record.ContainsKey( key ) ) {
          record.Add( key, new JsonContent( ) );
        }
        record[key].Add( RemoveApo( itemPairs[0] ), RemoveApo( itemPairs[1] ) );
      }
      return record;
    }

    /// <summary>
    /// Extracts a top level 'key:{ anything }' element from a Json string
    /// </summary>
    /// <param name="jsInput">The input string</param>
    /// <param name="fragment">out the extracted fragment</param>
    /// <param name="jsRemaining">out the input - the extracted part</param>
    /// <returns></returns>
    public static bool ExtractFragment( string jsInput, out string fragment, out string jsRemaining )
    {
      fragment = ""; jsRemaining = jsInput;
      // do some houskeeping first
      if ( string.IsNullOrWhiteSpace( jsInput ) ) return false; // no usable content

      jsInput = jsInput.Replace( "\n", "" ).Replace( "\r", "" ).TrimStart( ); // cleanup any CR, LFs and whitespaces

      int endPos = 0;
      int bOpen = 0; bool triggered = false;
      if ( !jsInput.Contains( "{" ) ) return false; // seems not having an { item..
      if ( !jsInput.Contains( "}" ) ) return false; // seems not having an } item..

      for ( int i = 0; i < jsInput.Length; i++ ) {
        if ( jsInput[i] == '{' ) {
          bOpen++; triggered = true;
        }
        else if ( triggered && jsInput[i] == '}' ) {
          bOpen--;
        }
        if ( triggered && bOpen == 0 ) {
          endPos = i;
          triggered = false;
          break; // no further reading needed
        }
      }
      if ( endPos > 0 ) {
        // extract
        fragment = jsInput.Substring( 0, endPos + 1 );
        jsRemaining = jsInput.Substring( endPos + 1 );
        return true;
      }
      return false;
    }

    /// <summary>
    /// Get a Json fragment from the input string  
    /// [key:]{ content } or { content } 
    /// </summary>
    /// <param name="jsInput"></param>
    /// <returns>The fragment</returns>
    public static string ExtractFragment( string jsInput)
    {
      // do some houskeeping first
      if ( string.IsNullOrWhiteSpace( jsInput ) ) return ""; // no usable content

      int endPos = 0;
      int bOpen = 0; bool triggered = false;
      if ( !jsInput.Contains( "{" ) ) return ""; // seems not having an { item..
      if ( !jsInput.Contains( "}" ) ) return ""; // seems not having an } item..

      for ( int i = 0; i < jsInput.Length; i++ ) {
        if ( jsInput[i] == '{' ) {
          bOpen++; triggered = true;
        }
        else if ( triggered && jsInput[i] == '}' ) {
          bOpen--;
        }
        if ( triggered && bOpen == 0 ) {
          endPos = i;
          triggered = false;
          break; // no further reading needed
        }
      }
      if ( endPos > 0 ) {
        // extract
        var fragment = jsInput.Substring( 0, endPos + 1 );
        return fragment;
      }
      return "";
    }

    /// <summary>
    /// Removes the Apostroph encapsulation and returns a plain string from it
    /// </summary>
    /// <param name="apoString">A string enclosed in Apostrophes</param>
    /// <returns>The plain string</returns>
    public static string RemoveApo( string apoString )
    {
      return apoString.Trim( ).Replace( "\"", "" );
    }


  }
}
