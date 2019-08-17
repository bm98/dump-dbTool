using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using System.Collections.Generic;
using d1090dataLib.d1090fa_dblib;

namespace d1090fa_dblib.test
{
  [TestClass]
  public class UT_JsonParser
  {
    //   BF.Invoke( "ClearBitField", new Object[] { } );
    // var Xm_content = (Dictionary<string, string>)BF.GetFieldOrProperty( "m_content" );
    // PrivateObject BF = new PrivateObject( typeof( JasonParser ), new object[] { } );

    [TestMethod]
    public void T_Split1()
    {
      PrivateType BF = new PrivateType( typeof( JsonParser ) );

      string sIn = "a,b,c"; // test simple comma separation
      List<string> retVal = (List<string>)BF.InvokeStatic( "Split", new Object[] { sIn, ',' } );

      Assert.AreEqual<int>( 3, retVal.Count );
      Assert.AreEqual<string>( "a", retVal[0] );
      Assert.AreEqual<string>( "b", retVal[1] );
      Assert.AreEqual<string>( "c", retVal[2] );
    }

    [TestMethod]
    public void T_Split2()
    {
      PrivateType BF = new PrivateType( typeof( JsonParser ) );

      string sIn = "\"r\": \"V5-NAM\",\"t\": \"F900\""; // test comma separation
      List<string> retVal = (List<string>)BF.InvokeStatic( "Split", new Object[] { sIn, ',' } );

      Assert.AreEqual<int>( 2, retVal.Count );
      Assert.AreEqual<string>( "\"r\": \"V5-NAM\"", retVal[0] );
      Assert.AreEqual<string>( "\"t\": \"F900\"", retVal[1] );
    }

    [TestMethod]
    public void T_Split3()
    {
      PrivateType BF = new PrivateType( typeof( JsonParser ) );

      string sIn = "\"r\": \"V5-NAM\""; // test colon separation
      List<string> retVal = (List<string>)BF.InvokeStatic( "Split", new Object[] { sIn, ':' } );

      Assert.AreEqual<int>( 2, retVal.Count );
      Assert.AreEqual<string>( "\"r\"", retVal[0] );
      Assert.AreEqual<string>( " \"V5-NAM\"", retVal[1] );
    }

    [TestMethod]
    public void T_Split4()
    {
      PrivateType BF = new PrivateType( typeof( JsonParser ) );

      string sIn = "\"r\": \"V5-NAM\",\"t\": \"F9,00\""; // test comma separation with comma in string
      List<string> retVal = (List<string>)BF.InvokeStatic( "Split", new Object[] { sIn, ',' } );

      Assert.AreEqual<int>( 2, retVal.Count );
      Assert.AreEqual<string>( "\"r\": \"V5-NAM\"", retVal[0] );
      Assert.AreEqual<string>( "\"t\": \"F9,00\"", retVal[1] );
    }

    [TestMethod]
    public void T_Parser1()
    {
      string sIn = "\"01001\": {\"r\": \"V5-NAM\",\"t\": \"F900\"},";

      JsonRecord retVal = JsonParser.Decompose( sIn ); // removes decoration
      Assert.AreEqual<int>( 1, retVal.Count );
      Assert.AreEqual<bool>( true, retVal.ContainsKey( "01001" ) );
      Assert.AreEqual<bool>( true, retVal["01001"].ContainsKey( "r" ) );
      Assert.AreEqual<bool>( true, retVal["01001"].ContainsKey( "t" ) );
      Assert.AreEqual<string>( "V5-NAM", retVal["01001"]["r"] );
      Assert.AreEqual<string>( "F900", retVal["01001"]["t"] );
    }

    [TestMethod]
    public void T_RemoveApo1()
    {
      string sIn = "\"01001\"";

      string retVal = JsonParser.RemoveApo( sIn );
      Assert.AreEqual<string>( "01001", retVal );
    }

    [TestMethod]
    public void T_RemoveApo2()
    {
      string sIn = "  \"01001\""; // leading blanks

      string retVal = JsonParser.RemoveApo( sIn );
      Assert.AreEqual<string>( "01001", retVal );
    }


  }
}
