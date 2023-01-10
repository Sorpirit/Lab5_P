using Lab6_P;

var initalPoint = new double[] {0, 1, 2};
var method = new NelderMeadMethod(initalPoint, 1);
Console.WriteLine("Initial simplex table:");
Console.WriteLine(method.SimplexTable);
double[] functionValues = new double[method.SimplexTable.M];
for (int row = 0; row < functionValues.Length; row++)
{
    functionValues[row] = TargetFunction(method.SimplexTable.GetRow(row));
}
Array.Sort(functionValues);
Console.WriteLine("Current function min:" + functionValues[0]);
Console.WriteLine();
method.Solve(500, 0.01, TargetFunction);
Console.WriteLine("Final simplex table:");
Console.WriteLine(method.SimplexTable);
for (int row = 0; row < functionValues.Length; row++)
{
    functionValues[row] = TargetFunction(method.SimplexTable.GetRow(row));
}
Array.Sort(functionValues);
Console.WriteLine("Final function min:" + functionValues[0]);

static double TargetFunction(double[] vector)
{
    return
        - 4 * vector[0] * vector[1] * vector[1]
        + 2 * vector[0] * vector[0] * vector[1]
        - 3 * vector[0] * vector[1] * vector[2] 
        + 7 * vector[0] * vector[0] * vector[2] * vector[2];
}