using WordSearcher;

internal class Program
{
    private static void search(int[,] mat, int n, int x)
    {
        // set indexes for top right
        // element
        int i = 0, j = n - 1;

        while (i < n && j >= 0)
        {
            if (mat[i, j] == x)
            {
                Console.WriteLine(mat[i, j]);
                Console.Write("Element found at " + i + ", "
                              + j);
                return;
            }

            if (mat[i, j] > x)
                j--;
            else // if mat[i][j] < x
                i++;
        }

        Console.Write("n Element not found");
        return; // if ( i==n || j== -1 )
    }
    private static void Main(string[] args)
    {
        int[,] mat = { { 10, 20, 30, 40 },
                        { 15, 25, 35, 45 },
                        { 27, 29, 37, 48 },
                        { 32, 33, 39, 50 } };

        // Function call
        search(mat, 4, 29);
    }

}