using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static d1090dataLib.d1090ext_aplib.apRec;

namespace d1090dataLib.d1090ext_aplib
{
  /// <summary>
  /// Writes airports as geojson
  /// </summary>
  public class apGeoWriter
  {
    private void WriteFile( StreamWriter sw, apTable subTable )
    {
      int i = 1; // have to count to avoid the last comma ??!!
      foreach ( var rec in subTable ) {
        sw.WriteLine( rec.Value.AsGeoJson( ) + ( ( i++ < subTable.Count ) ? "," : "" ) ); // adds commas but not for the last one
      }
    }

    /// <summary>
    /// Writes a GeoJson from the database
    /// </summary>
    /// <param name="db">The database</param>
    /// <param name="geojFile">the filename to write to</param>
    /// <param name="rangeLimitNm">Limits the output based on a range [Nm]</param>
    /// <returns>True ??!!</returns>
    public bool WriteGeoJson( apDatabase db, Stream geojOutStream,
                                double rangeLimitNm = -1.0, double Lat = 0, double Lon = 0, AptTypes[] aptTypes = null )
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
          WriteFile( sw, db.GetSubtable( rangeLimitNm, Lat, Lon, aptTypes ) );
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

