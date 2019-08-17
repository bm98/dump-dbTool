using System;
using System.Collections.Generic;
using System.IO;

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

namespace dump1090_dbTool
{
  /// <summary>
  /// Main prog for collecting, formatting and writing 
  /// dump1090fa compliant data and extended data for my addons
  /// </summary>
  class Program
  {

    private static void DumpHelp()
    {
      string[] e = System.Reflection.Assembly.GetExecutingAssembly( ).FullName.Split( new char[] { ',' } );
      string progName = e[0];
      Console.WriteLine( $"{progName} [-fa] buildPath" );
      Console.WriteLine( $"-fa (optional) to create a dump1090fa compatible aircraft JSON database" );
      Console.WriteLine( $"buildPath : absolute disk path where myRegion.csv and input folder is located" );
      Console.WriteLine( $"" );
    }

    private const string DATETIME = "yyyyMMddTHHmmss";

    // INPUT names
    private const string regionFile = "myRegion.csv";             //  our own location and range (region center and radius [nm])

    private const string inputDirName = "input";
    private const string basestationFile = "BaseStation.sqb";     // https://data.flightairmap.com/
    private const string icaoAddFile = "ICAO-AircraftAddon.csv";  // our own addon data for the aircrafts database
    private const string airportsFile = "airports.csv";           // http://ourairports.com/data/
    private const string routesFile = "routes.tsv";               // https://data.flightairmap.com/
    private const string navaidsFile = "navaids.csv";             // http://ourairports.com/data/
    private const string xAirwaysFile = "earth_awy.dat";          // XPlane V11 default_data folder
    private const string xFixesFile = "earth_fix.dat";            // XPlane V11 default_data folder
    private const string xNavsFile = "earth_nav.dat";             // XPlane V11 default_data folder
    private const string actFile = "ICAO-AircraftTypes.json";     // https://www.icao.int/publications/DOC8643 use http response...

    // OUTPUT names
    private const string outputDirName = "output";
    private const string faDbDirName = "db";  // the FA compatible DB path
    private static string actDbDirName = Path.Combine( faDbDirName, "aircraft_types" );  // the FA compatible DB path
    private const string actypesFile = "icao_aircraft_types.json";

    private const string aircraftsDBFile = "dump1090fa-aircrafts.sqb";
    private const string flightsDBFile = "dump1090fa-flights.sqb";
    private const string geoVorDmeFile = "nav-vordme-region.geojson";
    private const string geoNdbFile = "nav-ndb-region.geojson";
    private const string geoAptMainFile = "apt-midlarge-region.geojson";
    private const string geoAptOtherFile = "apt-others-region.geojson";
    private const string geoAwyLoFile = "navx-awy-lo-region.geojson";
    private const string geoAwyHiFile = "navx-awy-hi-region.geojson";
    private const string geoAwyFixFile = "navx-awy-fix-region.geojson";

    // other vars used throughout
    private static string inputError = "";
    private static string buildDir = "";
    private static string inputDir = "";
    private static string outputDir = "";
    private static string buildTS = "build-" + DateTime.Now.ToString( DATETIME ); // output folder timestamp as dir name
    private static bool argFAdb = false; // true if -fa was given in CmdLine

    // region data from file
    private static double ogLat = 0.0;
    private static double ogLon = 0.0;
    private static double ogRad = -1.0;

    // in memory databases
    private static icaoDatabase IDB = new icaoDatabase( );
    private static acDatabase ACDB = new acDatabase( );
    private static apDatabase APDB = new apDatabase( );
    private static rtDatabase RTDB = new rtDatabase( );
    private static navLib.navDatabase NAVDB = new navLib.navDatabase( );
    private static xNavLib.navDatabase XNAVDB = new xNavLib.navDatabase( );
    private static awyDatabase XAWYDB = new awyDatabase( );
    private static icaoActDatabase ACTDB = new icaoActDatabase( );

    // Job eval result (if true we can do it)
    private static bool jobAircrafts = false;     // create AC data
    private static bool jobAircraftsBS = false;   // read basestation
    private static bool jobAircraftsAddon = false;// read addon
    private static bool jobAircraftsFA = false;   // create FA JSON DB
    private static bool jobAircraftTypes = false; // create FA aircraft types JSON DB
    private static bool jobFlights = false;
    private static bool jobNavs = false;
    private static bool jobAirports = false;
    private static bool jobAirways = false;


    // Check for the main folder and return false if not found
    private static bool CheckBuildFolder( string arg )
    {
      if ( !Directory.Exists( arg ) ) {
        inputError = $" build folder not found: {arg}";
        return false;
      }
      buildDir = arg;
      return true;
    }



    // Command line arg checker
    private static Queue<string> cmds = new Queue<string>( );

    private static bool CheckArgs( string[] args )
    {
      bool retVal = true;
      // push up
      for ( int i = 0; i < args.Length; i++ ) {
        cmds.Enqueue( args[i] );
      }

      // check Input
      if ( cmds.Count < 1 ) {
        inputError = "missing build path";
        return false; // ERROR EXIT
      }

      string arg = cmds.Dequeue( );
      if ( arg == "-fa" ) {
        argFAdb = true;
        arg = cmds.Dequeue( );
      }
      retVal = CheckBuildFolder( arg );



      if ( !retVal ) return false; // ERROR EXIT
      return true; // seems ok ?!
    }



    // derives Lat/Lon/Range from the myRegion.csv file
    // format is Header<NL>Data (Lat;Lon;Range as decimal numbers)
    private static bool GetLatLonRad()
    {
      bool ok = true;

      string fName = Path.Combine( buildDir, regionFile );
      if ( !File.Exists( fName ) ) return false;
      using ( var sr = new StreamReader( fName ) ) {
        try {
          // brute force.. either it works - or not...
          string buffer = sr.ReadLine( ); // header line
          buffer = sr.ReadLine( ); // there is only one data line relevant..
          string[] e = buffer.Split( new char[] { ',',';' } ); // comma or semi separated..
          ogLat = double.Parse( e[0] );
          ogLon = double.Parse( e[1] );
          ogRad = double.Parse( e[2] );
        }
        catch {
          ok = false;
        }
      }
      return ok;
    }

    // Collect Aircraft data from BaseStation.sqb
    // Write into web aircraft database
    private static void DoAircraftJob()
    {
      if ( !jobAircrafts ) return;
      Console.WriteLine( $"\nCreating aircraft database .." );

      if ( jobAircraftsBS ) {
        string file = Path.Combine( inputDir, basestationFile );
        var ir = new BaseStation( );
        if ( !ir.Connect( file ) ) {
          Console.WriteLine( $"ERROR Reading BaseStation SQB database file: {file} - cannot open database" );
        }
        else {
          Console.WriteLine( $"Reading BaseStation SQB database file: {file}" );
          Console.WriteLine( ir.ReadDb( ref IDB ) );
          Console.WriteLine( $"DONE - ICAO ModeS Database contains {IDB.Count} records\n" );
        }
      }

      if ( jobAircraftsAddon ) {
        string file = Path.Combine( inputDir, icaoAddFile );
        var ir = new icaoCsvReader( );
        Console.WriteLine( $"Reading ICAO Aircraft Addon CSV file: {file}" );
        Console.WriteLine( ir.ReadDb( ref IDB, file ) );
        Console.WriteLine( $"DONE - ICAO ModeS Database Database contains now {IDB.Count} records\n" );
      }

      // Create now
      if ( IDB.Count <= 0 ) {
        Console.WriteLine( $"ERROR - No Aircraft records found, load populated BaseStation first\n" );
        return;
      }
      var iw = new AircraftsDB( );
      Console.WriteLine( iw.CreateDB( Path.Combine( outputDir, aircraftsDBFile ) ) );
      Console.WriteLine( iw.LoadDBfromIcao( IDB ) );
      Console.WriteLine( $"SQDB ICAO ModeS file written (or not if previously was an error...)" );
    }


    // Collect Aircraft data from BaseStation.sqb
    // Write into web aircraft dump1090fa compatible JSON database
    private static void DoAircraftFAJob()
    {
      if ( !jobAircraftsFA ) return;
      Console.WriteLine( $"\nCreating dump1090fa compatible aircraft database .." );

      if ( IDB.Count <= 0 ) {
        DoAircraftJob( );
      }
      // Create now
      if ( IDB.Count <= 0 ) {
        Console.WriteLine( $"ERROR - No Aircraft records found, load populated BaseStation first\n" );
        return;
      }
      string folder = Path.Combine( outputDir, faDbDirName );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      var iw = new icaoDbWriter( );
      iw.WriteDb( IDB, folder );
      Console.WriteLine( $"JsonDB ModeS written" );
    }


    // Collect Aircraft data from ICAO-AircraftTypes.json
    // Write into web aircraft types dump1090fa compatible JSON database
    private static void DoAircraftTypesJob()
    {
      if ( !jobAircraftTypes ) return;
      Console.WriteLine( $"\nCreating dump1090fa compatible aircraft types database .." );

      string file = Path.Combine( inputDir, actFile );
      var ir = new icaoActReader( );
      Console.WriteLine( $"Reading ICAO Aircraft Types json file: {file}" );
      Console.WriteLine( ir.ReadDb( ref ACTDB, file ) );
      Console.WriteLine( $"DONE - ICAO Aircraft Types Database contains {ACTDB.Count} records\n" );
      // Create now
      if ( ACTDB.Count <= 0 ) {
        Console.WriteLine( $"ERROR - No Aircraft Type records found, load populated ICAO Aircraft Types first\n" );
        return;
      }
      string folder = Path.Combine( outputDir, actDbDirName );
      if ( !Directory.Exists( folder ) )
        Directory.CreateDirectory( folder );
      var iw = new icaoActDbWriter( );
      iw.WriteDb( ACTDB, Path.Combine( folder, actypesFile ) );
      Console.WriteLine( $"JsonDB Aircraft Types written" );
    }


    private static void DoFlightsJob()
    {
      if ( !jobFlights ) return;
      Console.WriteLine( $"\nCreating flights database .." );

      string file = Path.Combine( inputDir, airportsFile );
      var ir = new apCsvReader( );
      Console.WriteLine( $"Reading Airport CSV: {file}" );
      Console.WriteLine( ir.ReadDb( ref APDB, file ) );
      Console.WriteLine( $"DONE - Airports Database contains {APDB.Count} records\n" );

      file = Path.Combine( inputDir, routesFile );
      var tr = new rtTsvReader( );
      Console.WriteLine( $"Reading Route TSV: {file}" );
      Console.WriteLine( tr.ReadDb( ref RTDB, file ) );
      Console.WriteLine( $"DONE - Route Database contains {RTDB.Count} records\n" );

      // Create now
      if ( APDB.Count <= 0 ) {
        Console.WriteLine( $"ERROR - No Airports found, provide a populated airports file\n" );
        return;
      }
      if ( RTDB.Count <= 0 ) {
        Console.WriteLine( $"ERROR - No Routes found, provide a populated routes file\n" );
        return;
      }

      var iw = new FlightsDB( );
      Console.WriteLine( iw.CreateDB( Path.Combine( outputDir, flightsDBFile ) ) );
      Console.WriteLine( iw.LoadDBfromAirports( APDB ) );
      Console.WriteLine( iw.LoadDBfromARoutes( RTDB ) );
      Console.WriteLine( $"SQDB Flights file written (or not if previously was an error...)" );
    }


    private static void DoNavsJob()
    {
      if ( !jobNavs ) return;
      Console.WriteLine( $"\nCreating nav locations .." );

      string file = Path.Combine( inputDir, navaidsFile );
      var ir = new navLib.navCsvReader( );
      Console.WriteLine( $"Reading Navaid CSV: {file}" );
      Console.WriteLine( ir.ReadDb( ref NAVDB, file ) );
      Console.WriteLine( $"DONE - Navaids Database contains {NAVDB.Count} records\n" );
      // Create now
      var iw = new navLib.navGeoWriter( );
      // limit by range and also by type
      // making two sets here, one for NDBs and one for all others
      using ( var outS = File.Create( Path.Combine( outputDir, geoNdbFile ) ) ) {
        iw.WriteGeoJson( NAVDB, outS, ogRad, ogLat, ogLon,
        new navLib.navRec.NavTypes[] { navLib.navRec.NavTypes.NDB, navLib.navRec.NavTypes.NDB_DME } );
        Console.WriteLine( $"{geoNdbFile} written" );
      }
      using ( var outS = File.Create( Path.Combine( outputDir, geoVorDmeFile ) ) ) {
        iw.WriteGeoJson( NAVDB, outS, ogRad, ogLat, ogLon,
        new navLib.navRec.NavTypes[] { navLib.navRec.NavTypes.DME, navLib.navRec.NavTypes.TACAN,
                                                navLib.navRec.NavTypes.VOR, navLib.navRec.NavTypes.VORTAC,
                                                navLib.navRec.NavTypes.VOR_DME, navLib.navRec.NavTypes.Other } );
        Console.WriteLine( $"{geoVorDmeFile} written" );
      }
    }

    private static void DoAirportsJob()
    {
      if ( !jobAirports ) return;
      Console.WriteLine( $"\nCreating airport locations .." );

      if ( APDB.Count <= 0 ) {
        string file = Path.Combine( inputDir, airportsFile );
        var ir = new apCsvReader( );
        Console.WriteLine( $"Reading Airport CSV: {file}" );
        Console.WriteLine( ir.ReadDb( ref APDB, file ) );
        Console.WriteLine( $"DONE - Airports Database contains {APDB.Count} records\n" );
      }
      // Create now
      var iw = new apGeoWriter( );
      // limit by range and also by type
      // making two sets here, one for mid-large Apts and one for all others
      using ( var outS = File.Create( Path.Combine( outputDir, geoAptMainFile ) ) ) {
        iw.WriteGeoJson( APDB, outS, ogRad, ogLat, ogLon,
        new apRec.AptTypes[] { apRec.AptTypes.medium_airport, apRec.AptTypes.large_airport } );
        Console.WriteLine( $"{geoAptMainFile} written" );
      }
      using ( var outS = File.Create( Path.Combine( outputDir, geoAptOtherFile ) ) ) {
        iw.WriteGeoJson( APDB, outS, ogRad, ogLat, ogLon,
        new apRec.AptTypes[] { apRec.AptTypes.balloonport, apRec.AptTypes.closed, apRec.AptTypes.heliport,
                                 apRec.AptTypes.seaplane_base, apRec.AptTypes.small_airport, apRec.AptTypes.Other } );
        Console.WriteLine( $"{geoAptOtherFile} written" );
      }
    }

    private static void DoAirwaysJob()
    {
      if ( !jobAirways ) return;
      Console.WriteLine( $"\nCreating airway layouts .." );

      string file = Path.Combine( inputDir, xNavsFile );
      var ir = new navReader( );
      Console.WriteLine( $"Reading XP11 Nav DAT: {file}" );
      Console.WriteLine( ir.ReadDb( ref XNAVDB, file ) );
      Console.WriteLine( $"DONE - X Nav Database contains {XNAVDB.Count} record\ns" );

      file = Path.Combine( inputDir, xFixesFile );
      var fr = new fixReader( );
      Console.WriteLine( $"Reading XP11 Fix DAT: {file}" );
      Console.WriteLine( fr.ReadDb( ref XNAVDB, file ) );
      Console.WriteLine( $"DONE - X Nav Database contains now {XNAVDB.Count} records\n" );

      file = Path.Combine( inputDir, xAirwaysFile );
      var ar = new awyReader( );
      Console.WriteLine( $"Reading XP11 Awy DAT: {file}" );
      Console.WriteLine( ar.ReadDb( ref XAWYDB, file ) );
      Console.WriteLine( $"DONE - X Airways Database contains {XAWYDB.Count} records\n" );
      // Create now
      Console.WriteLine( $"\nStarting to calculate and create airway files (this may take some minutes....)" );
      var iw = new xNavLib.navGeoWriter( );
      // limit by range and also by type
      var ia = new awyGeoWriter( );
      using ( var outS = File.Create( Path.Combine( outputDir, geoAwyHiFile ) ) ) {
        ia.WriteGeoJson( XAWYDB, XNAVDB, outS, "2", ogRad, ogLat, ogLon ); // layer hi
        Console.WriteLine( $"{geoAwyHiFile} written" );
      }

      using ( var outS = File.Create( Path.Combine( outputDir, geoAwyLoFile ) ) ) {
        ia.WriteGeoJson( XAWYDB, XNAVDB, outS, "1", ogRad, ogLat, ogLon ); // layer lo
        Console.WriteLine( $"{geoAwyLoFile} written" );
      }

      using ( var outS = File.Create( Path.Combine( outputDir, geoAwyFixFile ) ) ) {
        ia.WriteGeoJsonUsedMarkers( XNAVDB, outS );
        Console.WriteLine( $"{geoAwyFixFile} written" );
      }

      Console.WriteLine( $"GeoJson Navaids file written" );
    }


    // do all possible jobs
    static void DoJobs()
    {
      Console.WriteLine( $"Starting jobs.." );
      DoAircraftJob( );
      DoAircraftFAJob( );
      DoAircraftTypesJob( );
      DoFlightsJob( );
      DoNavsJob( );
      DoAirportsJob( );
      DoAirwaysJob( );
    }


    // get a input and create output structure
    static void MakeJoblist()
    {

      // get lat/lon/rad
      Console.WriteLine( $"Read myRegion.." );
      if ( !GetLatLonRad( ) ) {
        Console.WriteLine( $"ERROR reading 'myRegion.csv' - cannot continue" );
        return;
      }

      // Make outdir if needed
      Console.WriteLine( $"Creating output folders.." );
      try {
        outputDir = Path.Combine( buildDir, outputDirName );
        if ( !Directory.Exists( outputDir ) ) Directory.CreateDirectory( outputDir );
        outputDir = Path.Combine( outputDir, buildTS ); // make a timestamped subdir
        if ( !Directory.Exists( outputDir ) ) Directory.CreateDirectory( outputDir );
      }
      catch {
        Console.WriteLine( $"ERROR cannot create output folders - cannot continue" );
        return;
      }

      // check input files
      Console.WriteLine( $"Checking input files.." );
      inputDir = Path.Combine( buildDir, inputDirName );

      // create dump1090fa-aircrafts.sqb
      //  - needs input\BaseStation.sqb and/or ICAO-AircraftAddon.csv (our own addons)
      jobAircraftsAddon = File.Exists( Path.Combine( inputDir, icaoAddFile ));
      jobAircraftsBS = File.Exists( Path.Combine( inputDir, basestationFile ) );
      jobAircrafts = jobAircraftsBS || jobAircraftsAddon;
      jobAircraftsFA = jobAircrafts && argFAdb; // must create FA db if we can to aircrafts only

      // create dump1090fa-aircrafts.sqb
      //  - needs input\BaseStation.sqb
      jobAircrafts = File.Exists( Path.Combine( inputDir, basestationFile ) );
      jobAircraftsFA = jobAircrafts && argFAdb; // must create FA db

      // create icao_aircraft_types.json
      //  - needs input\ICAO-AircraftTypes.json
      jobAircraftTypes = File.Exists( Path.Combine( inputDir, actFile ) );

      // create dump1090fa-flights.sqb
      //  - needs input\airports.csv
      //  - needs input\routes.tsv
      jobFlights = File.Exists( Path.Combine( inputDir, airportsFile ) )
                    && File.Exists( Path.Combine( inputDir, routesFile ) );

      // create nav-ndb-region.geojson, nav-vordme-region.geojson
      //  - needs input\navaids.csv
      jobNavs = File.Exists( Path.Combine( inputDir, navaidsFile ) );

      // create apt-midlarge-region.geojson, apt-others-region.geojson
      //  - needs input\airports.csv
      jobAirports = File.Exists( Path.Combine( inputDir, airportsFile ) );

      // create navx-awy-lo-region.geojson, navx-awy-hi-region.geojson, navx-awy-fix-region.geojson
      //  - needs input\earth_awy.dat 
      //  - needs input\earth_nav.dat
      //  - needs input\earth_fix.dat
      jobAirways = File.Exists( Path.Combine( inputDir, xAirwaysFile ) )
                  && File.Exists( Path.Combine( inputDir, xNavsFile ) )
                  && File.Exists( Path.Combine( inputDir, xFixesFile ) );
    }


    static void Main( string[] args )
    {
      if ( !CheckArgs( args ) ) {
        Console.WriteLine( "ERROR command line error - cannot continue" );
        Console.WriteLine( inputError );
        Console.WriteLine( );
        DumpHelp( );
        Environment.ExitCode = -1;
        return;
      }
      // we shall continue...
      // Establish job list
      Console.WriteLine( "About to start processing input files ...." );
      MakeJoblist( );
      DoJobs( );

      Console.WriteLine( );
      Console.WriteLine( "DONE....." );
    }

  }
}
