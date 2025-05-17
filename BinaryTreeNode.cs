using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class BinaryTreeNode<T> where T : ICloneable
    {
        public T Data { get; set; }
        public BinaryTreeNode<T>? Left { get; set; }
        public BinaryTreeNode<T>? Right { get; set; }

        public BinaryTreeNode(T data)
        {
            Data = (T)data.Clone();
            Left = null;
            Right = null;
        }

        public BinaryTreeNode<T> DeepClone()
        {
            var clonedNode = new BinaryTreeNode<T>((T)Data.Clone())
            {
                Left = Left?.DeepClone(),
                Right = Right?.DeepClone()
            };
            return clonedNode;
        }
    }
}
