using System.Collections.Generic;
using Google.Apis.Drive.v3.Data;
using System.Linq;

namespace DriveDotNet.Helpers
{
    public class FileHelper
    {
        public FileHelper()
        {
            
        }

        /// <summary>
        /// Will take a list of 'files' and return any that are files by looking at the mime type
        /// </summary>
        /// <returns>A list of purely file items</returns>
        public List<Google.Apis.Drive.v3.Data.File> FindFiles(IList<Google.Apis.Drive.v3.Data.File> list){
            return list.Where(f => f.MimeType != "application/vnd.google-apps.folder").ToList();
        }

        /// <summary>
        /// Will take a list of 'files' and return any that are folders by looking at the mime type
        /// </summary>
        /// <returns>A list of purely folder items</returns>
        public List<Google.Apis.Drive.v3.Data.File> FindFolders(IList<Google.Apis.Drive.v3.Data.File> list){
            return list.Where(f => f.MimeType == "application/vnd.google-apps.folder").ToList();
        }
    }
}
