using SharedLib;
using Xunit;

namespace Lab6_P;

public class NelderMeadMethod
{
    private const double alpha = 1;
    private const double beta = 0.5;
    private const double gamma = 2.5;
    private const double delta = 0.5;
    
    private MatrixMN _simplexTable;

    public MatrixMN SimplexTable => _simplexTable;

    public NelderMeadMethod(double[] initialVector, double distanceBetweenTwoPoints)
    {
        CreateSimplexTable(initialVector, distanceBetweenTwoPoints);
    }

    public void Solve(int iterations, double precision, Func<double[], double> targetFunction)
    {
        double[] functionValues = new double[_simplexTable.M];
        int[] indexes = new int[_simplexTable.M];
        for (int i = 0; i < iterations; i++)
        {
            for (int row = 0; row < functionValues.Length; row++)
            {
                functionValues[row] = targetFunction(_simplexTable.GetRow(row));
                indexes[row] = row;
            }
            Array.Sort(functionValues, indexes);
        
            double maxFunctionValue = functionValues[^1];
            double secMaxFunctionValue = functionValues[^2];
            double minFunctionValue = functionValues[0];

            if (!double.IsFinite(maxFunctionValue) || !double.IsFinite(minFunctionValue))
            {
                return;
            }
            
            int indexOfMax = indexes[^1];
            int indexOfMin = indexes[0];
            
            double[] maxRow = _simplexTable.GetRow(indexOfMax); 
            
            double[] centroid = new double[_simplexTable.N];
            for (int row = 0; row < _simplexTable.M; row++)
            {
                if(row == indexOfMax)
                    continue;
                
                centroid.AddRow(_simplexTable, row);
            }
            centroid.Divide(_simplexTable.N);
            
            if (Math.Sqrt(functionValues
                    .Select(functionValue => Math.Pow(functionValue - targetFunction(centroid), 2))
                    .Sum() / (_simplexTable.M) ) <= precision) 
            {
                break;
            }

            double[] reflectedPoint = new double[_simplexTable.N];
            for (int row = 0; row < reflectedPoint.Length; row++)
            {
                reflectedPoint[row] = centroid[row] + alpha * (centroid[row] - maxRow[row]);
            }

            double reflectedFunctionValue = targetFunction(reflectedPoint);
            if (reflectedFunctionValue < secMaxFunctionValue &&
                reflectedFunctionValue >= minFunctionValue)
            {
                _simplexTable.SetRow(indexOfMax, reflectedPoint);
                continue;
            }
            
            if (reflectedFunctionValue < minFunctionValue)
            {
                double[] expandedPoint = new double[_simplexTable.N];
                for (int row = 0; row < expandedPoint.Length; row++)
                {
                    expandedPoint[row] = centroid[row] + gamma * (reflectedPoint[row] - centroid[row]);
                }
                _simplexTable.SetRow(indexOfMax,
                    targetFunction(expandedPoint) <= reflectedFunctionValue ? expandedPoint : reflectedPoint);
                continue;
            }
            
            double[] contractedPoint = new double[_simplexTable.N];
            if (reflectedFunctionValue >= secMaxFunctionValue)
            {
                for (int row = 0; row < contractedPoint.Length; row++)
                {
                    contractedPoint[row] = centroid[row] + beta * (reflectedPoint[row] - centroid[row]);
                }
                
                if (targetFunction(contractedPoint) <= maxFunctionValue)
                {
                    _simplexTable.SetRow(indexOfMax, contractedPoint);
                    continue;
                }
            }
            
            var minRow = _simplexTable.GetRow(indexOfMin);
            for (var j = 0; j < _simplexTable.M; j++)
            {
                if (j == indexOfMin) continue;

                var currentRow = _simplexTable.GetRow(j);
                for (int k = 0; k < currentRow.Length; k++)
                {
                    currentRow[k] = minRow[k] + delta * (currentRow[k] - minRow[k]);
                }
                _simplexTable.SetRow(j, currentRow);
            }
        }
    }

    private void CreateSimplexTable(double[] initialVector, double distanceBetweenTwoPoints)
    {
        _simplexTable = new MatrixMN(initialVector.Length + 1, initialVector.Length);
        int N = _simplexTable.N;
        for (int i = 0; i < N; i++)
        {
            _simplexTable[0, i] = initialVector[i];
        }
        
        for (int i = 1; i < _simplexTable.M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                _simplexTable[i, j] = _simplexTable[0, j] + (j == i - 1 ? D1() : D2());
            }
        }

        double D1()
        {
            return distanceBetweenTwoPoints / (N * Math.Sqrt(2)) * (Math.Sqrt(N + 1) + N - 1);
        }
        
        double D2()
        {
            return distanceBetweenTwoPoints / (N * Math.Sqrt(2)) * (Math.Sqrt(N + 1) - 1);
        }
    }
}