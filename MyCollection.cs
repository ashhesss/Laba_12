using MusicInstrumentLibr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class MyCollection<TValue> : IDictionary<string, TValue>, ICollection<KeyValuePair<string, TValue>>, IEnumerable<KeyValuePair<string, TValue>>
        where TValue : MusicInstrument, ICloneable
    {
        private HashNode<TValue>[] table;
        private int size;
        private int count;
        private readonly double loadFactorThreshold = 0.7;

        public MyCollection()
        {
            size = 10;
            table = new HashNode<TValue>[size];
            count = 0;
        }

        public MyCollection(int capacity)
        {
            if (capacity < 0) throw new ArgumentException("Ёмкость не может быть отрицательной.", nameof(capacity));
            size = Math.Max(capacity, 10);
            table = new HashNode<TValue>[size];
            count = 0;

            for (int i = 0; i < capacity; i++)
            {
                var instrument = InstrumentRequests.CreateRandomInstrument();
                if (instrument is TValue typedInstrument)
                {
                    Add($"Item{i + 1}", typedInstrument); 
                }
            }
        }

        public MyCollection(MyCollection<TValue> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            size = other.size;
            table = new HashNode<TValue>[other.table.Length];
            count = 0;

            foreach (var pair in other)
            {
                Add(pair.Key, (TValue)pair.Value.Clone());
            }
        }

        public int GetHashForKey(string key)
        {
            int code = 0;
            if (key != null)
            {
                foreach (char c in key)
                    code += c;
            }
            return Math.Abs(code) % size;
        }

        //Реализация IDictionary<string, TValue>
        public TValue this[string key]
        {
            get
            {
                var node = FindNode(key);
                if (node == null) throw new KeyNotFoundException($"Ключ {key} не найден.");
                return (TValue)node.Value.Clone();
            }
            set
            {
                var node = FindNode(key);
                if (node != null)
                {
                    node.Value = (TValue)value.Clone();
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                List<string> keys = new List<string>();
                foreach (var node in table)
                {
                    if (node != null)
                        keys.Add(node.Key);
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                foreach (var node in table)
                {
                    if (node != null)
                        values.Add((TValue)node.Value.Clone());
                }
                return values;
            }
        }

        public void Add(string key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if ((double)(count + 1) / size > loadFactorThreshold)
            {
                Resize();
            }

            int index = GetHashForKey(key);
            int attempt = 0;
            while (attempt < size)
            {
                int probeIndex = Probe(index, attempt);
                if (table[probeIndex] == null)
                {
                    table[probeIndex] = new HashNode<TValue>(key, (TValue)value.Clone());
                    count++;
                    return;
                }
                if (table[probeIndex].Key == key)
                {
                    throw new ArgumentException($"Ключ {key} уже существует.");
                }
                attempt++;
            }
            throw new InvalidOperationException("Хеш-таблица заполнена.");
        }

        public bool Remove(string key)
        {
            if (key == null) return false;

            int index = GetHashForKey(key);
            int attempt = 0;
            while (attempt < size)
            {
                int probeIndex = Probe(index, attempt);
                if (table[probeIndex] == null)
                {
                    return false;
                }
                if (table[probeIndex].Key == key)
                {
                    List<KeyValuePair<string, TValue>> remaining = new List<KeyValuePair<string, TValue>>();
                    foreach (var node in table)
                    {
                        if (node != null && node.Key != key)
                        {
                            remaining.Add(new KeyValuePair<string, TValue>(node.Key, node.Value));
                        }
                    }

                    table = new HashNode<TValue>[size];
                    count = 0;

                    foreach (var pair in remaining)
                    {
                        Add(pair.Key, pair.Value);
                    }

                    return true;
                }
                attempt++;
            }
            return false;
        }

        public bool TryGetValue(string key, out TValue? value)
        {
            var node = FindNode(key);
            if (node != null)
            {
                value = (TValue)node.Value.Clone();
                return true;
            }
            value = default;
            return false;
        }

        //Реализация ICollection<KeyValuePair<string, TValue>>
        public int Count => count;
        public bool IsReadOnly => false;

        public void Add(KeyValuePair<string, TValue> item) => Add(item.Key, item.Value);

        public void Clear()
        {
            table = new HashNode<TValue>[size];
            count = 0;
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            var node = FindNode(item.Key);
            return node != null && EqualityComparer<TValue>.Default.Equals(node.Value, item.Value);
        }

        public bool ContainsKey(string key) => FindNode(key) != null;

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Недостаточно места в массиве.");

            int i = arrayIndex;
            foreach (var pair in this)
            {
                array[i++] = new KeyValuePair<string, TValue>(pair.Key, (TValue)pair.Value.Clone());
            }
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node != null && EqualityComparer<TValue>.Default.Equals(node.Value, item.Value))
            {
                return Remove(item.Key);
            }
            return false;
        }

        // Реализация IEnumerable<KeyValuePair<string, TValue>>
        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            foreach (var node in table)
            {
                if (node != null)
                {
                    yield return new KeyValuePair<string, TValue>(node.Key, (TValue)node.Value.Clone());
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Вспомогательные методы
        private int Probe(int index, int attempt)
        {
            return (index + attempt) % size;
        }

        private HashNode<TValue> FindNode(string key)
        {
            if (key == null) return null;
            int index = GetHashForKey(key);
            int attempt = 0;
            while (attempt < size)
            {
                int probeIndex = Probe(index, attempt);
                if (table[probeIndex] == null)
                    return null;
                if (table[probeIndex].Key == key)
                    return table[probeIndex];
                attempt++;
            }
            return null;
        }

        private void Resize()
        {
            var oldTable = table;
            size = oldTable.Length * 2;
            table = new HashNode<TValue>[size];
            count = 0;

            foreach (var node in oldTable)
            {
                if (node != null)
                {
                    Add(node.Key, node.Value);
                }
            }
        }
        public void Print(string title = "Содержимое хеш-таблицы:")
        {
            Console.WriteLine(title);
            if (count == 0)
            {
                Console.WriteLine("  (Хеш-таблица пуста)");
                return;
            }
            for (int i = 0; i < size; i++)
            {
                if (table[i] == null)
                {
                    Console.WriteLine($"  [{i}]: пусто");
                }
                else
                {
                    Console.WriteLine($"  [{i}]: Key: {table[i].Key}, Value: {table[i].Value}");
                }
            }
            Console.WriteLine($"  Всего активных элементов: {count}");
        }
    }
}
