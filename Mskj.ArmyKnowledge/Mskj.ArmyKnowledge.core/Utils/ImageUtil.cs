using System;
using System.Configuration;
using System.IO;

namespace Mskj.ArmyKnowledge.Core
{
    public class ImageUtil
    {
        /// <summary>
        /// 将上传的Base64图片转成文件的地址
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string StrToUri(String strBase64,string fileName)
        {
            string result = string.Empty;

            if(string.IsNullOrEmpty(strBase64))
            {
                return result;
            }

            string imagePath = ConfigurationManager.AppSettings["ImagePath"];
            string Localhost = ConfigurationManager.AppSettings["Localhost"] + @"/images/goods/";
            string filePath = Path.Combine(imagePath, fileName);
            byte[] bt = Convert.FromBase64String(strBase64);
            File.WriteAllBytes(filePath, bt);
            result = Localhost + fileName;

            return result;
        }

    }
}
