namespace TelegramSoftware
{
    internal class Randomizer
    {
        public char RandCharMethod(char nowchar, char changechar)
        {
            char ch = nowchar;

            if (RandomizeChar() == 1)
            {
                ch = changechar;
            }

            return ch;
        }

        public int RandomizeChar()
        {
            int rand;
            Random rnd = new Random();
            rand = rnd.Next(0, 2);
            return rand;
        }
        public int RandimizeDelay(int time)
        {
            Random rnd = new Random();

            int rand = rnd.Next(time - 20, time + 20);
            return rand;
        }
    }
}
