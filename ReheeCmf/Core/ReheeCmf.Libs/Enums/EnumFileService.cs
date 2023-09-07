using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Enums
{
  public enum EnumFileService
  {
    None = 0,
    Local = 100,
    Dropbox = 200,
    OneDrive = 300,
    GoogleDrive = 400,
    AwsS3 = 500,
    AzureBlob = 600
  }
}
