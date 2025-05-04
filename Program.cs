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
            Console.WriteLine("Лабораторная работа №12 - Задания 1 и 2 (Вариант 9)");
            DoublyLinkedList<MusicInstrument> instrumentList = null;
            DoublyLinkedList<MusicInstrument> clonedList = null;
            HashTable<MusicInstrument> hashTable = new HashTable<MusicInstrument>(5);
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("Операции с двунаправленным списком");
                Console.WriteLine("1. Создать новый пустой список");
                Console.WriteLine("2. Добавить случайные элементы в список");
                Console.WriteLine("3. Вывести список");
                Console.WriteLine("4. Добавить элементы на нечётные позиции (1, 3, 5...)");
                Console.WriteLine("5. Удалить элементы от заданного имени до конца");
                Console.WriteLine("6. Выполнить глубокое клонирование списка");
                Console.WriteLine("7. Вывести клонированный список");
                Console.WriteLine("8. Очистить список");
                Console.WriteLine("Операции с хеш-таблицей");
                Console.WriteLine("9. Добавить элемент в хеш-таблицу");
                Console.WriteLine("10. Вывести хеш-таблицу");
                Console.WriteLine("11. Найти элемент по ключу");
                Console.WriteLine("12. Удалить элемент по ключу");
                Console.WriteLine("13. Добавить элемент в заполненную таблицу");
                Console.WriteLine("14. Перебор таблицы с помощью foreach");
                Console.WriteLine("15. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            instrumentList = new DoublyLinkedList<MusicInstrument>();
                            clonedList = null;
                            Console.WriteLine("Создан новый пустой двунаправленный список.");
                            break;

                        case "2":
                            if (instrumentList == null)
                            {
                                Console.WriteLine("! Сначала создайте список (пункт 1).");
                                break;
                            }
                            Console.Write("Сколько случайных элементов добавить? ");
                            if (int.TryParse(Console.ReadLine(), out int countToAdd) && countToAdd > 0)
                            {
                                for (int i = 0; i < countToAdd; i++)
                                {
                                    var randomInstrument = InstrumentRequests.CreateRandomInstrument();
                                    if (randomInstrument != null)
                                    {
                                        instrumentList.AddLast(randomInstrument);
                                    }
                                }
                                Console.WriteLine("Элементы добавлены.");
                                instrumentList.PrintList();
                            }
                            else
                            {
                                Console.WriteLine("! Некорректное количество.");
                            }
                            break;

                        case "3":
                            if (instrumentList == null)
                            {
                                Console.WriteLine("! Сначала создайте список (пункт 1).");
                                break;
                            }
                            instrumentList.PrintList();
                            break;

                        case "4":
                            if (instrumentList == null)
                            {
                                Console.WriteLine("! Сначала создайте список (пункт 1).");
                                break;
                            }
                            Console.Write("Сколько элементов добавить на нечётные позиции? ");
                            if (int.TryParse(Console.ReadLine(), out int oddCount) && oddCount > 0)
                            {
                                instrumentList.AddOddPositions(oddCount);
                                instrumentList.PrintList();
                            }
                            else
                            {
                                Console.WriteLine("! Некорректное количество.");
                            }
                            break;

                        case "5":
                            if (instrumentList == null)
                            {
                                Console.WriteLine("! Сначала создайте список (пункт 1).");
                                break;
                            }
                            Console.Write("Введите имя инструмента для удаления: ");
                            string nameToRemove = Console.ReadLine()?.Trim();
                            if (string.IsNullOrEmpty(nameToRemove))
                            {
                                Console.WriteLine("! Имя не может быть пустым.");
                                break;
                            }
                            instrumentList.RemoveFromNameToEnd(nameToRemove);
                            instrumentList.PrintList();
                            break;

                        case "6":
                            if (instrumentList == null)
                            {
                                Console.WriteLine("! Сначала создайте список (пункт 1).");
                                break;
                            }
                            Console.WriteLine("Глубокое клонирование списка...");
                            clonedList = instrumentList.DeepClone();
                            Console.WriteLine("Клонирование выполнено.");
                            clonedList.PrintList("Содержимое клонированного списка:");
                            break;

                        case "7":
                            if (clonedList == null)
                            {
                                Console.WriteLine("Клон ещё не создан (используйте пункт 6).");
                                break;
                            }
                            clonedList.PrintList("Содержимое клонированного списка:");
                            break;

                        case "8":
                            if (instrumentList == null)
                            {
                                Console.WriteLine("! Сначала создайте список (пункт 1).");
                                break;
                            }
                            instrumentList.Clear();
                            instrumentList.PrintList();
                            break;

                        case "9":
                            var instrument = InstrumentRequests.CreateInstrumentFromInput();
                            if (instrument != null)
                            {
                                if (hashTable.Add(instrument.Name, instrument))
                                {
                                    Console.WriteLine($"Элемент {instrument} добавлен в хеш-таблицу.");
                                }
                                else
                                {
                                    Console.WriteLine("Не удалось добавить элемент (дубликат или таблица заполнена).");
                                }
                            }
                            else
                            {
                                Console.WriteLine("! Ошибка создания инструмента.");
                            }
                            break;

                        case "10":
                            hashTable.Print();
                            break;

                        case "11":
                            Console.Write("Введите ключ (имя инструмента) для поиска: ");
                            string searchKey = Console.ReadLine()?.Trim();
                            if (string.IsNullOrEmpty(searchKey))
                            {
                                Console.WriteLine("! Ключ не может быть пустым.");
                                break;
                            }
                            var found = hashTable.Find(searchKey);
                            if (found != null)
                            {
                                Console.WriteLine($"Найден элемент: {found}");
                            }
                            else
                            {
                                Console.WriteLine("Элемент не найден.");
                            }
                            break;

                        case "12":
                            Console.Write("Введите ключ (имя инструмента) для удаления: ");
                            string removeKey = Console.ReadLine()?.Trim();
                            if (string.IsNullOrEmpty(removeKey))
                            {
                                Console.WriteLine("! Ключ не может быть пустым.");
                                break;
                            }
                            if (hashTable.Remove(removeKey))
                            {
                                Console.WriteLine($"Элемент с ключом '{removeKey}' удалён.");
                            }
                            else
                            {
                                Console.WriteLine("Элемент не найден.");
                            }
                            break;

                        case "13":
                            if (hashTable.IsFull())
                            {
                                Console.WriteLine("Таблица заполнена, попытка добавить новый элемент:");
                                var newInstrument = InstrumentRequests.CreateInstrumentFromInput();
                                if (newInstrument != null)
                                {
                                    if (!hashTable.Add(newInstrument.Name, newInstrument))
                                    {
                                        Console.WriteLine("Не удалось добавить элемент: таблица заполнена.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Элемент {newInstrument} добавлен.");
                                    }
                                    hashTable.Print();
                                }
                                else
                                {
                                    Console.WriteLine("! Ошибка создания инструмента.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Таблица ещё не заполнена. Добавьте больше элементов.");
                            }
                            break;

                        case "14":
                            Console.WriteLine("Перебор хеш-таблицы с помощью foreach:");
                            foreach (var pair in hashTable)
                            {
                                Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
                            }
                            break;

                        case "15":
                            Console.WriteLine("Выход из программы.");
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, выберите пункт из меню.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"! Ошибка: {ex.Message}");
                }
            }
        }
    }
}