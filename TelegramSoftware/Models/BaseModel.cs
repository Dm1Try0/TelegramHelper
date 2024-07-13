namespace TelegramSoftware.Models
{
    internal class BaseModel
    {
        Logger logger = new();
        public string[] GetAllProfiles()
        {
            string pathAccounts = Path.Combine("Settings", "Accounts.txt");
            string[] phonesSplit = File.ReadAllLines(pathAccounts);
            int count = 0;
            foreach (var i in phonesSplit)
            {
                logger.LogInfo($"[{count}]  {i}");
                count++;
            }
            return phonesSplit;
        }
        public string ReadLineMessageInfo(string message)
        {
            logger.LogInfo(message);
            Console.Write("=======>");

            string ConsoleRead = Console.ReadLine();
            return ConsoleRead;
        }
    }
}
