using System.Numerics;

namespace CalcIterationEqs;
class Program
{
    const int N = 4;
    static void Main(string[] args)
    {
        double[,] matrixA = new double[N, N + 1]
        {
            { 5, 1, -2, 1, 3},
            { 1, 4, 1.5, 0.5, 2.5},
            { -2, 1.5, 8, -3.5, 5},
            { 1, 0.5, -3.5, 6, 6.5}
        };
        double eps = 1E-3;
        int iterationNumber = 0;
        double[] oldResultVector = new double[N] { 0, 0, 0, 0 };
        double[] resultVector = Iterate(oldResultVector, matrixA);
        double infinityNorm = GetInfinityNorm(SubtractVectors(resultVector, oldResultVector));
        iterationNumber++;
        while (infinityNorm > eps)
        {
            Console.WriteLine($"Iteration {iterationNumber}:");
            ShowResultVector(resultVector);
            Console.WriteLine();
            oldResultVector = resultVector;
            resultVector = Iterate(resultVector, matrixA);
            infinityNorm = GetInfinityNorm(SubtractVectors(resultVector, oldResultVector));
            iterationNumber++;
        }
        Console.WriteLine($"Iteration {iterationNumber}:");
        ShowResultVector(resultVector);
        Console.Read();
    }

    public static double[] Iterate(double[] vector, double[,] matrix)
    {
        double[] vectorRes = new double[vector.Length];
        for (int i = 0; i < vector.Length; i++)
        {
            if (matrix[i, i] != 0)
            {
                vectorRes[i] = matrix[i, matrix.GetLength(1) - 1] / matrix[i, i];
                for (int j = 0; j < i; j++)
                {
                    vectorRes[i] -= (vectorRes[j] * matrix[i, j]) / matrix[i, i];
                }
                for (int j = i + 1; j < vector.Length; j++)
                {
                    vectorRes[i] -= (vector[j] * matrix[i, j]) / matrix[i, i];
                }
            }
        }
        return vectorRes;
    }

    public static double GetInfinityNorm(double[] vector)
    {
        double res = Math.Abs(vector[0]);
        for (int i = 0; i < vector.Length; i++)
        {
            if (res < Math.Abs(vector[i]))
            {
                res = Math.Abs(vector[i]);
            }
        }
        return res;
    }

    public static double[] SubtractVectors(double[] vectorA, double[] vectorB)
    {
        double[] vectorRes = new double[vectorA.Length];
        if (vectorA.Length == vectorB.Length)
        {
            for (int i = 0; i < vectorA.Length; i++)
            {
                vectorRes[i] = vectorA[i] - vectorB[i];
            }
        }
        return vectorRes;
    }

    public static void ShowResultVector(double[] vector)
    {
        for (int i = 0; i < vector.Length; i++)
        {
            Console.WriteLine($"X{i + 1} = {vector[i]}");
        }
    }
}

