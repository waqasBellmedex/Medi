using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
//using Tamir.SharpSsh;

namespace MediFusionPM.BusinessLogic
{
    public class SFTPSubmission
    {
        string _host = string.Empty;
        string _username = string.Empty;
        string _password = string.Empty;
        int _port = 22;
        string _connectivityType = string.Empty;
        public SFTPSubmission(string HostName, string UserName, string Password, int Port, string ConnectivityType)
        {
            _host = HostName;
            _username = UserName;
            _password = Password;
            _port = Port;
            _connectivityType = ConnectivityType;
        }
        public bool SubmitFile(string FTPDirectory, string FilePath)
        {
            SecurityProtocolType servicePointManager = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            bool succ = false;
            SftpClient client = null;
            try
            {
                // initialize client and connect like you normally would
                client = new SftpClient(_host, _port, _username, _password);
                client.Connect();

                // await a directory listing
                //var listing = await client.ListDirectoryAsync(".");

                // await a file upload
                using (var localStream = File.OpenRead(FilePath))
                {
                    client.UploadFile(localStream, FTPDirectory.Trim('/') + "/" + System.IO.Path.GetFileName(FilePath));
                    succ = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // disconnect like you normally would
                client.Disconnect();
                ServicePointManager.SecurityProtocol = servicePointManager;
            }
            return succ;
        }

        public bool DownloadFiles(string FTPDirectory, string OutputDirectory)
        {
            SecurityProtocolType servicePointManager = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            bool succ = false;

            try
            {
                using (var sftp = new SftpClient(_host, _port, _username, _password))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(FTPDirectory);

                    foreach (var file in files)
                    {
                        string remoteFileName = file.Name;
                        if (!file.Name.StartsWith(".") && !file.Name.StartsWith("..") && !file.Name.StartsWith("..."))
                        {
                            if (!file.Attributes.IsDirectory)
                            {
                                string fileToDownload = Path.Combine(OutputDirectory, remoteFileName);
                                using (Stream file1 = File.OpenWrite(fileToDownload))
                                {
                                    sftp.DownloadFile(FTPDirectory + remoteFileName, file1);
                                }
                            }
                        }
                    }
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ServicePointManager.SecurityProtocol = servicePointManager;
            }
            return succ;
        }


        public bool DeleteDownloadFiles(string FTPDirectory, string ArchiveDirectory, string OutputDirectory)
        {
            SecurityProtocolType servicePointManager = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                using (var sftp = new SftpClient(_host, _port, _username, _password))
                {
                    sftp.Connect();
                    foreach (string FilePath in Directory.GetFiles(OutputDirectory))
                    {
                        try
                        {
                            if (FilePath.Contains("alldownloads.zip")) continue;
                            // Deleting the file From Downlaod Directory
                            sftp.DeleteFile(FTPDirectory + Path.GetFileName(FilePath));

                            //if (!ArchiveDirectory.IsNull())
                            //{
                            //    // Archieving into Archive Directory
                            //    using (var localStream = File.OpenRead(FilePath))
                            //    {
                            //        sftp.UploadFile(localStream, ArchiveDirectory.Trim('/') + "/" + System.IO.Path.GetFileName(FilePath));
                            //    }
                            //}
                        }
                        catch (Exception)
                        {
                        }
                    }
                    sftp.Disconnect();
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally
            {
                ServicePointManager.SecurityProtocol = servicePointManager;
            }
            return true;
        }
    }
}