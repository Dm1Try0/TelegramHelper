using Newtonsoft.Json;
using TelegramSoftware.Commands;
using TelegramSoftware.Models;

namespace TelegramSoftware
{
    internal class Program
    {
        static async Task Main(string[] _)
        {
            Console.Title = "Telegram software prod. mainXan";
            FilesManager filesManager = new FilesManager();
            filesManager.CreateFolders();


            AddNewProfile addNewProfile = new AddNewProfile();
            ChatHistoryUsersParsing usersParsing = new ChatHistoryUsersParsing();
            ChatsSendMessages messagesSendToChat = new ChatsSendMessages();
            InviteUsersToChat inviteUsersToChat = new InviteUsersToChat();
            SendMessageToUsers sendMessageToUsers = new SendMessageToUsers();

            Logger logger = new Logger();
            logger.LogSuccess("Привет загружаю файлы для работы.");
            // • Log to file in replacement of default Console screen logging, using this static variable:
            // StreamWriter WTelegramLogs = new StreamWriter("WTelegram.log", true, Encoding.UTF8) { AutoFlush = true };
            //  WTelegram.Helpers.Log = (lvl, str) => WTelegramLogs.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{"TDIWE!"[lvl]}] {str}");

            // • Log to VS Output debugging pane in addition (+=) to the default Console screen logging:
            // WTelegram.Helpers.Log += (lvl, str) => System.Diagnostics.Debug.WriteLine(str);

            // • In ASP.NET service, you will typically send logs to an ILogger:
            // WTelegram.Helpers.Log = (lvl, str) => _logger.Log((LogLevel)lvl, str);

            // • Disable logging (⛔️𝗗𝗢𝗡'𝗧 𝗗𝗢 𝗧𝗛𝗜𝗦 as you won't be able to diagnose any upcoming problem):
            //  WTelegram.Helpers.Log = (lvl, str) => { };

            if (!File.Exists(Path.Combine("Settings", "Settings.json")))
            {
                API_Settings settings = new API_Settings()
                {
                    api_id = string.Empty,
                    api_hash = string.Empty,
                };

                File.WriteAllText(Path.Combine("Settings", "Settings.json"), JsonConvert.SerializeObject(settings, Formatting.Indented));

                logger.LogInfo($"Заполните файл {Path.Combine("Settings", "Settings.json")} и перезапустите программу");
                logger.LogInfo($"Для завершения работы программы нажмите любую клавишу...");
                Console.Write("=======>");
                Console.ReadKey();
                return;
            }

            API_Settings.Current = JsonConvert.DeserializeObject<API_Settings>(File.ReadAllText(Path.Combine("Settings", "Settings.json")));
            logger.LogSuccess("Настройки API загружены");
            while (true)
            {
                logger.LogInfo("Доступные комманды\n\n======> [0] Add Profile - добавление нового профиля\n\n======> [1] Parse Users - парсинг пользователей из истории сообщений чата\n======> [2] Invite to Chat - приглашения в ваш чат\n======> [3] Send Message to Users - отправка сообщений пользователям\n======> [4] Send message to Chats  - отправка сообщения по чатам");
                Console.Write("=======>");
                string readCommand = Console.ReadLine();

                if (readCommand == "4") { messagesSendToChat.ChatsMessages().GetAwaiter().GetResult(); }

                if (readCommand == "0") { addNewProfile.AddNewProfilesToApp().GetAwaiter().GetResult(); }

                if (readCommand == "2") { inviteUsersToChat.InviteUsersToChatPublicMethod().GetAwaiter().GetResult(); }

                if (readCommand == "1") { usersParsing.ParseUsers().GetAwaiter().GetResult(); }

                if (readCommand == "3") { sendMessageToUsers.SendMessagesToUsersPublic().GetAwaiter().GetResult(); }

                logger.LogSuccess("Программа завершила работу.");
            }
        }
    }
}
