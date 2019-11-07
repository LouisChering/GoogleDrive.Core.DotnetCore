using Google.Apis.Drive.v3;
using System.Collections.Generic;

namespace DriveDotNet.Models
{
    public class FileResultContainer{
        public List<Google.Apis.Drive.v3.Data.File> files {get;set;}
        public List<Google.Apis.Drive.v3.Data.File> folders {get;set;}
    }
}
