using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_rtlib
{
  public class rtDbWriter
  {
    const int NREC = 2500;

    const string PREFIXES = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Gets a table with prefix to either write or decompose again
    /// </summary>
    /// <param name="dbFolder">The database folder</param>
    /// <param name="table">The table containing the prefixed records</param>
    /// <param name="prefix">The prefix limiting the choice</param>
    /// <returns></returns>
    private void DecomposeTable( string dbFolder, rtTable table )
    {
      int cnt = table.Count; // how many with that prefix
      if ( cnt <= NREC ) {
        WriteFile( dbFolder, table, "" );
        return;
      }
      else {
        // decompose and analyze
        SortedDictionary<string, rtTable> tmpParts = new SortedDictionary<string, rtTable>( );
        // make a split into all child tables
        var writeTable = new rtTable( table.DbPrefix ); // the main table carries the submitted prefix
        int total = 0;
        foreach ( var c in PREFIXES ) {
          // scan all prefixes
          string qualifier = table.DbPrefix + c.ToString( ); // one child level deeper
          var subTable = table.GetSubtable( qualifier );
          if ( subTable != null ) {
            if ( ( subTable != null ) && ( total + subTable.Count ) < NREC ) {
              // collect 
              total += subTable.Count;
              writeTable.AddSubtable( table.GetSubtable( qualifier ) );
            }
            else {
              tmpParts.Add( qualifier, table.GetSubtable( qualifier ) ); // collect all tables
            }
          }
        }
        // first write the ones that fit
        // "{children":["39","3C"]}
        // compose extension:
        string extension = "";
        foreach ( var tab in tmpParts ) {
          extension += $"\"{tab.Key}\",";
        }
        extension = extension.Substring( 0, extension.Length - 1 ); // remove last comma
        extension = $"\"children\":[{extension}]";
        WriteFile( dbFolder, writeTable, extension );
        writeTable = null; // free

        // process the ones that do not fit
        foreach ( var tab in tmpParts ) {
          DecomposeTable( dbFolder, tab.Value ); // recursive
        }
        return;
      }
    }


    private void WriteFile( string dbFolder, rtTable subTable, string extension )
    {
      string fName = Path.Combine( dbFolder, subTable.DbPrefix + ".json" );
      using ( var sw = new StreamWriter( fName ) ) {
        string buffer = "";
        foreach ( var rec in subTable ) {
          buffer += rec.Value.AsJson( subTable.DbPrefix ) + ","; // delete prefix from the record
        }
        if ( !string.IsNullOrEmpty( extension ) ) {
          buffer += extension;
        }
        if ( buffer.EndsWith( "," ) )
          buffer = buffer.Substring( 0, buffer.Length - 1 ); // cut last comma

        sw.Write( $"{{{buffer}}}" ); //  { buffer }
      }
    }


    public bool WriteDb( rtDatabase db, string dbFolder )
    {
      if ( !Directory.Exists( dbFolder ) ) return false;

      Dictionary<string, int> prefixes = new Dictionary<string, int>( );
      foreach ( var c in PREFIXES ) {
        DecomposeTable( dbFolder, db.GetSubtable( c.ToString( ) ) ); // level one get always decomposed
      }
      return true;
    }

  }
}
