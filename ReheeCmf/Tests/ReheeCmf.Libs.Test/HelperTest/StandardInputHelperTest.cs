using ReheeCmf.Enums;
using ReheeCmf.Utility.CmfRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.HelperTest
{
  internal class StandardInputHelperTest
  {
    [SetUp]
    public virtual void Setup()
    {
      CmfRegister.Init();
    }
    [Test]
    public void PropertyUpdateTest()
    {
      var value = new StandardInputHelperClass();
      var dictionary = new Dictionary<string, string?>()
      {
        ["StringValue"] = "1",
        ["StringValueNull"] = "2",
        ["NullInt"] = "a",
        ["Int"] = ""
      };
      StandardInputHelper.UpdateProperty(value, dictionary);
    }
    [TestCaseSource(nameof(StringToObjectValue_Test_Data))]
    public void StringToObjectValue_Test(string? value, Type type, bool expectSuccess, object? expectValue
      )
    {
      var result = StringValueHelper.GetObjValue(value, type);
      Assert.That(result.Success, Is.EqualTo(expectSuccess), () => value);
      if (expectSuccess)
      {
        Assert.That(result.Content, Is.EqualTo(expectValue), () => value);
      }
    }
    private static TestCaseData[] StringToObjectValue_Test_Data = new TestCaseData[]
    {
      new TestCaseData(
        "1",typeof(int),true,1
        ),
      new TestCaseData(
        "a",typeof(int?),true,null
        ),
      new TestCaseData(
        "true",typeof(bool),true,true
        ),
      new TestCaseData(
        "tru1e",typeof(bool),false,true
        ),
       new TestCaseData(
        "Get",typeof(EnumHttpMethod),true,EnumHttpMethod.Get
        ),
       new TestCaseData(
        "2022-01-01",typeof(DateTime),true,new DateTime(2022,01,01)
        ),
       new TestCaseData(
        "2022-01-02",typeof(DateTimeOffset),true, new DateTimeOffset(new DateTime(2022,01,02))
        ),
       new TestCaseData(
        "[1,2,3]",typeof(int[]),true, new int[]{ 1,2,3 }
        ),
       new TestCaseData(
        "Get",typeof(EnumHttpMethod?),true,EnumHttpMethod.Get
        ),
    };

    [TestCaseSource(nameof(GetStrValue_Test_Data))]
    public void GetStrValue_Test(object? value, Type type, bool expectSuccess, string? expectValue)
    {
      var response = StringValueHelper.GetStrValue(value, type);
      Assert.That(response.Success, Is.EqualTo(expectSuccess));
      if (expectSuccess)
      {
        Assert.That(response.Content, Is.EqualTo(expectValue));
      }
    }
    private static TestCaseData[] GetStrValue_Test_Data = new TestCaseData[]
    {
      new TestCaseData(
        new DateTime(2001,01,1),typeof(DateTime),true,"2001-01-01T00:00:00.000Z"
        ),
      new TestCaseData(
        new DateTime[]{ new DateTime(2001,01,1) ,new DateTime(2001,01,1) },typeof(DateTime[]),true,"""["2001-01-01T00:00:00.000Z","2001-01-01T00:00:00.000Z"]"""
        ),
      new TestCaseData(
        new DateTime(2001,01,1),typeof(DateTime?),true,"2001-01-01T00:00:00.000Z"
        ),
      new TestCaseData(
        null,typeof(DateTime),false,"2001-01-01T00:00:00.000Z"
        ),
      new TestCaseData(
        null,typeof(DateTime?),true,null
        ),
      new TestCaseData(
        EnumHttpMethod.Get,typeof(EnumHttpMethod),true,"Get"
        ),
      new TestCaseData(
        new EnumHttpMethod[]{ EnumHttpMethod.Get,EnumHttpMethod.Get },typeof(EnumHttpMethod[]),true,"""["Get","Get"]"""
        )
    };
  }


  file class StandardInputHelperClass
  {
    public string StringValue { get; set; }
    public string? StringValueNull { get; set; }
    public int? NullInt { get; set; }
    public int Int { get; set; }
  }
}
