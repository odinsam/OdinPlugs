using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace OdinPlugs.OdinNetCore.OdinJson
{
    public class JObjectList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private JArray jary;
        public JObjectList()
        {
            jary = new JArray();
        }

        public JObjectList(JArray _jary)
        {
            jary = _jary;
        }

        public T this[int index] { get => (T)Convert.ChangeType(jary[index], typeof(T)); set => jary[index] = JObject.FromObject(value); }

        public int Count => jary.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            jary.Add(JObject.FromObject(item));
        }

        public void AddRangs(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                jary.Add(JObject.FromObject(item));
            }
        }

        public void Clear()
        {
            jary.Clear();
        }

        public bool Contains(T item)
        {
            return jary.Contains(JObject.FromObject(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<T> jaryEnum = (IEnumerable<T>)jary.ToObject(typeof(IEnumerable<T>));
            return jaryEnum.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return jary.IndexOf(JObject.FromObject(item));
        }

        public void Insert(int index, T item)
        {
            jary.Insert(index, JObject.FromObject(item));
        }

        public bool Remove(T item)
        {
            return jary.Remove(JObject.FromObject(item));
        }

        public void RemoveAt(int index)
        {
            jary.RemoveAt(index);
        }

        public JArray ToJArray()
        {
            return (JArray)jary.ToObject(typeof(JArray));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // IEnumerator<T> IEnumerable<T>.GetEnumerator()
        // {
        //     return ((IEnumerable<T>)jary.ToObject(typeof(IEnumerable<T>))).GetEnumerator();
        // }
    }

}