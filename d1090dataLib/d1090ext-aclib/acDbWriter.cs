﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace d1090dataLib.d1090ext_aclib
{
  /// <summary>
  /// Writer for the Aircraft database
  /// Writes the FA Json formated data (many files with prefixed filenames)
  /// </summary>
  public class acDbWriter
  {
    const int NREC = 1024; // number of entries in one Part file

    const string PREFIXES = "0123456789ABCDEF";

    /// <summary>
    /// Decomposes the database is chunks of NREC
    /// </summary>
    /// <param name="dbFolder">The database folder to write to</param>
    /// <param name="table">The table containing the prefixed records</param>
    private static void DecomposeTable( string dbFolder, acTable table )
    {
      int cnt = table.Count; // how many with that prefix
      if ( cnt <= NREC ) {
        WriteFile( dbFolder, table, "" );
        return;
      }
      else {
        // decompose and analyze
        var tmpParts = new SortedDictionary<string, acTable>( );
        // make a split into all child tables
        var writeTable = new acTable( table.DbPrefix ); // the main table carries the submitted prefix
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

    /// <summary>
    /// Writes one file from the given sub table
    /// </summary>
    /// <param name="dbFolder">The folder to write to</param>
    /// <param name="subTable">The subtable to write out</param>
    /// <param name="extension">The extension string to add for the FA record keeping</param>
    private static void WriteFile( string dbFolder, acTable subTable, string extension )
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

    /// <summary>
    /// Write the aircraft db as FA formatted Json files into the given folder
    /// </summary>
    /// <param name="db">The database to dump</param>
    /// <param name="dbFolder">The folder to write to</param>
    /// <returns>True for success</returns>
    public static bool WriteDb( acDatabase db, string dbFolder )
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
