using MusicInstrumentLibr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class BinaryTree<T> where T : MusicInstrument, IComparable, ICloneable
    {
        private BinaryTreeNode<T> ?root;
        private readonly Func<T> dataGenerator;

        public BinaryTree(Func<T> generator)
        {
            root = null;
            dataGenerator = generator;
        }

        //создание идеально сбалансированного дерева
        public void CreateIdealTree(int size)
        {
            root = CreateIdealTreeRecursive(size);
        }

        private BinaryTreeNode<T>? CreateIdealTreeRecursive(int size)
        {
            if (size <= 0) return null;

            int leftSize = size / 2;
            int rightSize = size - leftSize - 1;

            var node = new BinaryTreeNode<T>(dataGenerator()); 
            node.Left = CreateIdealTreeRecursive(leftSize);
            node.Right = CreateIdealTreeRecursive(rightSize);

            return node;
        }

        //печать дерева по уровням (слева направо)
        public void PrintTree(string treeName)
        {
            Console.WriteLine($"\n{treeName}:");
            if (root == null)
            {
                Console.WriteLine("Дерево пустое");
                return;
            }
            PrintTreeRecursive(root, 0);
        }

        private void PrintTreeRecursive(BinaryTreeNode<T> ?node, int level)
        {
            if (node == null) return;

            PrintTreeRecursive(node.Left, level + 3);
            for (int i = 0; i < level; i++) Console.Write(" ");
            Console.WriteLine(node.Data.ToString());
            PrintTreeRecursive(node.Right, level + 3);
        }

        //поиск максимального элемента (по Name, согласно IComparable)
        public T FindMaxElement()
        {
            if (root == null) return null;

            T max = root.Data;
            FindMaxElementRecursive(root, ref max);
            return (T)max.Clone();
        }

        private void FindMaxElementRecursive(BinaryTreeNode<T> ?node, ref T max)
        {
            if (node == null) return;

            if (node.Data.CompareTo(max) > 0)
                max = node.Data;

            FindMaxElementRecursive(node.Left, ref max);
            FindMaxElementRecursive(node.Right, ref max);
        }

        //преобразование в дерево поиска
        public BinaryTree<T> ConvertToSearchTree()
        {
            var searchTree = new BinaryTree<T>(dataGenerator);
            if (root == null) return searchTree;

            var elements = new List<T>();
            CollectElements(root, elements);

            foreach (var element in elements)
            {
                searchTree.InsertToSearchTree(element);
            }

            return searchTree;
        }

        private void CollectElements(BinaryTreeNode<T> ?node, List<T> elements)
        {
            if (node == null) return;
            elements.Add((T)node.Data.Clone());
            CollectElements(node.Left, elements);
            CollectElements(node.Right, elements);
        }

        private void InsertToSearchTree(T data)
        {
            root = InsertToSearchTreeRecursive(root, data);
        }

        private BinaryTreeNode<T> InsertToSearchTreeRecursive(BinaryTreeNode<T>? node, T data)
        {
            if (node == null)
                return new BinaryTreeNode<T>(data);

            int compareResult = data.CompareTo(node.Data);
            if (compareResult == 0)
                return node; //повторение, не добавляем

            if (compareResult < 0)
                node.Left = InsertToSearchTreeRecursive(node.Left, data);
            else
                node.Right = InsertToSearchTreeRecursive(node.Right, data);

            return node;
        }

        //удаление элемента из дерева поиска по ключу (Name)
        public void DeleteFromSearchTree(string key)
        {
            root = DeleteFromSearchTreeRecursive(root, key);
        }

        private BinaryTreeNode<T> DeleteFromSearchTreeRecursive(BinaryTreeNode<T>? node, string key)
        {
            if (node == null) return null;

            int compareResult = string.Compare(key, node.Data.Name, StringComparison.Ordinal);
            if (compareResult < 0)
            {
                node.Left = DeleteFromSearchTreeRecursive(node.Left, key);
            }
            else if (compareResult > 0)
            {
                node.Right = DeleteFromSearchTreeRecursive(node.Right, key);
            }
            else
            {
                //узел найден
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;

                // Узел с двумя детьми: находим минимальный элемент в правом поддереве
                var minNode = FindMin(node.Right);
                node.Data = minNode.Data;
                node.Right = DeleteFromSearchTreeRecursive(node.Right, minNode.Data.Name);
            }
            return node;
        }

        private BinaryTreeNode<T> FindMin(BinaryTreeNode<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        //jчистка дерева
        public void Clear()
        {
            ClearRecursive(root);
            root = null;
        }

        private void ClearRecursive(BinaryTreeNode<T>? node)
        {
            if (node == null) return;
            ClearRecursive(node.Left);
            ClearRecursive(node.Right);
        }
    }
}
