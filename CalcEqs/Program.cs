namespace CalcEqs
{
    internal class Program
    {
        const int N = 4;
        static void Main(string[] args)
        {
            int[] variableNumbers = new int[N];
            double[] maxElements = new double[N];
            int changeCount = 0;
            for (int i = 0; i < N; i++)
            {
                variableNumbers[i] = i;
            }

            double[,] matrixAs = new double[N, N]
{
                { 1, 2, 3, -2 },
                { 2, -1, -2, -3 },
                { 3, 2, -1, 2 },
                { 2, -3, 2, 1 }
};

            double[,] matrixA = new double[N, N + 1]
            {
                { 1, 2, 3, -2, 1},
                { 2, -1, -2, -3, 2},
                { 3, 2, -1, 2, -5 },
                { 2, -3, 2, 1, 1}
            };

            double[,] matrixL = new double[N, N]
            {
                { 1, 2, 3, -2 },
                { 2, -1, -2, -3 },
                { 3, 2, -1, 2, },
                { 2, -3, 2, 1 }
            };
            Console.WriteLine("Matrix A: ");
            ShowVariablesVector(variableNumbers);
            ShowMatrix(matrixA);
            (int, int) maxElementIndex;
            double[,] matrixP;
            double[,] matrixY = new double[N, N]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };
            double[,] matrixY2 = new double[N, N]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };
            double[,] matrixX;
            double[,] matrixPAP;
            double[,] matrixM;
            double[,] matrixM2 = new double[N,N];

            for (int n = N; n > 0; n--)
            {
                maxElementIndex = GetMaxElementIndex(matrixA, n);
                maxElements[N - n] = matrixA[maxElementIndex.Item1, maxElementIndex.Item2];
                Console.WriteLine($"Max element: ({maxElementIndex.Item1}, {maxElementIndex.Item2}) = {matrixA[maxElementIndex.Item1, maxElementIndex.Item2]}");

                matrixP = InitMatrixP(N - n, maxElementIndex.Item1, N);
                Console.WriteLine("Matrix P: ");
                ShowMatrix(matrixP);

                if (maxElementIndex.Item1 != N - n)
                {
                    changeCount++;
                    matrixPAP = MultipleMatrix(matrixP, matrixA);
                    matrixL = MultipleMatrix(matrixP, matrixL);
                    matrixY = MultipleMatrix(matrixP, matrixY);
                }
                else
                {
                    matrixPAP = matrixA;
                }

                Console.WriteLine("Matrix PA: ");
                ShowMatrix(matrixPAP);

                matrixM = InitMatrixM(matrixL, n);
                matrixL = MultipleMatrix(matrixM, matrixL);
                matrixY = MultipleMatrix(matrixM, matrixY);

                matrixP = InitMatrixP(N - n, maxElementIndex.Item2, N + 1);
                Console.WriteLine("Matrix P: ");
                ShowMatrix(matrixP);

                if (maxElementIndex.Item2 != N - n)
                {
                    changeCount++;
                    matrixPAP = MultipleMatrix(matrixPAP, matrixP);
                }

                Console.WriteLine("Matrix PAP: ");
                ShowMatrix(matrixPAP);

                int old = variableNumbers[N - n];
                variableNumbers[N - n] = variableNumbers[maxElementIndex.Item2];
                variableNumbers[maxElementIndex.Item2] = old;

                matrixM = InitMatrixM(matrixPAP, n);
                matrixM2 = InitMatrixMinusM(matrixPAP, n);
                matrixY2 = MultipleMatrix(matrixM2, matrixY2);

                Console.WriteLine("Matrix M: ");
                ShowMatrix(matrixM);

                matrixA = MultipleMatrix(matrixM, matrixPAP);
                Console.WriteLine("Matrix A: ");
                ShowVariablesVector(variableNumbers);
                ShowMatrix(matrixA);
            }



            double[] res = GetVectorRes(matrixA);
            res = GetReorderedVector(res, variableNumbers);
            Console.WriteLine("Result:");
            ShowResultVector(res);

            double det = GetDeterminant(maxElements, changeCount);
            Console.WriteLine($"Determinant: {det}");

            matrixX = GetMatrixX(matrixL, matrixY);
            Console.WriteLine("Matrix A(-1): ");
            ShowMatrix(matrixX);

            double normA = GetInfinityNorm(matrixAs);
            double normX = GetInfinityNorm(matrixX);
            double cond = normA * normX;
            Console.WriteLine($"Norm A: {normA}");
            Console.WriteLine($"Norm A(-1): {normX}");
            Console.WriteLine($"Cond: {cond}");

            Console.Read();
        }

        public static (int, int) GetMaxElementIndex(double[,] matrix, int dimension)
        {
            (int, int) maxIndex = (N - dimension, N - dimension);
            double maxValue = Math.Abs(matrix[maxIndex.Item1, maxIndex.Item2]);
            for (int i = N - dimension; i < N; i++)
            {
                for (int j = N - dimension; j < N; j++)
                {
                    if (Math.Abs(matrix[i, j]) > maxValue)
                    {
                        maxValue = Math.Abs(matrix[i, j]);
                        maxIndex = (i, j);
                    }
                }
            }
            return maxIndex;
        }

        public static double[,] InitMatrixP(int firstRaw, int secondRaw, int dimention)
        {
            double[,] matrixRes = new double[dimention, dimention];
            for (int i = 0; i < dimention; i++)
            {
                for (int j = 0; j < dimention; j++)
                {
                    if (i == firstRaw)
                    {
                        matrixRes[i, j] = (j == secondRaw) ? 1 : 0;
                    }
                    else if (i == secondRaw)
                    {
                        matrixRes[i, j] = (j == firstRaw) ? 1 : 0;
                    }
                    else
                    {
                        matrixRes[i, j] = (i == j) ? 1 : 0;
                    }
                }
            }
            return matrixRes;
        }

        public static double[,] InitMatrixM(double[,] matrix, int dimension)
        {
            int rA = matrix.GetLength(0);
            double[,] matrixRes = new double[rA, rA];
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < rA; j++)
                {
                    if (j == N - dimension)
                    {
                        if (i >= j)
                        {
                            matrixRes[i, j] = (i == j) ? (1 / matrix[j, j]) : -(matrix[i, j] / matrix[j, j]);
                        }
                        else
                        {
                            matrixRes[i, j] = 0;
                        }
                    }
                    else
                    {
                        matrixRes[i, j] = (i == j) ? 1 : 0;
                    }
                }
            }
            return matrixRes;
        }

        public static double[,] InitMatrixMinusM(double[,] matrix, int dimension)
        {
            int rA = matrix.GetLength(0);
            double[,] matrixRes = new double[rA, rA];
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < rA; j++)
                {
                    if (j == N - dimension)
                    {
                        if (i >= j)
                        {
                            matrixRes[i, j] = (i == j) ? (matrix[j, j]) : (matrix[i, j] / matrix[j, j]);
                        }
                        else
                        {
                            matrixRes[i, j] = 0;
                        }
                    }
                    else
                    {
                        matrixRes[i, j] = (i == j) ? 1 : 0;
                    }
                }
            }
            return matrixRes;
        }

        public static double[,] MultipleMatrix(double[,] matrixA, double[,] matrixB)
        {
            int rA = matrixA.GetLength(0);
            int cA = matrixA.GetLength(1);
            int rB = matrixB.GetLength(0);
            int cB = matrixB.GetLength(1);
            double[,] matrixRes = new double[rA, cB];
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    matrixRes[i, j] = 0;
                    for (int k = 0; k < cA; k++)
                    {
                        matrixRes[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }
            return matrixRes;
        }
        public static void ShowMatrix(double[,] matrix)
        {
            int rA = matrix.GetLength(0);
            int cA = matrix.GetLength(1);
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cA; j++)
                {
                    Console.Write($"{matrix[i, j],10:F3}");
                }
                Console.WriteLine();
            }
        }

        public static void ShowResultVector(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine($"X{i + 1} = {vector[i]}");
            }
        }

        public static void ShowVariablesVector(int[] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write($"        X{matrix[i] + 1}");
            }
            Console.WriteLine();
        }

        public static double[] GetVectorRes(double[,] matrix)
        {
            double[] vectorRes = new double[N];
            for (int i = N - 1; i >= 0; i--)
            {
                vectorRes[i] = matrix[i, N];
                for (int j = N - 1; j > i; j--)
                {
                    vectorRes[i] -= vectorRes[j] * matrix[i, j];
                }
            }
            return vectorRes;
        }

        public static double[] GetReorderedVector(double[] vector, int[] sortOrder)
        {
            double[] vectorRes = new double[vector.Length];
            for (int i = 0; i < vectorRes.Length; i++)
            {
                vectorRes[sortOrder[i]] = vector[i];
            }
            return vectorRes;
        }

        public static double GetDeterminant(double[] vector, int changeCount)
        {
            double res = changeCount % 2 == 0 ? 1 : -1;
            for (int i = 0; i < vector.Length; i++)
            {
                res *= vector[i];
            }
            return res;
        }

        public static double[,] GetMatrixX(double[,] matrixL, double[,] matrixY)
        {
            double[,] matrixRes = new double[N, N];

            for (int k = 0; k < N; k++)
            {
                for (int i = N - 1; i >= 0; i--)
                {
                    matrixRes[i, k] = matrixY[i, k];
                    for (int j = N - 1; j > i; j--)
                    {
                        matrixRes[i, k] -= matrixL[i, j] * matrixRes[j, k];
                    }
                }
            }
            return matrixRes;
        }

        public static double GetInfinityNorm(double[,] matrix)
        {
            double res = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                double sum = 0;
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    sum += Math.Abs(matrix[i, j]);
                }
                if (res < sum)
                {
                    res = sum;
                }
            }
            return res;
        }
    }
}