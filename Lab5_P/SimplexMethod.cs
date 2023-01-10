using System.Text;
using SharedLib;

namespace Lab5_P;

public class SimplexMethod
{
    private MatrixMN _matrixMn;
    private double[] _bVector;
    private double[] _cVector;
    private int[] _basicVarIndexes;
    private double _zScore;

    private int _iteration = 0;
    
    public SimplexMethod(MatrixMN matrixMn, double[] bVector, double[] cVector, int[] basicVarIndexes)
    {
        _matrixMn = matrixMn;
        _bVector = bVector;
        _cVector = cVector;
        _basicVarIndexes = basicVarIndexes;
        _zScore = 0;
        
        for (int i = 0; i < _cVector.Length; i++)
        {
            _cVector[i] *= -1;
        }
        
        Console.WriteLine("Given LP:");
        int nonBasicVariables = matrixMn.N - matrixMn.M;
        int basicVariables = matrixMn.M;
        Console.WriteLine("Basic and non basic variables are:");
        Console.WriteLine($"Basic: {basicVariables} Non basic: {nonBasicVariables}");
        Console.WriteLine("Basic initial variables: " + String.Join(", ", _basicVarIndexes));
        Console.WriteLine("Given table:");
        Console.WriteLine(this);
        MatrixExtensions.GaussianDiagonal(ref _matrixMn, _bVector, _basicVarIndexes);
        Console.WriteLine(this);
        
        NormaliseFunction();
        

        //_zScore *= -1;
        Console.WriteLine("Simplex table:");
        Console.WriteLine(this);
    }
    
    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        string strFormat = "{0,10:0.0}";
        
        for (int i = 0; i < _cVector.Length; i++)
        {
            strBuilder.AppendFormat(strFormat, $"x{i}");
        }
        
        strBuilder.AppendFormat(strFormat, "z");
        strBuilder.Append('\n');
        
        for (int i = 0; i < _cVector.Length; i++)
        {
            strBuilder.AppendFormat(strFormat, _cVector[i]);
        }

        strBuilder.AppendFormat(strFormat, _zScore);
        strBuilder.Append('\n');
        
        for (int i = 0; i < _matrixMn.M; i++)
        {
            for (int j = 0; j < _matrixMn.N; j++)
            {
                strBuilder.AppendFormat(strFormat, _matrixMn[i, j]);
            }

            strBuilder.AppendFormat(strFormat, _bVector[i]);
            strBuilder.Append('\n');
        }

        return strBuilder.ToString();
    }

    public void Solve()
    {
        while (!IsOptimal())
        {
            PickNewBasic();
            MatrixExtensions.GaussianDiagonal(ref _matrixMn, _bVector, _basicVarIndexes);
            NormaliseFunction();
            _iteration++;
            Console.WriteLine($"Iteration: {_iteration}");
            Console.WriteLine(this);
        }
    }


    public bool IsOptimal() => _cVector.All(c => c <= 0.00001);

    public bool PickNewBasic()
    {
        int skipElements = 0;
        int maxIndexCol = -1;
        do
        {
            _cVector.PickMax(out maxIndexCol, skipElements);
            int currentLeavingIndex = -1;
            double minRatio = double.MaxValue;
            for (int i = 0; i < _matrixMn.M; i++)
            {
                double val = _matrixMn[i, maxIndexCol];
                
                if(val <= 0)
                    continue;

                double ratio = _bVector[i] / val;
                if (ratio < minRatio)
                {
                    currentLeavingIndex = i;
                    minRatio = ratio;
                }
            }

            if (currentLeavingIndex != -1)
            {
                _basicVarIndexes[currentLeavingIndex] = maxIndexCol;
                return true;
            }
            
            skipElements++;
        } while (maxIndexCol != -1);
        return false;
    }
    
    private void NormaliseFunction()
    {
        for (var i = 0; i < _basicVarIndexes.Length; i++)
        {
            double factor = _cVector[_basicVarIndexes[i]];
            
            if(factor == 0)
                continue;

            _cVector.SubtractRow(in _matrixMn, i, factor);
            _zScore -= _bVector[i] * factor;
        }
    }
}