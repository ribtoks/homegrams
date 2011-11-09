using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HierarchyClasses.MatrixClasses
{
    public interface StandartMatrixInterface
    {
        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        //занулити елементи матриці
        void ZeroMe();

        //повертає окремі рвані масиви рядків
        double[][] GetRows();

        //повертає масив колонок
        double[][] GetColumns();

        //множення матриці на вектор
        double[] MulVector(double[] vector);

        //множення матриці на скаляр
        void MulDouble(double mulWhat);

        //додавання дійсного числа до матриці
        void AddDouble(double addWhat);

        //повертає слід матриці
        double Track();

        //рядковий індексатор
        double[] this[int index]
        {
            get;
            set;
        }

        //простий індексатор
        double this[int h, int w]
        {
            get;
            set;
        }
    }

    public interface IOMatrixInterface
    {
        //зчитування рядка матриці із стрічки-рівняння
        double ReadRow(string rowString, int RowIndex);
        //зчитування всіх рядків
        void ReadAllRows(string[] rows);
        //зчитування матриці з потоку
        void ReadFromStreamWithParsing(TextReader tr);
        //зчитування матриці з потоку
        void ReadFromStream(TextReader tr);
        //запис до потоку
        void WriteToStream(TextWriter tw);
    }
}
