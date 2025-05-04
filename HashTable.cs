using MusicInstrumentLibr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class HashTable<T> : IEnumerable<KeyValuePair<string, T>> where T : MusicInstrument, ICloneable
    {
        private HashNode<T>[] table;
        private int size;
        private int count;
        private readonly double loadFactorThreshold = 1.0;

        public HashTable(int size = 5)
        {
            this.size = size;
            table = new HashNode<T>[size];
            count = 0;
        }

        public int Count => count;

        private int ComputeHash(string key)
        {
            int code = 0;
            if (key != null)
            {
                foreach (char c in key)
                    code += c;
            }
            return Math.Abs(code) % size;
        }

        private int Probe(int index, int attempt)
        {
            return (index + attempt) % size;
        }

        public bool Add(string key, T value)
        {
            if ((double)(count + 1) / size > loadFactorThreshold)
            {
                return false;
            }
            int index = ComputeHash(key);
            int attempt = 0;
            while (attempt < size)
            {
                int probeIndex = Probe(index, attempt);
                if (table[probeIndex] == null || table[probeIndex].IsDeleted)
                {
                    table[probeIndex] = new HashNode<T>(key, (T)value.Clone());
                    count++;
                    return true;
                }
                if (table[probeIndex].Key == key && !table[probeIndex].IsDeleted)
                {
                    return false;
                }
                attempt++;
            }
            return false;
        }

        public bool AddAtIndex(string key, T value, int index)
        {
            if (index < 0 || index >= size)
            {
                return false;
            }
            if ((double)(count + 1) / size > loadFactorThreshold)
            {
                return false;
            }
            if (table[index] == null || table[index].IsDeleted)
            {
                table[index] = new HashNode<T>(key, (T)value.Clone());
                count++;
                return true;
            }
            else if (table[index].Key == key && !table[index].IsDeleted)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public T Find(string key)
        {
            int index = ComputeHash(key);
            int attempt = 0;
            while (attempt < size)
            {
                int probeIndex = Probe(index, attempt);
                if (table[probeIndex] == null)
                {
                    return null;
                }
                if (table[probeIndex].Key == key && !table[probeIndex].IsDeleted)
                {
                    return (T)table[probeIndex].Value.Clone();
                }
                attempt++;
            }
            return null;
        }

        public bool Remove(string key)
        {
            int index = ComputeHash(key);
            int attempt = 0;
            while (attempt < size)
            {
                int probeIndex = Probe(index, attempt);
                if (table[probeIndex] == null)
                {
                    return false;
                }
                if (table[probeIndex].Key == key && !table[probeIndex].IsDeleted)
                {
                    table[probeIndex].IsDeleted = true;
                    count--;
                    return true;
                }
                attempt++;
            }
            return false;
        }

        public bool IsFull()
        {
            return (double)count / size >= loadFactorThreshold;
        }

        public void Print()
        {
            if (count == 0)
            {
                Console.WriteLine("Хеш-таблица пуста");
                return;
            }
            for (int i = 0; i < size; i++)
            {
                if (table[i] == null)
                {
                    Console.WriteLine($"Index {i}: пусто");
                }
                else
                {
                    Console.WriteLine($"Index {i}: {table[i]}");
                }
            }
            Console.WriteLine($"Количество активных элементов: {count}");
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                if (table[i] != null && !table[i].IsDeleted)
                {
                    yield return new KeyValuePair<string, T>(table[i].Key, (T)table[i].Value.Clone());
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}