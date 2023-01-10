using Lab5_P;
using SharedLib;


MatrixMN matrix = new MatrixMN(3, 5)
{
    [0, 0] = 0, [0, 1] = 3, [0, 2] = 1, [0, 3] = 1, [0, 4] = 1,
    [1, 0] = 4, [1, 1] = 3, [1, 2] = 0, [1, 3] = 1, [1, 4] = 0,
    [2, 0] = 3, [2, 1] = -2, [2, 2] = 0, [2, 3] = 0, [2, 4] = 1
};
var bVec = new double[] {20, 12, 6};
var cVec = new double[] {-1, -7, -2, -1, 1};

int[] basicVarIndexes = {0, 1, 3};

var simplex = new SimplexMethod(matrix, bVec, cVec, basicVarIndexes);
simplex.Solve();