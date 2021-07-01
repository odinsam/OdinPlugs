using System;
using System.Collections.Generic;
using System.Linq;

namespace OdinPlugs.OdinUtils.OdinAlgorithm.OdinRandom
{
    /// <summary>
    /// 随机算法类
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// ~ 随机种子值
        /// </summary>
        /// <returns></returns> 
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }


        /// <summary>
        /// ~  按权重返回对应需要个数的数组
        /// </summary>
        /// <param name="list">受权重影响的数组</param>
        /// <param name="count">需要返回的个数</param>
        /// <typeparam name="T">权重对象的类型，唯一标识</typeparam>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<T, int>> GetRandomListByWeight<T>(List<KeyValuePair<T, int>> list, int count)
        {
            if (list == null)
            {
                throw new Exception("list不能为null");
            }
            if (count <= 0)
            {
                throw new Exception("list.count不能 小于等于 = ");
            }
            if (list.Count <= count)
            {
                return list;
            }
            //计算权重总和
            var totalWeights = list.Sum(ls => ls.Value);
            //随机赋值权重
            Random ran = new Random(RandomHelper.GetRandomSeed());  //GetRandomSeed()随机种子，防止快速频繁调用导致随机一样的问题 
            List<KeyValuePair<T, int>> weightlst = new List<KeyValuePair<T, int>>();
            for (int i = 0; i < list.Count; i++)
            {
                int w = (list[i].Value + 1) + ran.Next(0, totalWeights);   // （权重+1） + 从0到（总权重-1）的随机数
                weightlst.Add(new KeyValuePair<T, int>(list[i].Key, w));
            }
            //排序
            weightlst.Sort(
              delegate (KeyValuePair<T, int> kvp1, KeyValuePair<T, int> kvp2)
              {
                  return kvp2.Value - kvp1.Value;
              });
            return weightlst.Take(count);
        }
    }
}