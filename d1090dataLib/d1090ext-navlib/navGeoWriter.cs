using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static d1090dataLib.d1090ext_navlib.navRec;

namespace d1090dataLib.d1090ext_navlib
{
  /// <summary>
  /// Writes navaids as geojson
  /// </summary>
  public class navGeoWriter
  {
    /// <summary>
    /// Write one geojson file from the supplied table
    /// </summary>
    /// <param name="sw">An open streamwriter</param>
    /// <param name="subTable">The apTable to write out</param>
    private static void WriteFile( StreamWriter sw, navTable subTable )
    {
      int i = 1; // have to count to avoid the last comma ??!!
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsGeoJson( ) + ( ( i++ < subTable.Count ) ? "," : "" ) ); // adds commas but not for the last one
      }
    }

    /// <summary>
    /// Writes a geojson file from the given database into the open stream
    /// Selects items to write from the given selection criteria
    /// </summary>
    /// <param name="db">The navDatabase to dump</param>
    /// <param name="geojOutStream">The open outstream</param>
    /// <param name="rangeLimitNm">Range Limit in nm</param>
    /// <param name="Lat">Center Lat (decimal)</param>
    /// <param name="Lon">Center Lon (decimal)</param>
    /// <param name="navTypes">Type of nav items to include</param>
    /// <returns>True ??!!</returns>
    public static bool WriteGeoJson( navDatabase db, Stream geojOutStream,
                                double rangeLimitNm = -1.0, double Lat = 0, double Lon = 0, NavTypes[] navTypes = null )
    {
      /*
          {
          "type": "FeatureCollection",
          "crs": { "type": "name", "properties": { "name": "urn:ogc:def:crs:OGC:1.3:CRS84" } },
          "features": [
                            entry, entry
                      ]
           }
       */
      string head = $"{{\n\"type\": \"FeatureCollection\",\n" +
                    $"\"crs\": {{ \"type\": \"name\", \"properties\": {{ \"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\" }} }}," +
                    $"\"features\": [";
      string foot = $"]\n}}";

      using ( var sw = new StreamWriter( geojOutStream, Encoding.UTF8 ) ) {
        sw.WriteLine( head );
        if ( rangeLimitNm > 0 ) {
          WriteFile( sw, db.GetSubtable( rangeLimitNm, Lat, Lon, navTypes ) );
        }
        else {
          WriteFile( sw, db.GetSubtable( ) );
        }
        sw.WriteLine( foot );
      }
      return true;
    }

  }
}
