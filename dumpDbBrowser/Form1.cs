using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using d1090dataLib.bsDB_connection;
using d1090dataLib.d1090ext_aclib;
using d1090dataLib.d1090ext_aplib;
using d1090dataLib.d1090ext_rtlib;
using d1090dataLib.d1090fa_dblib;
using d1090dataLib.d1090ext_aircraftsDB;
using d1090dataLib.d1090ext_flightsDB;
using navLib = d1090dataLib.d1090ext_navlib;

using d1090dataLib.xp11_awylib;
using xNavLib = d1090dataLib.xp11_navlib;
using d1090dataLib.xp11_navlib;


namespace dumpDbBrowser
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent( );
    }
    const string DATETIME = "yyyyMMddTHHmmss";

    icaoDatabase IDB = new icaoDatabase( );
    const string IDB_PATH = "db"; // from main

    const string EXT_PATH = "ext_bm98adbs"; // from main

    acDatabase ACDB = new acDatabase( );
    string ACDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "airc" );

    apDatabase APDB = new apDatabase( );
    string APDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "airp" );

    rtDatabase RTDB = new rtDatabase( );
    string RTDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "route" );

    navLib.navDatabase NAVDB = new navLib.navDatabase( );
    string NAVDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "nava" );

    xNavLib.navDatabase XNAVDB = new xNavLib.navDatabase( );
    string XNAVDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "xnava" );

    awyDatabase XAWYDB = new awyDatabase( );
    string XAWYDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "xnava" );

    icaoActDatabase ACTDB = new icaoActDatabase( );
    string ACTDB_PATH = Path.Combine( IDB_PATH, EXT_PATH, "act" );

    string CSV_PATH = Path.Combine( IDB_PATH, EXT_PATH );



    private void btLoad_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load FA Json database";
      OFD.Filter = "JSON files|*.json|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string folder = OFD.FileName;
        folder = Path.GetDirectoryName( folder );
        RTB.Text += $"Reading Json Db: {folder}\n";
        var ir = new icaoDbReader( );
        string changes = ir.ReadDb( ref IDB, folder );
        changes += $"DONE \n";
        changes += $"Database contains {IDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btLoadTsv_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load TSV file";
      OFD.Filter = "TSV files|*.tsv|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new icaoTsvReader( );
        RTB.Text += $"Reading TSV: {file}\n";
        string changes = ir.ReadDb( ref IDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {IDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btLoadBS_Click( object sender, EventArgs e )
    {
      OFD.FileName = "BaseStation.sqb";
      OFD.Title = "Load BaseStation SQB file";
      OFD.Filter = "SQB files|*.sqb|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new BaseStation( );
        if ( !ir.Connect( file ) ) {
          RTB.Text += $"ERROR Reading BaseStation SQB database file: {file} - cannot open database\n";
        }
        else {
          RTB.Text += $"Reading BaseStation SQB database file: {file}\n";
          string changes = ir.ReadDb( ref IDB ); ;
          changes += $"DONE \n";
          changes += $"Database contains {IDB.Count} records \n";
          RTB.Text += changes;
        }
      }
    }

    private void btWriteDb_Click( object sender, EventArgs e )
    {
      string folder = Path.Combine( @"D:\DUMPTEST", IDB_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      var iw = new icaoDbWriter( );
      iw.WriteDb( IDB, folder );
      RTB.Text += $"JsonDB ModeS written\n";
    }

    private void btWriteIcaoCSV_Click( object sender, EventArgs e )
    {
      var iw = new icaoCsvWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      using ( var outS = File.Create( Path.Combine( folder, "fa-data-" + DateTime.Now.ToString( DATETIME ) + ".csv" ) ) ) {
        iw.WriteCsv( IDB, outS );
      }
      RTB.Text += $"CSV Icao ModeS file written\n";
    }

    private void btWriteIcaoJsonCSV_Click( object sender, EventArgs e )
    {
      var iw = new icaoJsonXsvWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      using ( var outS = File.Create( Path.Combine( folder, "fa-data-json-" + DateTime.Now.ToString( DATETIME ) + ".csv" ) ) ) {
        iw.WriteCsv( IDB, outS );
      }
      RTB.Text += $"CSV (Json) Icao ModeS file written\n";
    }

    private void btWriteIcaoSQDB_Click( object sender, EventArgs e )
    {
      if ( IDB.Count <= 0 ) {
        RTB.Text += $"ERROR - No Aircraft records found, load BaseStation first\n";
        return;
      }

      var iw = new AircraftsDB( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      RTB.Text += iw.CreateDB( Path.Combine( folder, "dump1090fa-aircrafts.sqb" ) );
      RTB.Text += iw.LoadDBfromIcao( IDB );
      RTB.Text += $"SQDB Icao ModeS file written (or not if prevously was an error...)\n";
    }



    private void btLoadAircraftCsv_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load Aircraft CSV file";
      OFD.Filter = "CSV files|*.csv|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new acCsvReader( );
        RTB.Text += $"Reading Aircraft CSV: {file}\n";
        string changes = ir.ReadDb( ref ACDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {ACDB.Count} records \n";
        RTB.Text += changes;
      }
      RTB.Text += $"\nUpdating Icao Database from Aircraft data\n";
      RTB.Text += $"The Icao Database contains now {IDB.Count} records \n";
      IDB.Update( ACDB ); // Update from Aircrafts
      RTB.Text += $"The Icao Database contains now {IDB.Count} records \n";
    }

    private void btWriteAircraftDB_Click( object sender, EventArgs e )
    {
      var iw = new acDbWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", ACDB_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      iw.WriteDb( ACDB, folder );
      RTB.Text += $"JsonDB Aircraft written\n";
    }

    private void btWriteAcCSV_Click( object sender, EventArgs e )
    {
      var iw = new acCsvWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      using ( var outS = File.Create( Path.Combine( folder, "aircraft-" + DateTime.Now.ToString( DATETIME ) + ".csv" ) ) ) {
        iw.WriteCsv( ACDB, outS );
      }
      RTB.Text += $"CSV Aircraft file written\n";
    }



    private void btLoadRouteTsv_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load Route TSV file";
      OFD.Filter = "TSV files|*.tsv|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new rtTsvReader( );
        RTB.Text += $"Reading Route TSV: {file}\n";
        string changes = ir.ReadDb( ref RTDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {RTDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btWriteRouteDb_Click( object sender, EventArgs e )
    {
      var iw = new rtDbWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", RTDB_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      iw.WriteDb( RTDB, folder );
      RTB.Text += $"JsonDB Route written\n";
    }

    private void btWriteRtCSV_Click( object sender, EventArgs e )
    {
      var iw = new rtCsvWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      using ( var outS = File.Create( Path.Combine( folder, "route-" + DateTime.Now.ToString( DATETIME ) + ".csv" ) ) ) {
        iw.WriteCsv( RTDB, outS );
      }
      RTB.Text += $"CSV Route file written\n";
    }

    private void btWriteFlightsSQDB_Click( object sender, EventArgs e )
    {
      if ( APDB.Count <= 0 ) {
        RTB.Text += $"ERROR - No Airports found, load airports first\n";
        return;
      }
      if ( RTDB.Count <= 0 ) {
        RTB.Text += $"ERROR - No Routes found, load routes first\n";
        return;
      }

      var iw = new FlightsDB( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      RTB.Text += iw.CreateDB( Path.Combine( folder, "dump1090fa-flights.sqb" ) );
      RTB.Text += iw.LoadDBfromAirports( APDB );
      RTB.Text += iw.LoadDBfromARoutes( RTDB );
      RTB.Text += $"SQDB Flights file written (or not if prevously was an error...)\n";
    }

    private void btLoadAirportCsv_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load Airport CSV file";
      OFD.Filter = "CSV files|*.csv|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new apCsvReader( );
        RTB.Text += $"Reading Airport CSV: {file}\n";
        string changes = ir.ReadDb( ref APDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {APDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btWriteAirportCsv_Click( object sender, EventArgs e )
    {
      var iw = new apCsvWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      using ( var outS = File.Create( Path.Combine( folder, "airport-" + DateTime.Now.ToString( DATETIME ) + ".csv" ) ) ) {
        iw.WriteCsv( APDB, outS );
      }
      RTB.Text += $"CSV Airport file written\n";
    }

    private void btWriteAirportGJson_Click( object sender, EventArgs e )
    {
      var iw = new apGeoWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      if ( chkRangeLimit.Checked ) {
        // limit by range and also by type
        // making two sets here, one for mid-large Apts and one for all others
        using ( var outS = File.Create( Path.Combine( folder, "apt-midlarge-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( APDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new apRec.AptTypes[] { apRec.AptTypes.medium_airport, apRec.AptTypes.large_airport } );
        }
        using ( var outS = File.Create( Path.Combine( folder, "apt-others-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( APDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new apRec.AptTypes[] { apRec.AptTypes.balloonport, apRec.AptTypes.closed, apRec.AptTypes.heliport,
                                 apRec.AptTypes.seaplane_base, apRec.AptTypes.small_airport, apRec.AptTypes.Other } );
        }
      }
      else {
        using ( var outS = File.Create( Path.Combine( folder, "apt-all-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( APDB, outS );
        }
      }
      RTB.Text += $"GeoJson Airport file written\n";
    }


    private void btLoadNavCSV_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load Navaid CSV file";
      OFD.Filter = "CSV files|*.csv|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new navLib.navCsvReader( );
        RTB.Text += $"Reading Navaid CSV: {file}\n";
        string changes = ir.ReadDb( ref NAVDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {NAVDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btWriteNavGJson_Click( object sender, EventArgs e )
    {
      var iw = new navLib.navGeoWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      if ( chkRangeLimit.Checked ) {
        // limit by range and also by type
        // making two sets here, one for NDBs and one for all others
        using ( var outS = File.Create( Path.Combine( folder, "nav-ndb-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( NAVDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new navLib.navRec.NavTypes[] { navLib.navRec.NavTypes.NDB, navLib.navRec.NavTypes.NDB_DME } );
        }
        using ( var outS = File.Create( Path.Combine( folder, "nav-vordme-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( NAVDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new navLib.navRec.NavTypes[] { navLib.navRec.NavTypes.DME, navLib.navRec.NavTypes.TACAN,
                                                  navLib.navRec.NavTypes.VOR, navLib.navRec.NavTypes.VORTAC,
                                                  navLib.navRec.NavTypes.VOR_DME, navLib.navRec.NavTypes.Other } );
        }
      }
      else {
        using ( var outS = File.Create( Path.Combine( folder, "nav-all-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( NAVDB, outS );
        }
      }
      RTB.Text += $"GeoJson Navaids file written\n";
    }

    private void btXP11nav_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load XP11 Nav File";
      OFD.Filter = "DAT files|*.dat|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new navReader( );
        RTB.Text += $"Reading XP11 Nav DAT: {file}\n";
        string changes = ir.ReadDb( ref XNAVDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {XNAVDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btXP11fix_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load XP11 Fix File";
      OFD.Filter = "DAT files|*.dat|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new fixReader( );
        RTB.Text += $"Reading XP11 Fix DAT: {file}\n";
        string changes = ir.ReadDb( ref XNAVDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains now {XNAVDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btXP11awy_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load XP11 Awy File";
      OFD.Filter = "DAT files|*.dat|All files|*.*";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        var ir = new awyReader( );
        RTB.Text += $"Reading XP11 Awy DAT: {file}\n";
        string changes = ir.ReadDb( ref XAWYDB, file ); ;
        changes += $"DONE \n";
        changes += $"Database contains {XAWYDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btWriteAwyGJson_Click( object sender, EventArgs e )
    {
      var iw = new xNavLib.navGeoWriter( );
      string folder = Path.Combine( @"D:\DUMPTEST", CSV_PATH );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      if ( chkRangeLimit.Checked ) {
        // limit by range and also by type
        // making two sets here, one for NDBs and one for all others
        using ( var outS = File.Create( Path.Combine( folder, "navx-ndb-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( XNAVDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new xNavLib.navRec.NavTypes[] { xNavLib.navRec.NavTypes.NDB } );
        }
        RTB.Text += $"navx-ndb-region-  Written\n";
        using ( var outS = File.Create( Path.Combine( folder, "navx-vordme-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( XNAVDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new xNavLib.navRec.NavTypes[] { xNavLib.navRec.NavTypes.VOR, xNavLib.navRec.NavTypes.DME } );
        }
        RTB.Text += $"navx-vordme-region-  Written\n";
        using ( var outS = File.Create( Path.Combine( folder, "navx-fix-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          iw.WriteGeoJson( XNAVDB, outS,
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ),
          new xNavLib.navRec.NavTypes[] { xNavLib.navRec.NavTypes.FIX } );
        }
        RTB.Text += $"navx-fix-region-  Written\n";

        var ia = new awyGeoWriter( );
        using ( var outS = File.Create( Path.Combine( folder, "navx-awy-hi-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          ia.WriteGeoJson( XAWYDB, XNAVDB, outS, "2",
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ) ); // layer hi
        }
        RTB.Text += $"navx-awy-hi-region-  Written\n";
        using ( var outS = File.Create( Path.Combine( folder, "navx-awy-lo-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          ia.WriteGeoJson( XAWYDB, XNAVDB, outS, "1",
          double.Parse( txRangeLimit.Text ), double.Parse( txMyLat.Text ), double.Parse( txMyLon.Text ) ); // layer lo
        }
        RTB.Text += $"navx-awy-lo-region-  Written\n";
        using ( var outS = File.Create( Path.Combine( folder, "navx-awy-fix-region-" + DateTime.Now.ToString( DATETIME ) + ".geojson" ) ) ) {
          ia.WriteGeoJsonUsedMarkers( XNAVDB, outS );
        }
        RTB.Text += $"navx-awy-fix-region-  Written\n";
      }
      else {
      }
      RTB.Text += $"GeoJson Navaids file written\n";
    }

    // ICAO ACT Part
    private void btLoadIcaoAct_Click( object sender, EventArgs e )
    {
      OFD.Title = "Load ICAO Aircraft Type Json database";
      OFD.Filter = "JSON files|*.json|All files|*.*";
      OFD.FileName = "ICAO-AircraftTypes.json";
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        string file = OFD.FileName;
        RTB.Text += $"Reading ICAO Aircraft Type Json Db: {file}\n";
        var ir = new icaoActReader( );
        string changes = ir.ReadDb( ref ACTDB, file );
        changes += $"DONE \n";
        changes += $"Database contains {ACTDB.Count} records \n";
        RTB.Text += changes;
      }
    }

    private void btWriteIcaoAct_Click( object sender, EventArgs e )
    {
      string file = Path.Combine( @"D:\DUMPTEST", IDB_PATH , "aircraft_types" , "icao_aircraft_types.json ");
      var iw = new icaoActDbWriter( );
      iw.WriteDb( ACTDB, file );
      RTB.Text += $"JsonDB ModeS written\n";
    }


  }
}
