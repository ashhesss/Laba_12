using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class DoublyLinkedList<T>: IEnumerable<T> where T: ICloneable
    {
        private Point<T> head; // Начало списка
        private Point<T> tail; // Конец списка
        private int count;     // Количество элементов

        public int Count => count; // Свойство для получения количества элементов
        public bool IsEmpty => count == 0; // Проверка на пустоту

        // Конструктор по умолчанию
        public DoublyLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        // Добавление элемента в конец списка
        public void AddLast(T data)
        {
            Point<T> newNode = new Point<T>(data);
            if (IsEmpty)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
            count++;
        }

        // Добавление элемента в начало списка
        public void AddFirst(T data)
        {
            Point<T> newNode = new Point<T>(data);
            if (IsEmpty)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                newNode.Next = head;
                head.Prev = newNode;
                head = newNode;
            }
            count++;
        }

        // Добавление элемента по индексу (0-based)
        public void AddAt(T data, int index)
        {
            if (index < 0 || index > count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс выходит за пределы списка.");
            }

            if (index == 0)
            {
                AddFirst(data);
                return;
            }
            if (index == count)
            {
                AddLast(data);
                return;
            }

            Point<T> newNode = new Point<T>(data);
            Point<T> current = head;
            // Идем до элемента, *перед* которым нужно вставить
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            newNode.Next = current.Next;
            newNode.Prev = current;
            if (current.Next != null) // Проверка, если current.Next не null
            {
                current.Next.Prev = newNode;
            }
            current.Next = newNode;
            count++;
        }

        public Point<T> FindNode(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));

            Point<T> current = head;
            while (current != null)
            {
                // Проверяем условие для данных текущего узла
                // Добавим проверку current.Data на null для обобщенного случая
                if (current.Data != null && match(current.Data))
                {
                    return current; // Узел найден
                }
                current = current.Next;
            }
            return null; // Узел не найден
        }


        public bool RemoveAfter(Point<T> node)
        {
            if (node == null || node.Next == null) // Если узел не существует или он уже хвост
            {
                return false;
            }

            // Делаем указанный узел новым хвостом
            tail = node;
            tail.Next = null; // Обрываем связь со следующими элементами

            // Пересчитываем количество элементов
            int newCount = 0;
            Point<T> current = head;
            while (current != null)
            {
                newCount++;
                if (current == tail) // Дошли до нового хвоста
                {
                    break;
                }
                current = current.Next;
            }
            count = newCount;
            return true;
        }


        // Печать списка
        public void PrintList(string title = "Содержимое списка:")
        {
            Console.WriteLine(title);
            if (IsEmpty)
            {
                Console.WriteLine("  (Список пуст)");
                return;
            }
            Point<T> current = head;
            int index = 0;
            while (current != null)
            {
                Console.WriteLine($"  [{index++}]: {current.Data}"); // Используем Data
                current = current.Next;
            }
            Console.WriteLine($"  Всего элементов: {count}");
        }

        // Глубокое клонирование списка
        public DoublyLinkedList<T> DeepClone()
        {
            DoublyLinkedList<T> cloneList = new DoublyLinkedList<T>();
            Point<T> current = head;
            while (current != null)
            {
                // Клонируем данные узла и добавляем в новый список
                T clonedData = (T)current.Data.Clone();
                cloneList.AddLast(clonedData);
                current = current.Next;
            }
            return cloneList;
        }

        // Очистка списка (удаление всех элементов)
        public void Clear()
        {
            // Просто обнуляем ссылки, сборщик мусора сделает остальное
            head = null;
            tail = null;
            count = 0;
            // Можно добавить явный вызов GC.Collect(), но обычно это не рекомендуется
        }

        // Реализация интерфейса IEnumerable<T> для поддержки foreach
        public IEnumerator<T> GetEnumerator()
        {
            Point<T> current = head;
            while (current != null)
            {
                yield return current.Data; // Возвращаем данные узла
                current = current.Next;
            }
        }

        // Реализация необобщенного интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); // Просто вызываем обобщенную версию
        }
    }
}
