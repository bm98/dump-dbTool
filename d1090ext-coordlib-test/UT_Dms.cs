using System;
using d1090dataLib.d1090ext_coordlib;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace d1090ext_coordlib_test
{
  [TestClass]
  public class UT_Dms
  {
    #region ParseDMS
    [TestMethod]
    public void T_ParseDMS01()
    {
      Dms.Separator = "";
      double ret = Dms.ParseDMS( "51° 28′ 40.12″ N" );
      Assert.AreEqual<double>( 51.477811, Math.Round( ret, 6 ) );
    }

    [TestMethod]
    public void T_ParseDMS02()
    {
      Dms.Separator = "";
      double ret = Dms.ParseDMS( "51° 28′ N" );
      Assert.AreEqual<double>( 51.466667, Math.Round( ret, 6 ) );
    }

    [TestMethod]
    public void T_ParseDMS03()
    {
      Dms.Separator = "";
      double ret = Dms.ParseDMS( "51°  N" );
      Assert.AreEqual<double>( 51.0, Math.Round( ret, 6 ) );
    }


    [TestMethod]
    public void T_ParseDMS11()
    {
      Dms.Separator = "";
      double ret = Dms.ParseDMS( "000° 00′ 05.31″ W" );
      Assert.AreEqual<double>( -0.001475, Math.Round( ret, 6 ) );
    }

    [TestMethod]
    public void T_ParseDMS12()
    {
      Dms.Separator = "";
      double ret = Dms.ParseDMS( "015° 06′ W" );
      Assert.AreEqual<double>( -15.1, Math.Round( ret, 6 ) );
    }

    [TestMethod]
    public void T_ParseDMS13()
    {
      Dms.Separator = "";
      double ret = Dms.ParseDMS( "128°  W" );
      Assert.AreEqual<double>( -128.0, Math.Round( ret, 6 ) );
    }

    #endregion ParseDMS

    #region ToDMS
    [TestMethod]
    public void T_ToDMS01()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.477811, "dms" );
      Assert.AreEqual<string>( "051°28′40″", ret );
    }

    [TestMethod]
    public void T_ToDMS02()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.477811, "dms", 2 );
      Assert.AreEqual<string>( "051°28′40.12″", ret );
    }

    [TestMethod]
    public void T_ToDMS03()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.477811, "dm" );
      Assert.AreEqual<string>( "051°28.67′", ret );
    }

    [TestMethod]
    public void T_ToDMS04()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.477811, "d" );
      Assert.AreEqual<string>( "051.4778°", ret );
    }

    [TestMethod]
    public void T_ToDMS05()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.477811, "d", 2 );
      Assert.AreEqual<string>( "051.48°", ret );
    }

    [TestMethod]
    public void T_ToDMS06()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.477811, "d", 0 );
      Assert.AreEqual<string>( "051°", ret );
    }

    [TestMethod]
    public void T_ToDMS07()
    {
      Dms.Separator = "";
      string ret = Dms.ToDMS( 51.5, "d", 0 );
      Assert.AreEqual<string>( "052°", ret );
    }
    #endregion ToDMS

    #region ToLat ToLon

    [TestMethod]
    public void T_ToLat01()
    {
      Dms.Separator = "";
      string ret = Dms.ToLat( 51.477811, "dms" );
      Assert.AreEqual<string>( "51°28′40″N", ret );
    }

    [TestMethod]
    public void T_ToLat02()
    {
      Dms.Separator = "";
      string ret = Dms.ToLat( -51.477811, "dms" );
      Assert.AreEqual<string>( "51°28′40″S", ret );
    }

    [TestMethod]
    public void T_ToLon01()
    {
      Dms.Separator = " ";
      string ret = Dms.ToLon( -0.001475, "dms", 2 );
      Assert.AreEqual<string>( "000° 00′ 05.31″ W", ret );
    }

    [TestMethod]
    public void T_ToLon02()
    {
      Dms.Separator = " ";
      string ret = Dms.ToLon( 128.001475, "dms", 2 );
      Assert.AreEqual<string>( "128° 00′ 05.31″ E", ret );
    }
    #endregion ToLat ToLon

    [TestMethod]
    public void T_CompassPoint01()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 0 );
      Assert.AreEqual<string>( "N", ret );
    }

    [TestMethod]
    public void T_CompassPoint02()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 90 );
      Assert.AreEqual<string>( "E", ret );
    }

    [TestMethod]
    public void T_CompassPoint03()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 180 );
      Assert.AreEqual<string>( "S", ret );
    }

    [TestMethod]
    public void T_CompassPoint04()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 270 );
      Assert.AreEqual<string>( "W", ret );
    }

    [TestMethod]
    public void T_CompassPoint11()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 044 );
      Assert.AreEqual<string>( "NE", ret );
    }

    [TestMethod]
    public void T_CompassPoint12()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 012 );
      Assert.AreEqual<string>( "NNE", ret );
    }

    [TestMethod]
    public void T_CompassPoint13()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 036, 2 );
      Assert.AreEqual<string>( "NE", ret );
    }

    [TestMethod]
    public void T_CompassPoint14()
    {
      Dms.Separator = " ";
      string ret = Dms.CompassPoint( 036, 1 );
      Assert.AreEqual<string>( "N", ret );
    }



  }
}
