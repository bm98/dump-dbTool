﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using d1090dataLib.d1090ext_coordlib;
using static d1090dataLib.d1090ext_navlib.navRec;

namespace d1090dataLib.d1090ext_navlib
{
  /// <summary>
  /// contains all icao airport records
  /// </summary>
  public class navTable : SortedDictionary<string, navRec>
  {

    public navTable()
    {
    }

    /// <summary>
    /// Create an ICAO table from the given table
    /// </summary>
    /// <param name="prefix">The prefix of the table</param>
    /// <param name="table">The source to fill from</param>
    public navTable( navTable table )
    {
      this.AddSubtable( table );
    }


    /// <summary>
    /// Add one record to the table
    /// </summary>
    /// <param name="rec"></param>
    public string Add( navRec rec )
    {
      string ret = "";
      if ( rec != null ) {
        if ( rec.ident.Length >= 2 ) {
          if ( !this.ContainsKey( rec.ident ) ) {
            this.Add( rec.ident, rec );
          }
          else {
            // overwite ?? NO 
          }
        }
      }
      return ret;
    }


    /// <summary>
    /// Adds a table to this table (omitting key dupes)
    /// </summary>
    /// <param name="subtable">A table to add to this table</param>
    public string AddSubtable( navTable subtable )
    {
      string ret = "";
      foreach ( var rec in subtable ) {
        try {
          ret += this.Add( rec.Value );
        }
        catch { }
      }
      return ret;
    }


    private string AddSubtable( IEnumerable<KeyValuePair<string, navRec>> selection )
    {
      string ret = "";
      foreach ( var rec in selection ) {
        try {
          ret += this.Add( rec.Value );
        }
        catch { }
      }
      return ret;
    }


    public navTable GetSubtable( double rangeLimitNm, double Lat, double Lon, NavTypes[] navTypes = null )
    {
      if ( navTypes == null ) navTypes = new NavTypes[] { NavTypes.All };

      var nT = new navTable( );
      var myLoc = new LatLon( Lat, Lon );
      foreach ( var rec in this ) {
        var dist = myLoc.DistanceTo( new LatLon( double.Parse( rec.Value.lat ), double.Parse( rec.Value.lon ) ), ConvConsts.EarthRadiusNm );
        if ( ( dist <= rangeLimitNm ) && ( rec.Value.IsTypeOf( navTypes ) ) ) {
          nT.Add( rec.Value );
        }
      }
      return nT;
    }



  }
}
