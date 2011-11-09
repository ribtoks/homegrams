using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExternalClasses
{
    /// <summary>
    /// Class, that represents matrices, entered by all experts
    /// </summary>
    [Serializable]
    public class ExpertsMatrix : ICloneable
    {
        public ExpertsMatrix()
        {
            LastId = 0;
            Matrices = new Dictionary<string, HierarchyClasses.MatrixClasses.Matrix>();
            ExpertIds = new Dictionary<string, int>();
        }

        public Dictionary<string, HierarchyClasses.MatrixClasses.Matrix> Matrices { get; set; }
        public Dictionary<string, int> ExpertIds { get; set; }
        public int LastId { get; set; }

        public object Clone()
        {
            var expertsMatrix = new ExpertsMatrix();

            expertsMatrix.Matrices = new Dictionary<string, HierarchyClasses.MatrixClasses.Matrix>();
            foreach (var keyvalue in Matrices)
            {
                expertsMatrix.Matrices.Add(keyvalue.Key, (HierarchyClasses.MatrixClasses.Matrix)keyvalue.Value.Clone());
            }

            expertsMatrix.ExpertIds = new Dictionary<string, int>();
            foreach (var keyvalue in ExpertIds)
            {
                expertsMatrix.ExpertIds.Add(keyvalue.Key, keyvalue.Value);
            }

            return expertsMatrix;
        }

        public HierarchyClasses.MatrixClasses.Matrix GenerateMatrix()
        {
            int size = -1;

            foreach (var keyvalue in Matrices)
            {
                if (size == -1)
                    size = keyvalue.Value.Width;

                if (size != keyvalue.Value.Width)
                    throw new ArgumentException("Submitted matrix has wrong dimensions");
            }

            if (size == -1)
                return new HierarchyClasses.MatrixClasses.Matrix();

            var matrix = new HierarchyClasses.MatrixClasses.Matrix(size);

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    double product = 1;
                    foreach (var keyvalue in Matrices)
                    {
                        product *= keyvalue.Value[i, j];
                    }

                    matrix[i, j] = Math.Pow(product, 1.0/Convert.ToDouble(Matrices.Count));
                }
            }

            return matrix;
        }
    }
}
