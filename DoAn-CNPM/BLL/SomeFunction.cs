using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public static class SomeFunction
    {
        public static string getFormatLyrics(string path, string lyrics)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path + "Assets/Lyrics/" + lyrics + ".txt");
                string text2 = "";
                foreach (string line in lines)
                {
                    text2 = text2 + "<br />" + line;
                }
                return text2;
            }
            catch
            {
                return null;
            }
        }

        public static string getFormatInfoArtist(string path, string info)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path + "Assets/Description/Artist/" + info + ".txt");
                string text2 = "";
                foreach (string line in lines)
                {
                    text2 = text2 + "<br />" + line;
                }
                return text2;
            }
            catch
            {
                return null;
            }
        }
        public static string ConvertToUnSign(string input)
        {
            input = input.Trim();
            input = input.ToLower();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        public static List<string> SeparateWords(string input)
        {
            List<string> listResult = new List<string>();
            input = input.Trim();
            int length = input.Length;
            if (length > 0)
            {
                

                return input.Split(' ').ToList(); ;
            }
            else
            {
                return null;
            }

        }
    }
}