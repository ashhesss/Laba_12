using MusicInstrumentLibr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class DoublyLinkedList<T>: IEnumerable<T> where T: MusicInstrument, ICloneable
    {
        private Point<T> head; //Начало списка
        private Point<T> tail; //Конец списка
        private int count;     //Количество элементов

        public int Count => count; //Свойство для получения количества элементов
        public bool IsEmpty => count == 0; //Проверка на пустоту

        //Конструктор по умолчанию
        public DoublyLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        //Добавление элемента в конец списка
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

        //Добавление элемента в начало списка
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

        //Добавление элемента по индексу 
        public void AddAt(T data, int index)
        {
            if (data == null)
            {
                Console.WriteLine($"! ошибка: попытка добавить null элемент.");
                return; // не добавляем null
            }

            if (index < 0 || index > count)
            {
                // вместо выбрасывания исключения выводим сообщение
                Console.WriteLine($"! ошибка: индекс {index} выходит за пределы [0..{count}]. элемент не добавлен.");
                return; // выходим из метода
                // throw new ArgumentOutOfRangeException(nameof(index), "индекс выходит за пределы списка.");
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
            // идем до элемента, *перед* которым нужно вставить
            for (int i = 0; i < index - 1; i++)
            {
                if (current == null)
                {
                    Console.WriteLine("! ошибка: непредвиденный null при обходе списка для вставки.");
                    return; // непредвиденная ошибка
                }
                current = current.Next;
            }

            // проверка, что current не null после цикла
            if (current == null)
            {
                Console.WriteLine("! ошибка: непредвиденный null после обхода списка для вставки.");
                return;
            }

            newNode.Next = current.Next;
            newNode.Prev = current;
            // проверка, что current.next не null перед обращением к current.next.prev
            if (current.Next != null)
            {
                current.Next.Prev = newNode;
            }
            current.Next = newNode;
            count++;
            // return true; // убрали возврат bool
        }
        public void AddOddPositions(int countToAdd)
        {
            int addedCount = 0;
            for (int i = 0; addedCount < countToAdd; i++)
            {
                int insertIndex = i * 2; // Индексы: 0, 2, 4, ... (позиции 1, 3, 5, ...)
                if (insertIndex > count)
                {
                    Console.WriteLine($"Позиция {insertIndex + 1} выходит за пределы списка. Добавление остановлено.");
                    break;
                }

                var newInstrument = InstrumentRequests.CreateRandomInstrument();
                if (newInstrument == null)
                {
                    Console.WriteLine("Ошибка: не удалось сгенерировать инструмент.");
                    continue;
                }

                // Проверяем, что newInstrument соответствует T
                if (newInstrument is T typedInstrument)
                {
                    Console.WriteLine($"Добавление '{newInstrument.Name}' на позицию {insertIndex + 1} (индекс {insertIndex})");
                    AddAt(typedInstrument, insertIndex);
                    addedCount++;
                }
                else
                {
                    Console.WriteLine($"Ошибка: сгенерированный инструмент не соответствует типу T.");
                }
            }
            Console.WriteLine($"Добавлено {addedCount} элементов.");
        }

        public Point<T> FindNode(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));

            Point<T> current = head;
            while (current != null)
            {
                //проверяем условие для данных текущего узла
                //добавим проверку current.Data на null для обобщенного случая
                if (current.Data != null && match(current.Data))
                {
                    return current; //Узел найден
                }
                current = current.Next;
            }
            return null; //Узел не найден
        }


        public void RemoveFromNameToEnd(string name)
        {
            if (IsEmpty)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            Point<T> startNode = FindNode(instr => instr.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (startNode == null)
            {
                Console.WriteLine($"Элемент с именем '{name}' не найден.");
                return;
            }

            Console.WriteLine($"Удаление элементов начиная с '{name}' до конца");
            if (startNode == head)
            {
                head = null;
                tail = null;
                count = 0;
            }
            else
            {
                startNode.Prev.Next = null;
                tail = startNode.Prev;
                Point<T> current = startNode;
                while (current != null)
                {
                    Point<T> temp = current;
                    current = current.Next;
                    temp.Prev = null;
                    temp.Next = null;
                    temp.Data = default(T); // Исправлено: используем default(T)
                    count--;
                }
            }
        }

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
                Console.WriteLine($"  [{index++}]: {current.Data}"); //Используем Data
                current = current.Next;
            }
            Console.WriteLine($"  Всего элементов: {count}");
        }

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

        //Очистка списка (удаление всех элементов)
        public void Clear()
        {
            //обнуляем ссылки, сборщик мусора сделает остальное
            head = null;
            tail = null;
            count = 0;
        }

        //реализация интерфейса IEnumerable<T> для поддержки foreach
        public IEnumerator<T> GetEnumerator()
        {
            Point<T> current = head;
            while (current != null)
            {
                yield return current.Data; //возвращаем данные узла
                current = current.Next;
            }
        }

        //Реализация необобщенного интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); //Просто вызываем обобщенную версию
        }
    }
}
