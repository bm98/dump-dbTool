using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using d1090dataLib.xp11_navlib;

namespace d1090dataLib.xp11_awylib
{
  public class awyGeoWriter
  {

    private HashSet<string> m_trackedMarkers = new HashSet<string>( ); // track written Markers

    /// <summary>
    /// Writes a GeoJson from the database
    /// </summary>
    /// <param name="db">The database</param>
    /// <param name="geojFile">the filename to write to</param>
    /// <returns>True ??!!</returns>
    public bool WriteGeoJson( awyDatabase db, navDatabase ndb, Stream geojOutStream,
                                string layer,
                                double rangeLimitNm = -1.0, double Lat = 0, double Lon = 0 )
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
        bool doComma = false; // pesty last comma avoidance...
        sw.WriteLine( head );

        // Navs and Fixes in range
        var NAVTAB = ndb.GetSubtable( rangeLimitNm, Lat, Lon );
        // for each item in NDB get the airway(s) that is using it 
        foreach ( var rec in NAVTAB ) {
          var AWYTAB = db.GetSubtable( rec.Key );
          // for each item then find it's counterpart and write an airway segment
          foreach ( var awRec in AWYTAB ) {
            if ( awRec.Value.layer == layer ) {
              // expected layer (hi or lo)
              if ( rec.Key == awRec.Value.startID ) {
                var endNav = ndb.GetSubtable( )[awRec.Value.endID];
                if ( !m_trackedMarkers.Contains( rec.Value.ident ) ) {
                  m_trackedMarkers.Add( rec.Value.ident );
                }
                if ( !m_trackedMarkers.Contains( endNav.ident ) ) {
                  m_trackedMarkers.Add( endNav.ident );
                }
                sw.WriteLine( ( doComma ? "," : "" ) + awRec.Value.AsGeoJson( rec.Value.lat, rec.Value.lon, endNav.lat, endNav.lon ) );
                doComma = true; // after the first line prepend records with comma
              }
              else if ( rec.Key == awRec.Value.endID ) {
                var startNav = ndb.GetSubtable( )[awRec.Value.endID];
                if ( !m_trackedMarkers.Contains( startNav.ident ) ) {
                  m_trackedMarkers.Add( startNav.ident );
                }
                if ( !m_trackedMarkers.Contains( rec.Value.ident ) ) {
                  m_trackedMarkers.Add( rec.Value.ident );
                }
                sw.WriteLine( ( doComma ? "," : "" ) + awRec.Value.AsGeoJson( startNav.lat, startNav.lon, rec.Value.lat, rec.Value.lon ) );
                doComma = true; // after the first line prepend records with comma
              }
              else {
                ; //ERROR - DEBUG BREAK
              }
            }
          }
        }
        sw.WriteLine( foot );
      }
      return true;
    }
    /// <summary>
    /// Writes a GeoJson from the database
    /// </summary>
    /// <param name="db">The database</param>
    /// <param name="geojFile">the filename to write to</param>
    /// <returns>True ??!!</returns>
    public bool WriteGeoJsonUsedMarkers( navDatabase ndb, Stream geojOutStream )
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
        bool doComma = false; // pesty last comma avoidance...
        sw.WriteLine( head );
        // Navs and Fixes in tracked
        foreach ( var rec in m_trackedMarkers ) {
          var nav = ndb.GetSubtable( )[rec];
          // expected layer (hi or lo)
          sw.WriteLine( ( doComma ? "," : "" ) + nav.AsGeoJson( ) );
          doComma = true; // after the first line prepend records with comma
        }
        sw.WriteLine( foot );
      }
      return true;
    }

  }
}
