namespace TelegramSoftware
{
    internal class Logger
    {
        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log("[" + "ERROR" + "] " + message);
        }
        public void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Log("[" + "SUCCESS" + "] " + message);
        }
        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log("[" + "INFO" + "] " + message);

        }
        private void Log(string message)
        {
            string text = $"[{DateTime.Now}] {message}";

            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
