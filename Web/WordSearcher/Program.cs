internal class Program
{
    private static void Main(string[] args)
    {
        Random random = new Random();
        String characters = "abcdefghijklmnopqrstuvwxyz";

        var matrix = new int[5, 5];

        for (int i = 0; i < 5; i++)
        {
            char randomLetter;
            for (int j = 0; j < 5; j++)
            {
                matrix[i, j] = random.Next(characters.Length);
                randomLetter = characters[matrix[i, j]];
                Console.Write(randomLetter + "\t");
            }
            Console.WriteLine();
        }
    }
}