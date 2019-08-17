using System;
using System.Collections.Generic;
using System.Text;

namespace d1090dataLib.xp11_awylib
{
  public class awyRec
  {
    // fields in db
    public string ident = ""; // Key ?! start_icao_id + "_" + start_icao_region + "_" + end_icao_id + "_" + end_icao_region

    public string start_icao_id = "";     //01 Identifier of enroute fix or navaid at the beginning of this segment
    public string start_icao_region = ""; //02 ICAO region code of enroute NDB or terminal area airport Must be region code according to ICAO document No. 7910 
    public string start_navaid = "";      //03 Type of Fix or Navaid 11 = Fix, 2 = enroute NDB, 3 = VHF navaid
    public string end_icao_id = "";       //04 Identifier of enroute fix or navaid at the beginning of this segment
    public string end_icao_region = "";   //05 ICAO region code of enroute NDB or terminal area airport Must be region code according to ICAO document No. 7910 
    public string end_navaid = "";        //06 Type of Fix or Navaid 11 = Fix, 2 = enroute NDB, 3 = VHF navaid
    public string restriction = "";       //07 N = “None”, F = ”Forward”, B = “Backward”
                                          //   If the directional restriction is F = “Forward”, the airway segment is authorized to be flown in 
                                          //    the direction from the first fix to the second fix.
                                          //   If the directional restriction is B = “Backward”, the segment is only to be flown in the
                                          //    direction from second fix to first fix.
    public string layer = "";             //08 This is a "High" airway (1 = "low", 2 = "high"). 
                                          //   If an airway segment is both High and Low, then it should be listed twice( once in each category ). 
                                          //   This determines if the airway is shown on X-Plane's "High Enroute" or "Low Enroute" charts
    public string baselevel = "";         //09 Base of airway in hundreds of feet (18000 ft in this example) Integer between 0 and 600
    public string toplevel = "";          //10 Top of airways in hundreds of feet (45000 ft in this example) Integer between 0 and 600

    public string name = "";              //11 Airway segment name. Up to five characters per name, names separated by hyphens
                                          //  If multiple airways share this segment, then all names will be included separated by a hyphen( eg. "J13-J14-J15")

    public string startID = "";
    public string endID = "";

    /// <summary>
    /// cTor: populate the record
    /// </summary>
    /// <param name="id">ident (UPPERCASE)</param>
    public awyRec( string sid, string sreg, string styp,
                   string eid, string ereg, string etyp,
                   string rest, string lay, string blevel, string tlevel,
                   string nam )
    {
      start_icao_id = sid;
      start_icao_region = sreg;
      start_navaid = styp;
      end_icao_id = eid;
      end_icao_region = ereg;
      end_navaid = etyp;
      restriction = rest;
      layer = lay;
      baselevel = blevel;
      toplevel = tlevel;
      name = nam;

      startID = $"{sid}_{sreg}";
      endID = $"{eid}_{ereg}";
      ident = $"{startID}_{endID}";

    }


    /// <summary>
    /// returns true if the record is valid
    /// </summary>
    public bool IsValid { get => !string.IsNullOrEmpty( start_icao_id ) && !string.IsNullOrEmpty( end_icao_id ); }

    // GeoJson stuff

    /// <summary>
    /// Returns the content in GeoJson Notation
    /// </summary>
    /// <param name="slat">start loc Lat</param>
    /// <param name="slon">start loc Lon</param>
    /// <param name="elat">end loc Lat</param>
    /// <param name="elon">end loc Lon</param>
    /// <returns>The GeoJson database record</returns>
    public string AsGeoJson(string slat, string slon , string elat, string elon )
    {
      // Example:
      /*
           {
              "type": "Feature",
              "properties": {"Name":"xy", "Ident":"hh"},
              "geometry": { "type": "LineString", "coordinates": [ [ 9.1131591796875, 47.934746769467786 ], [ 9.656982421875, 48.180738507303836 ] ] }
           }
       */
      string feature = $"\"type\":\"Feature\"";
      string props = $"\"properties\":{{\"Name\":\"{start_icao_id}\",\"Ident\":\"{name}\"}}";
      string geo = $"\"geometry\":{{\"type\":\"LineString\",\"coordinates\":[[{slon},{slat}],[{elon},{elat}]]}}";

      string ret = $"{{{feature},{props},{geo}}}";
      return ret;
    }




  }
}
