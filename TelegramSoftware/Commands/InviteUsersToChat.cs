using TelegramSoftware.Models;
using TL;

namespace TelegramSoftware.Commands
{
    internal class InviteUsersToChat : BaseModel
    {
        Logger logger = new();
        Randomizer randomizer = new Randomizer();
        object locker = new();
        public async Task InviteUsersToChatPublicMethod()
        {
            try
            {
                string pathInviteUsernames = Path.Combine("Inviting", $"Usernames.txt");
                string[] usernames = File.ReadAllLines(pathInviteUsernames);
                if (usernames.Length < 1)
                {
                    logger.LogError($"В файле нет пользователей.{pathInviteUsernames}");
                    logger.LogInfo($"Заполните файл и запустите программу заново. Нажмите любую кнопку для выхода.");
                    Console.ReadKey();
                    return;
                }

                string chooseVPN = ReadLineMessageInfo("Нужно ли останавливаться после каждого пользователя что бы вы сменили впн/прокси? \n====>   y|n");
                if (chooseVPN == "y") { logger.LogSuccess("Каждый аккаунт будет ждать вашего подтверждения о смене впн"); }
                else { logger.LogSuccess("Ожидание смены VPN|PROXY отключено."); }

                string channelToInvite = ReadLineMessageInfo("Введите username канала куда хотите добавлять пользователей (без @)");

                string delay = ReadLineMessageInfo("Введите задержку в секундах каждого добавления пользователя");
                int delayToInt = Convert.ToInt32(delay);

                string usersPerAccount = ReadLineMessageInfo("Введите сколько пользователей добавит каждый аккаунт.");
                int usersPerAccountToInt = Convert.ToInt32(usersPerAccount);

                string[] phonesSplit = GetAllProfiles();

                Console.WriteLine("\n");
                try
                {
                    foreach (var i in phonesSplit)
                    {
                        logger.LogInfo($"Запуск нового профиля. {i}");
                        lock (locker)
                        {
                            InviteToChat($"{i}", usersPerAccountToInt, chooseVPN, delayToInt, channelToInvite).GetAwaiter().GetResult();
                        }
                        int rndDelay = randomizer.RandimizeDelay(delayToInt) * 1000;
                        await Task.Delay(rndDelay);
                    }
                }
                catch (Exception e) { logger.LogError("Ошибка в методе. Позвоните по номеру 102 что бы сообщить об ошибке."); }

                return;
            }
            catch (Exception ex) { logger.LogError("Ошибка метода."); logger.LogError(ex.Message); }
        }
        private async Task InviteToChat(string phoneNumber, int usersCountPerAccount, string chooseVPN, int delay, string channelToInvite)
        {
            if (chooseVPN == "y")
            {
                logger.LogSuccess("Пауза для смены впн");
                logger.LogInfo("Нажмите любую кнопку что бы продолжить");
                Console.ReadKey();
            }
            string Config(string what)
            {
                if (what == "api_id") return $"{API_Settings.Current.api_id}";
                if (what == "api_hash") return $"{API_Settings.Current.api_hash}";
                if (what == "phone_number") return $"{phoneNumber}";
                if (what == "verification_code") return null; // let WTelegramClient ask the user with a console prompt 
                if (what == "session_pathname") return $"{phoneNumber}.session";
                return null;
            }

            using var client = new WTelegram.Client(Config);
            var user = await client.LoginUserIfNeeded();

            logger.LogSuccess($"Аккаунт {user.username ?? user.first_name + " " + user.last_name} (id {user.id})");
            logger.LogSuccess("\nЗапуск Инвайтинга");

            string pathInviteUsernames = Path.Combine("Inviting", $"Usernames.txt");
            string[] usernames = File.ReadAllLines(pathInviteUsernames);
            for (int i = 0; i < usersCountPerAccount; i++)
            {
                try
                {
                    var resolved = await client.Contacts_ResolveUsername(channelToInvite); // without the @
                    var resolvedUser = await client.Contacts_ResolveUsername(usernames[i]);
                    await client.AddChatUser(resolved, resolvedUser.User);
                    logger.LogInfo($"Добавил в чат - {resolvedUser.User.MainUsername}");
                    int rndDelay = randomizer.RandimizeDelay(delay) * 1000;
                    await Task.Delay(rndDelay);
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message); string[] exp = e.Message.Split("_");
                    foreach (var a in exp)
                    {
                        if (a == "FLOOD")
                        {
                            logger.LogError($"\nАккаунт попал в спам! {user.last_name} {user.username} {user.phone}\n");
                            logger.LogInfo("Аккаунт не будет работать в этой сессии.");
                            return;
                        }
                    }
                }

            }
            //if(все пользователи отправлены) то удалить файл usernames и перезаписать

            File.WriteAllText(pathInviteUsernames, "");
            for (int i = usersCountPerAccount++; i < usernames.Length; i++)
            {
                File.AppendAllText(pathInviteUsernames, $"\n{usernames[i]}");

            }
            logger.LogSuccess($"Профиль отработал -> {user.last_name} {user.username} {user.phone} ");
        }

    }
}
