using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using d1090dataLib.d1090fa_dblib;

namespace d1090fa_dblib.test
{
  /// <summary>
  /// Summary description for UT_IcaoRec
  /// </summary>
  [TestClass]
  public class UT_IcaoRec
  {
    [TestMethod]
    public void T_cTor1()
    {
      var retVal = new icaoRec( "01AA10", "V5-NAM", "F900" );
      Assert.AreEqual<string>( "01AA10", retVal.icao );
      Assert.AreEqual<string>( "V5-NAM", retVal.registration );
      Assert.AreEqual<string>( "F900", retVal.airctype );
    }



  }
}
