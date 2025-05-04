using MusicInstrumentLibr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class HashNode<T> where T : ICloneable
    {
        public string Key { get; set; }
        public T Value { get; set; }
        public bool IsDeleted { get; set; }

        public HashNode(string key, T value)
        {
            Key = key;
            Value = value;
            IsDeleted = false;
        }

        public override string ToString()
        {
            return $"Key: {Key}, Value: {Value}";
        }

        public override int GetHashCode()
        {
            int code = 0;
            if (Key != null)
            {
                foreach (char c in Key)
                    code += c;
            }
            return code;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            HashNode<T> other = (HashNode<T>)obj;
            return Key == other.Key;
        }
    }
}
