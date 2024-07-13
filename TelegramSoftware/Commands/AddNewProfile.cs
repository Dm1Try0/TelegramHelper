using TelegramSoftware.Models;

namespace TelegramSoftware.Commands
{
    internal class AddNewProfile : BaseModel
    {
        Logger logger = new();
        public async Task AddNewProfilesToApp()
        {
            string pathAccounts = Path.Combine("Settings", "Accounts.txt");
            GetAllProfiles();
            logger.LogInfo("\nУже добавленные\n");
            Console.Write("=======>");
            string profileChange = ReadLineMessageInfo("Введите новый номер что бы добавить новый профиль");


            int apiID = Convert.ToInt32(API_Settings.Current.api_id);

            if (profileChange.StartsWith("+"))
            {
                string Config(string what)
                {
                    if (what == "session_pathname") return $"{profileChange}.session";
                    if (what == "api_id") return $"{API_Settings.Current.api_id}";
                    if (what == "api_hash") return $"{API_Settings.Current.api_hash}";
                    if (what == "phone_number") return $"{profileChange}";
                    if (what == "verification_code") return null; // let WTelegramClient ask the user with a console prompt 
                                                                  // if user has enabled 2FA
                    return null;
                }

                using var client = new WTelegram.Client(Config);

                var user = await client.LoginUserIfNeeded();

                logger.LogSuccess($"We are logged-in as {user.username ?? user.first_name + " " + user.last_name} (id {user.id})");
                File.AppendAllText(pathAccounts, $"\n{profileChange}");
                logger.LogSuccess($"{profileChange} добавлен в базу");

                logger.LogInfo("Нажмите любую кнопку для завершения.");
                Console.ReadKey();

                return;
            }
        }
    }
}
