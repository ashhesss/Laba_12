using System;
using System.Collections; 
using System.Collections.Generic;
using MusicInstrumentLibr;

namespace Laba_12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Задание №1");

            // 1. Сформировать двунаправленный список с объектами MusicInstrument
            DoublyLinkedList<MusicInstrument> instrumentList = new DoublyLinkedList<MusicInstrument>();
            Random rnd = new Random();
            int initialSize = 5; // Начальный размер списка
            Console.WriteLine($"\n1.Создание списка из {initialSize} случайных музыкальных инструментов:");
            for (int i = 0; i < initialSize; i++)
            {
                instrumentList.AddLast(InstrumentRequests.CreateRandomInstrument());
            }

            // 2. Распечатать полученный список
            instrumentList.PrintList("\n2.Исходный список:");

            // 3. Выполнить обработку списка: добавить элементы с номерами 1, 3, 5...
            //    (т.е. вставить на позиции 0, 2, 4... в 0-based индексации)
            Console.WriteLine("\n3.Добавление элементов на нечетные позиции (1, 3, 5...):");
            // Определим, сколько элементов добавить. Например, добавим еще 3 элемента.
            int elementsToAdd = 3;
            for (int i = 0; i < elementsToAdd; i++)
            {
                int insertIndex = i * 2; // Позиции 0, 2, 4...
                if (insertIndex <= instrumentList.Count) // Проверяем, чтобы индекс был допустим
                {
                    MusicInstrument newInstrument = InstrumentRequests.CreateRandomInstrument();
                    Console.WriteLine($"Добавляем '{newInstrument.Name}' на позицию {insertIndex + 1} (индекс {insertIndex})");
                    try
                    {
                        instrumentList.AddAt(newInstrument, insertIndex);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Ошибка добавления: {ex.Message}");
                        // Прерываем добавление, если индекс стал некорректным
                        break;
                    }

                }
                else
                {
                    Console.WriteLine($"Позиция {insertIndex + 1} выходит за пределы текущего размера списка ({instrumentList.Count}). Добавление остановлено.");
                    break; // Прекращаем добавление, если вышли за границы
                }
            }


            // 4. Распечатать полученный список
            instrumentList.PrintList("\n4.Список после добавления элементов:");

            Console.WriteLine("\n5.Удаление элементов начиная с заданного имени и до конца:");

            // Определим имя для поиска (возьмем имя из середины списка, если он не пуст)
            string nameToRemoveFrom = null;
            if (instrumentList.Count > 2)
            {
                // Попробуем взять имя элемента с индексом 2 (третий элемент)
                int targetIndex = Math.Min(2, instrumentList.Count - 1); // Берем индекс 2 или последний, если список короче
                Point<MusicInstrument> tempNode = instrumentList.FindNode(instr => true); // Находим первый узел
                for (int i = 0; i < targetIndex && tempNode?.Next != null; i++)
                {
                    tempNode = tempNode.Next;
                }
                if (tempNode?.Data != null)
                {
                    nameToRemoveFrom = (tempNode.Data as MusicInstrument)?.Name;
                }
            }

            if (string.IsNullOrEmpty(nameToRemoveFrom))
            {
                nameToRemoveFrom = "Инструмент_50"; // Имя по умолчанию, если не удалось взять из списка
                Console.WriteLine($"Не удалось взять имя из списка, используем имя по умолчанию: '{nameToRemoveFrom}'");
            }
            else
            {
                Console.WriteLine($"Ищем элемент с именем: '{nameToRemoveFrom}'");
            }


            // Ищем узел с заданным именем
            Point<MusicInstrument> startNodeToRemove = instrumentList.FindNode(
                instr => (instr as MusicInstrument)?.Name.Equals(nameToRemoveFrom, StringComparison.OrdinalIgnoreCase) ?? false
            );

            if (startNodeToRemove != null)
            {
                Console.WriteLine($"Найден узел: {startNodeToRemove.Data}. Удаляем все последующие элементы.");
                bool removed = instrumentList.RemoveAfter(startNodeToRemove);
                if (removed)
                {
                    Console.WriteLine("Удаление выполнено.");
                }
                else
                {
                    Console.WriteLine("Удаление не потребовалось (найденный узел был последним).");
                }
            }
            else
            {
                Console.WriteLine($"Элемент с именем '{nameToRemoveFrom}' не найден. Список не изменен.");
            }

            // Распечатать список после удаления
            instrumentList.PrintList("\n6.Список после попытки удаления:");

            Console.WriteLine("\nПроверка с помощью foreach:");
            foreach (var instrument in instrumentList)
            {
                Console.WriteLine($"     {instrument}");
            }


            // 5. Выполнить глубокое клонирование списка
            Console.WriteLine("\n7.Выполнение глубокого клонирования списка...");
            DoublyLinkedList<MusicInstrument> clonedList = instrumentList.DeepClone();
            Console.WriteLine("Клонирование завершено.");

            // Проверка клона (распечатка)
            clonedList.PrintList("\nСодержимое клонированного списка:");

            // Дополнительная проверка: изменим элемент в оригинале, клон не должен измениться
            if (!instrumentList.IsEmpty)
            {
                Console.WriteLine("\nПроверка независимости клона:");
                // Получим первый элемент оригинала (если есть)
                MusicInstrument originalFirst = null;
               /* Point<MusicInstrument> current = instrumentList.GetEnumerator().MoveNext() ? (Point<MusicInstrument>)instrumentList.GetEnumerator().Current : null;*/ // Это неверно, GetEnumerator возвращает T
                                                                                                                                                                    // Правильный способ получить первый элемент, если нужен сам узел:
                                                                                                                                                                    // Point<T> firstNode = head; // Если бы мы были внутри класса списка
                                                                                                                                                                    // Вне класса:
                IEnumerator<MusicInstrument> enumerator = instrumentList.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    originalFirst = enumerator.Current;
                    Console.WriteLine($"Исходное имя первого элемента оригинала: {originalFirst.Name}");
                    originalFirst.Name = "ИЗМЕНЕНО_В_ОРИГИНАЛЕ";
                    Console.WriteLine($"Измененное имя первого элемента оригинала: {originalFirst.Name}");

                    // Проверяем первый элемент клона
                    IEnumerator<MusicInstrument> clonedEnumerator = clonedList.GetEnumerator();
                    if (clonedEnumerator.MoveNext())
                    {
                        MusicInstrument clonedFirst = clonedEnumerator.Current;
                        Console.WriteLine($"Имя первого элемента клона (должно остаться прежним): {clonedFirst.Name}");
                        if (clonedFirst.Name != "ИЗМЕНЕНО_В_ОРИГИНАЛЕ")
                        {
                            Console.WriteLine("Клон не изменился. Глубокое копирование работает корректно.");
                        }
                        else
                        {
                            Console.WriteLine("ОШИБКА! Клон изменился. Копирование неглубокое!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Оригинальный список пуст, проверка независимости невозможна.");
                }
            }


            // 6. Удалить список из памяти
            Console.WriteLine("\n8. Очистка исходного списка...");
            instrumentList.Clear(); // Удаляем все элементы
            instrumentList = null; // Убираем ссылку на объект списка
            Console.WriteLine("Исходный список очищен и ссылка удалена (готов к сборке мусора).");

            // Проверка клонированного списка (он должен остаться)
            clonedList.PrintList("\nКлонированный список все еще существует:");

            Console.ReadKey(); // Ожидание нажатия клавиши перед закрытием
        }
    }
}