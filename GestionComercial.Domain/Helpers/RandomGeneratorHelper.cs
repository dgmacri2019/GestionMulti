using System.Text;

namespace GestionComercial.Domain.Helpers
{
    public static class RandomGeneratorHelper
    {
        // Generate a random number between two numbers    
        public static int RandomNumber(int min, int max)
        {
            Random random = new();
            return random.Next(min, max);
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        private static string RandomString(int size, bool lowerCase = false)
        {
            StringBuilder builder = new();
            Random random = new();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString().ToUpper();
        }

        // Generate a random password of a given length (optional)  
        public static string RandomTokenLower(int stringLength, int minNumber, int maxNumber)
        {
            StringBuilder builder = new();
            builder.Append(RandomString(stringLength, true));
            builder.Append(RandomNumber(minNumber, maxNumber));
            return builder.ToString();
        }

        public static string RandomTokenUpper(int stringLength, int minNumber, int maxNumber)
        {
            StringBuilder builder = new();
            builder.Append(RandomString(stringLength, false));
            builder.Append(RandomNumber(minNumber, maxNumber));
            return builder.ToString();
        }

        // Generate a random password of a given length (optional)  
        public static string RandomToken()
        {
            StringBuilder builder = new();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }

        public static string RandomToken(bool second)
        {
            StringBuilder builder = new();
            builder.Append(RandomString(2, true));
            builder.Append("#");
            builder.Append(RandomNumber(2000, 9999));
            builder.Append(RandomString(4, false));
            builder.Append(RandomNumber(100, 9999));

            return builder.ToString();
        }

    }
}
