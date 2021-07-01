using System;
using System.IO;

namespace OdinPlugs.OdinUtils.OdinFiles
{
    public class FileHelper
    {
        /// <summary>
        /// 系统分隔符  win '\'  other '/'
        /// </summary>
        /// <returns></returns>
        public static string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();
        public static string GetFileBase64(String filePath)
        {
            using (FileStream filestream = new FileStream(filePath, FileMode.Open))
            {
                byte[] arr = new byte[filestream.Length];
                filestream.Read(arr, 0, (int)filestream.Length);
                string baser64 = Convert.ToBase64String(arr);
                filestream.Close();
                return baser64;
            }
        }
    }
}