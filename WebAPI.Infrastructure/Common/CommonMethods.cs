using System;
using System.Net;

namespace WebAPI.Infrastructure.Common
{
    public static class CommonMethods
    {
        public static bool IsNotNull(this object value)
        {
            return value != DBNull.Value && value != null;
        }
        /// <summary>
        /// Common function to check Directory exists or not
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static bool DoesFtpDirectoryExist(string dirPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
