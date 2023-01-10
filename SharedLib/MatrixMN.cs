using System.Collections;
using System.Text;
using Xunit;

namespace SharedLib;

public struct MatrixMN : IEnumerable<double>, IEnumerable<(int i, int j)>
{
    
    public double this[int row, int column]
    {
        get => _matrix[row, column];
        set => _matrix[row, column] = value;
    }
    
    public double this[(int i, int j) tuple]
    {
        get => _matrix[tuple.i, tuple.j];
        set => _matrix[tuple.i, tuple.j] = value;
    }

    public int M => _matrix.GetLength(0);
    public int N => _matrix.GetLength(1);
    

    private readonly double[,] _matrix;
        
    public MatrixMN(int rows, int columns)
    {
        _matrix = new double[rows, columns];
    }
    
    public MatrixMN(int dimensions)
    {
        _matrix = new double[dimensions, dimensions];
    }

    public bool Equals(MatrixMN other)
    {
        if (_matrix.Equals(other._matrix))
            return true;

        if (M != other.M || N != other.N)
            return false;

        foreach (var indexes in other)
        {
            if (!this[indexes].Equals(other[indexes]))
                return false;
        }

        return true;
    }

    public IEnumerator<(int i, int j)> GetEnumerator()
    {
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                yield return (i, j);
            }
        }
    }

    IEnumerator<double> IEnumerable<double>.GetEnumerator()
    {
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                yield return this[i, j];
            }
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is MatrixMN other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _matrix.GetHashCode();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public double[] GetRow(int m)
    {
        var arr = new double[N];
        for (int i = 0; i < N; i++)
        {
            arr[i] = this[m, i];
        }

        return arr;
    }
    
    public void SetRow(int m, double[] row)
    {
        Assert.Equal(N, row.Length);
        for (int i = 0; i < N; i++)
        { 
            this[m, i] = row[i];
        }
    }
    
    public double[] GetColumn(int n)
    {
        var arr = new double[M];
        for (int i = 0; i < M; i++)
        {
            arr[i] = this[i, n];
        }

        return arr;
    }
    
    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append($"Matrix{M}x{N}:\n");
        for (int i = 0; i < M; i++)
        {
            strBuilder.Append(' ');
            strBuilder.AppendJoin(", ", GetRow(i));
            strBuilder.Append('\n');
        }

        return strBuilder.ToString();
    }
}