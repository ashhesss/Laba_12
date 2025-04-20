using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_12
{
    public class Point<T> where T: ICloneable
    {
        public T Data { get; set; } // Информационное поле (обобщенное)
        public Point<T> Next { get; set; } // Ссылка на следующий узел
        public Point<T> Prev { get; set; } // Ссылка на предыдущий узел

        // Конструктор узла
        public Point(T data)
        {
            Data = data;
            Next = null;
            Prev = null;
        }

        // Переопределение ToString для удобного вывода
        public override string ToString()
        {
            return Data?.ToString() ?? "null"; // Выводим данные узла
        }

        // Метод для клонирования узла (клонирует данные глубоко)
        public Point<T> Clone()
        {
            T clonedData = (T)Data.Clone(); // Глубокое клонирование данных
            return new Point<T>(clonedData);
        }
    }
}
