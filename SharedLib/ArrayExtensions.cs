using Xunit;

namespace SharedLib;

public static class ArrayExtensions
{
    public static void Multiply(this double[] arr, double factor)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] *= factor;
        }
    }
    
    public static void Divide(this double[] arr, double factor)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] /= factor;
        }
    }
    
    public static void Add(this double[] arr, double[] arr2, double factor = 1)
    {
        Assert.Equal(arr.Length, arr2.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] += arr2[i] * factor;
        }
    }
    
    public static void Subtract(this double[] arr, double[] arr2, double factor = 1)
    {
        Assert.Equal(arr.Length, arr2.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] -= arr2[i] * factor;
        }
    }
    
    public static void AddRow(this double[] arr, in MatrixMN mat, int row, double factor = 1)
    {
        Assert.Equal(arr.Length, mat.N);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] += mat[row, i] * factor;
        }
    }
    
    public static void SubtractRow(this double[] arr, in MatrixMN mat, int row, double factor = 1)
    {
        Assert.Equal(arr.Length, mat.N);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] -= mat[row, i] * factor;
        }
    }

    public static string Print<T>(this T[] arr, string splitter = ", ") where T : IFormattable
    {
        return string.Join(splitter, arr);
    }
    
    public static double PickMax(this double[] arr, out int index, int skipElement = 0)
    {
        double maxElement = double.MinValue;
        index = -1;
        if (skipElement == 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > maxElement)
                {
                    maxElement = arr[i];
                    index = i;
                }
            }

            return maxElement;
        }

        int stackIndex = 0;
        Span<int> mem = stackalloc int[skipElement + 1];
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > maxElement)
            {
                maxElement = arr[i];
                index = i;
                mem[stackIndex] = i;
                stackIndex++;
                if (stackIndex >= skipElement + 1)
                    stackIndex = 0;
            }
        }

        index = mem[stackIndex % (skipElement + 1)];
        return arr[index];
    }
}