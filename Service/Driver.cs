using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.Linq;
using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Download;

using DriveDotNet.Models;
using DriveDotNet.Helpers;

namespace DriveDotNet.Service
{
    public class Driver
    {
        /// <summary>
        /// Get all of this from appsettings.json
        /// </summary>
        /// <value></value>
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Drive API .NET Quickstart";
        string rootFolderId = "0BzsfKrxUTl6TWkhsRWFOVERlSkE";
        string rootFolderName = "Tunes";
        UserCredential credential;
        DriveService service;
        FileHelper fileHelper;


        public Driver(string directory)
        {
            fileHelper = new FileHelper();
            using (var stream =
                new FileStream($"{directory}credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = $"{directory}token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            


            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public FileResultContainer GetFiles(){
            return GetFiles(rootFolderId,rootFolderName);
        }

        public FileResultContainer GetFiles(string id,string name)
        {
            id = string.IsNullOrWhiteSpace(id)? rootFolderId : id ;
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
             listRequest.Q = $"'{id}' in parents";

            listRequest.Fields = "nextPageToken, files(id, name,mimeType)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

            var viewModel = new FileResultContainer{
                files  = fileHelper.FindFiles(files),
                folders = fileHelper.FindFolders(files)
            };
            return viewModel;

        }

         public FileResultContainer SearchFiles(string id,string searchString)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            if(!string.IsNullOrWhiteSpace(id)){
                listRequest.Q = $"('{id}' in parents) and (name contains '{searchString}')";
            }else{
               listRequest.Q = $"(name contains '{searchString}')";
            }

            listRequest.Fields = "nextPageToken, files(id, name,mimeType)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

            var viewModel = new FileResultContainer{
                files  = fileHelper.FindFiles(files),
                folders = fileHelper.FindFolders(files)
            };
            return viewModel;

        }

        public Stream Download(string id){
        
            var finished = false;
            var request = service.Files.Get(id);
            var stream = new System.IO.MemoryStream();

            request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            finished = true;
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            finished = true;

                            break;
                        }
                }
            };
            request.Download(stream);

            while(!finished){

            }

             byte[] bytesInStream = stream.ToArray(); // simpler way of converting to array
             stream.Close(); 
            Stream feedStream = new MemoryStream(bytesInStream);
            return feedStream;
            //return File(feedStream, $"{fileType}"); // returns a FileStreamResult
        }

        public async Task<Stream> AsyncDownload(string id){
        
            var finished = false;
            var request = service.Files.Get(id);
            var stream = new System.IO.MemoryStream();

            request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            finished = true;
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            finished = true;

                            break;
                        }
                }
            };
            request.Download(stream);

            while(!finished){
                await PutTaskDelay(100);
            }

             byte[] bytesInStream = stream.ToArray(); // simpler way of converting to array
             stream.Close(); 
            Stream feedStream = new MemoryStream(bytesInStream);
            return feedStream;
            //return File(feedStream, $"{fileType}"); // returns a FileStreamResult
        }

        
        async Task PutTaskDelay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        } 

    }
}
