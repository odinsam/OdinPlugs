using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OdinPlugs.OdinUtils.OdinAlgorithm.OdinString
{
    public class GenerationCodeHelper
    {
        private static List<string> lstNumber = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        private static List<string> lstValidateCodeNumber = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9" };
        private static List<string> lstUpperLetter = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private static List<string> lstValidateCodeUpperLetter = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private static List<string> lstLowerLetter = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private static List<string> lstValidateCodeLowerLetter = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };



        /// <summary>
        /// 按长度生成对应的字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="containNum">是否包含数据</param>
        /// <param name="containLetter">是否包含字母</param>
        /// <returns>生成的字符串</returns>
        public static string GenerationCode(int length, bool containNum = true, bool containLetter = true)
        {
            List<string> code = new List<string>();
            if (containNum)
                code = code.Union(lstNumber).ToList();
            if (containLetter)
                code = code.Union(lstUpperLetter).Union(lstLowerLetter).ToList();
            List<string> result = new List<string>();
            for (int i = 0; i < length; i++)
            {
                int index = new Random((int)DateTime.Now.Ticks).Next(0, code.Count);
                result.Add(code[index]);
                Thread.Sleep(10);
            }
            return string.Join("", result);
        }

        /// <summary>
        /// 不包含 0 o 1 I 等容易混淆的之母和数字
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="containNum">是否包含数据</param>
        /// <param name="containLetter">是否包含字母</param>
        /// <returns>生成的字符串</returns>
        public static string GenerationlimpidCode(int length, bool containNum = true, bool containLetter = true)
        {
            List<string> code = new List<string>();
            if (containNum)
                code = code.Union(lstValidateCodeNumber).ToList();
            if (containLetter)
                code = code.Union(lstValidateCodeUpperLetter).Union(lstValidateCodeLowerLetter).ToList();
            List<string> result = new List<string>();
            for (int i = 0; i < length; i++)
            {
                int index = new Random((int)DateTime.Now.Ticks).Next(0, code.Count);
                result.Add(code[index]);
                Thread.Sleep(10);
            }
            return string.Join("", result);
        }

    }
}