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
            DoublyLinkedList<MusicInstrument>? list = null;
            HashTable<MusicInstrument>? hashTable = null;
            BinaryTree<MusicInstrument>? idealTree = null;
            BinaryTree<MusicInstrument>? searchTree = null;

            while (true)
            {
                Console.WriteLine("\n=== Меню ===");
                Console.WriteLine("1. Работа с двунаправленным списком (Задание 1)");
                Console.WriteLine("2. Работа с хеш-таблицей (Задание 2)");
                Console.WriteLine("3. Работа с бинарным деревом (Задание 3)");
                Console.WriteLine("4. Работа с обобщенной коллекцией (Задание 4)");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите опцию: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleDoublyLinkedList(ref list);
                        break;

                    case "2":
                        HandleHashTable(ref hashTable);
                        break;

                    case "3":
                        HandleBinaryTree(ref idealTree, ref searchTree);
                        break;

                    case "4":
                        HandleMyCollection();
                        break;
                    case "5":
                        // Очистка памяти перед выходом
                        list?.Clear();
                        hashTable = null; // Для хеш-таблицы достаточно, так как сборщик мусора освободит память
                        idealTree?.Clear();
                        searchTree?.Clear();
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void HandleDoublyLinkedList(ref DoublyLinkedList<MusicInstrument> list)
        {
            DoublyLinkedList<MusicInstrument> instrumentList = new DoublyLinkedList<MusicInstrument>();
            DoublyLinkedList<MusicInstrument>? clonedList = null;

            while (true)
            {
                Console.WriteLine("\n=== Двунаправленный список ===");
                Console.WriteLine("1. Создать список");
                Console.WriteLine("2. Вывести список");
                Console.WriteLine("3. Добавить элементы с нечётными номерами");
                Console.WriteLine("4. Удалить элементы после заданного имени");
                Console.WriteLine("5. Сделать глубокое клонирование списка");
                Console.WriteLine("6. Удалить список из памяти");
                Console.WriteLine("7. Назад");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

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

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void HandleHashTable(ref HashTable<MusicInstrument> hashTable)
        {
            while (true)
            {
                Console.WriteLine("\n=== Хеш-таблица ===");
                Console.WriteLine("1. Создать хеш-таблицу");
                Console.WriteLine("2. Вывести хеш-таблицу");
                Console.WriteLine("3. Найти элемент по ключу");
                Console.WriteLine("4. Удалить элемент по ключу");
                Console.WriteLine("5. Повторно найти элемент");
                Console.WriteLine("6. Добавить элемент в полную таблицу");
                Console.WriteLine("7. Назад");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите размер хеш-таблицы: ");
                        if (int.TryParse(Console.ReadLine(), out int size) && size > 0)
                        {
                            hashTable = new HashTable<MusicInstrument>(size);
                            for (int i = 0; i < size; i++)
                            {
                                var instrument = InstrumentRequests.CreateRandomInstrument();
                                if (!hashTable.Add(instrument.Name, instrument))
                                {
                                    Console.WriteLine($"Не удалось добавить элемент с ключом {instrument.Name}: таблица полна или ключ уже существует.");
                                    break;
                                }
                            }
                            Console.WriteLine("Хеш-таблица создана и заполнена.");
                        }
                        else
                        {
                            Console.WriteLine("Неверный размер.");
                        }
                        break;

                    case "2":
                        if (hashTable == null)
                            Console.WriteLine("Хеш-таблица не создана.");
                        else
                            hashTable.Print();
                        break;

                    case "3":
                        if (hashTable == null)
                        {
                            Console.WriteLine("Хеш-таблица не создана.");
                            break;
                        }
                        Console.Write("Введите ключ (имя инструмента) для поиска: ");
                        string keyToFind = Console.ReadLine();
                        var found = hashTable.Find(keyToFind);
                        if (found != null)
                            Console.WriteLine($"Элемент найден: {found}");
                        else
                            Console.WriteLine("Элемент не найден.");
                        break;

                    case "4":
                        if (hashTable == null)
                        {
                            Console.WriteLine("Хеш-таблица не создана.");
                            break;
                        }
                        Console.Write("Введите ключ (имя инструмента) для удаления: ");
                        string keyToDelete = Console.ReadLine();
                        if (hashTable.Remove(keyToDelete))
                            Console.WriteLine("Элемент удалён.");
                        else
                            Console.WriteLine("Элемент не найден или уже удалён.");
                        break;

                    case "5":
                        if (hashTable == null)
                        {
                            Console.WriteLine("Хеш-таблица не создана.");
                            break;
                        }
                        Console.Write("Введите ключ (имя инструмента) для повторного поиска: ");
                        string keyToFindAgain = Console.ReadLine();
                        var foundAgain = hashTable.Find(keyToFindAgain);
                        if (foundAgain != null)
                            Console.WriteLine($"Элемент найден: {foundAgain}");
                        else
                            Console.WriteLine("Элемент не найден.");
                        break;

                    case "6":
                        if (hashTable == null)
                        {
                            Console.WriteLine("Хеш-таблица не создана.");
                            break;
                        }
                        if (!hashTable.IsFull())
                        {
                            Console.WriteLine("Хеш-таблица не полна. Сначала заполните её.");
                            break;
                        }
                        var newInstrument = InstrumentRequests.CreateRandomInstrument();
                        Console.WriteLine($"Попытка добавить элемент: {newInstrument}");
                        if (!hashTable.Add(newInstrument.Name, newInstrument))
                            Console.WriteLine("Не удалось добавить элемент: таблица полна.");
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void HandleBinaryTree(ref BinaryTree<MusicInstrument> idealTree, ref BinaryTree<MusicInstrument> searchTree)
        {
            while (true)
            {
                Console.WriteLine("\nБинарное дерево");
                Console.WriteLine("1. Создать идеально сбалансированное дерево");
                Console.WriteLine("2. Вывести деревья");
                Console.WriteLine("3. Найти максимальный элемент");
                Console.WriteLine("4. Преобразовать в дерево поиска");
                Console.WriteLine("5. Удалить элемент из дерева поиска");
                Console.WriteLine("6. Удалить деревья из памяти");
                Console.WriteLine("7. Назад");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите размер дерева: ");
                        if (int.TryParse(Console.ReadLine(), out int size) && size > 0)
                        {
                            idealTree = new BinaryTree<MusicInstrument>(() => InstrumentRequests.CreateRandomInstrument());
                            idealTree.CreateIdealTree(size);
                            Console.WriteLine("Идеально сбалансированное дерево создано.");
                            searchTree = null; // Сбрасываем дерево поиска
                        }
                        else
                        {
                            Console.WriteLine("Неверный размер.");
                        }
                        break;

                    case "2":
                        if (idealTree == null)
                            Console.WriteLine("Идеально сбалансированное дерево не создано.");
                        else
                            idealTree.PrintTree("Идеально сбалансированное дерево");

                        if (searchTree == null)
                            Console.WriteLine("Дерево поиска не создано.");
                        else
                            searchTree.PrintTree("Дерево поиска");
                        break;

                    case "3":
                        if (idealTree == null)
                        {
                            Console.WriteLine("Идеально сбалансированное дерево не создано.");
                            break;
                        }
                        var maxElement = idealTree.FindMaxElement();
                        if (maxElement != null)
                            Console.WriteLine($"Максимальный элемент: {maxElement}");
                        else
                            Console.WriteLine("Дерево пустое.");
                        break;

                    case "4":
                        if (idealTree == null)
                        {
                            Console.WriteLine("Идеально сбалансированное дерево не создано.");
                            break;
                        }
                        searchTree = idealTree.ConvertToSearchTree();
                        Console.WriteLine("Дерево преобразовано в дерево поиска.");
                        break;

                    case "5":
                        if (searchTree == null)
                        {
                            Console.WriteLine("Дерево поиска не создано. Сначала выполните преобразование.");
                            break;
                        }
                        Console.Write("Введите ключ (имя инструмента) для удаления: ");
                        string keyToDelete = Console.ReadLine();
                        searchTree.DeleteFromSearchTree(keyToDelete);
                        Console.WriteLine("Элемент удалён (если он существовал).");
                        break;

                    case "6":
                        if (idealTree == null && searchTree == null)
                        {
                            Console.WriteLine("Деревья уже пусты.");
                            break;
                        }
                        idealTree?.Clear();
                        searchTree?.Clear();
                        idealTree = null;
                        searchTree = null;
                        Console.WriteLine("Деревья удалены из памяти.");
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void HandleMyCollection()
        {
            MyCollection<MusicInstrument> collection = null;
            while (true)
            {
                Console.WriteLine("\n=== Задание 4: Обобщённая хеш-таблица ===");
                Console.WriteLine("1. Создать пустую коллекцию");
                Console.WriteLine("2. Создать коллекцию с 5 случайными элементами");
                Console.WriteLine("3. Создать копию коллекции");
                Console.WriteLine("4. Добавить новый элемент");
                Console.WriteLine("5. Проверить наличие ключа");
                Console.WriteLine("6. Получить значение по ключу");
                Console.WriteLine("7. Удалить элемент по ключу");
                Console.WriteLine("8. Копировать коллекцию в массив");
                Console.WriteLine("9. Очистить коллекцию");
                Console.WriteLine("10. Вывести коллекцию");
                Console.WriteLine("0. Вернуться в главное меню");
                Console.Write("Выберите опцию: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            collection = new MyCollection<MusicInstrument>();
                            Console.WriteLine("Пустая коллекция создана.");
                            collection.Print();
                            break;
                        case "2":
                            collection = new MyCollection<MusicInstrument>(5);
                            Console.WriteLine("Коллекция с 5 элементами создана.");
                            collection.Print();
                            break;
                        case "3":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            var copy = new MyCollection<MusicInstrument>(collection);
                            Console.WriteLine("Копия коллекции создана.");
                            copy.Print("Содержимое копии:");
                            break;
                        case "4":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            var newInstrument = InstrumentRequests.CreateRandomInstrument() as MusicInstrument;
                            collection.Add("NewItem", newInstrument);
                            Console.WriteLine($"Элемент с ключом 'NewItem' добавлен.");
                            collection.Print();
                            break;
                        case "5":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            Console.Write("Введите ключ для проверки: ");
                            string? keyToCheck = Console.ReadLine();
                            Console.WriteLine($"Ключ '{keyToCheck}' существует: {collection.ContainsKey(keyToCheck)}");
                            break;
                        case "6":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            Console.Write("Введите ключ для получения значения: ");
                            string? keyToGet = Console.ReadLine();
                            if (collection.TryGetValue(keyToGet, out var value))
                            {
                                Console.WriteLine($"Значение для ключа '{keyToGet}': {value}");
                            }
                            else
                            {
                                Console.WriteLine($"Ключ '{keyToGet}' не найден.");
                            }
                            break;
                        case "7":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            Console.Write("Введите ключ для удаления: ");
                            string? keyToRemove = Console.ReadLine();
                            bool removed = collection.Remove(keyToRemove);
                            Console.WriteLine($"Элемент с ключом '{keyToRemove}' {(removed ? "удалён" : "не найден")}.");
                            collection.Print();
                            break;
                        case "8":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            KeyValuePair<string, MusicInstrument>[] array = new KeyValuePair<string, MusicInstrument>[collection.Count];
                            collection.CopyTo(array, 0);
                            Console.WriteLine("Коллекция скопирована в массив:");
                            for (int i = 0; i < array.Length; i++)
                            {
                                Console.WriteLine($"  [{i}]: Key: {array[i].Key}, Value: {array[i].Value}");
                            }
                            break;
                        case "9":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            collection.Clear();
                            Console.WriteLine("Коллекция очищена.");
                            collection.Print();
                            break;
                        case "10":
                            if (collection == null)
                            {
                                Console.WriteLine("Сначала создайте коллекцию.");
                                break;
                            }
                            collection.Print();
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}