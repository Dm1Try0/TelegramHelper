using TelegramSoftware.Models;
using TL;

namespace TelegramSoftware.Commands
{
    internal class SendMessageToUsers : BaseModel
    {
        Logger logger = new();
        Randomizer randomizer = new Randomizer();
        object locker = new();
        public async Task SendMessagesToUsersPublic()
        {
            string pathSendMessagesMessage = Path.Combine("SendMassages", "Message.txt");
            string pathSendMessagesUsernames = Path.Combine("SendMassages", "Usernames.txt");

            string[] usersCheck = File.ReadAllLines(pathSendMessagesUsernames);
            string MessageToSend = File.ReadAllText(pathSendMessagesMessage);

            if (usersCheck.Length < 1)
            {
                logger.LogError($"Нет пользователей для отправки. Исправьте и перезапустите программу.{pathSendMessagesUsernames}");
                logger.LogInfo("Нажмите любую кнопку для завершения.");
                Console.ReadKey();
                return;

            }
            logger.LogSuccess("Файл с пользователями получен.");
            if (MessageToSend.Length < 5)
            {
                logger.LogError($"Сообщение не может быть пустым. Исправьте и перезапустите программу.{pathSendMessagesMessage}");
                logger.LogInfo("Нажмите любую кнопку для завершения.");
                Console.ReadKey();
                return;

            }
            logger.LogSuccess("Файл с сообщением получен.");

            string delay = ReadLineMessageInfo("Введите задержку в секундах перед отправкой в каждый чат.");
            int delayToInt = Convert.ToInt32(delay);

            string enterCountUsers = ReadLineMessageInfo("Введите сколько пользователей получат сообщения с 1 аккаунта.");
            int enterCountsUsersToInt = Convert.ToInt32(enterCountUsers);

            string chooseVPN = ReadLineMessageInfo("Нужно ли останавливаться после каждого пользователя что бы вы сменили впн/прокси? \n====>   y|n");
            if (chooseVPN == "y") { logger.LogSuccess("Каждый аккаунт будет ждать вашего подтверждения о смене впн"); }

            else { logger.LogSuccess("Ожидание смены VPN|PROXY отключено."); }

            string[] phonesSplit = GetAllProfiles();
            try
            {
                foreach (var i in phonesSplit)
                {
                    logger.LogInfo($"Запуск нового профиля.");
                    lock (locker)
                    {
                        SendMessagesAllAccounts($"{i}", enterCountsUsersToInt, chooseVPN, 0, delayToInt).GetAwaiter().GetResult();
                    }
                    int rndDelay = randomizer.RandimizeDelay(delayToInt) * 1000;
                    Thread.Sleep(rndDelay);
                }
            }
            catch (Exception e) { logger.LogError("Ошибка в методе отправки. Позвоните по номеру 102 что бы сообщить об ошибке."); }

        }
        private async Task SendMessagesAllAccounts(string phoneNumber, int usersCountPerAccount, string chooseVPN, int proxyCount, int delayToInt)
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
            string[] MTProxy = File.ReadAllLines("MTProxy.txt");
            if (MTProxy[proxyCount] != "" && MTProxy[proxyCount] != null)
            {
                client.MTProxyUrl = MTProxy[proxyCount];
            }

            var user = await client.LoginUserIfNeeded();

            logger.LogSuccess($"Аккаунт {user.username ?? user.first_name + " " + user.last_name} (id {user.id})");
            logger.LogSuccess("\nЗапуск рассылки");


            string MessageToSend = File.ReadAllText("Message.txt");
            string[] usernames = File.ReadAllLines("Usernames.txt");

            string RandomizeMessage = "";

            char[] MessageToChar = MessageToSend.ToCharArray();
            for (int a = 0; a < MessageToChar.Length; a++)
            {
                if (MessageToChar[a] == 'A') { MessageToChar[a] = randomizer.RandCharMethod('A', 'А'); }
                if (MessageToChar[a] == 'B') { MessageToChar[a] = randomizer.RandCharMethod('B', 'В'); }
                if (MessageToChar[a] == 'E') { MessageToChar[a] = randomizer.RandCharMethod('E', 'Е'); }
                if (MessageToChar[a] == 'H') { MessageToChar[a] = randomizer.RandCharMethod('H', 'Н'); }
                if (MessageToChar[a] == 'K') { MessageToChar[a] = randomizer.RandCharMethod('K', 'К'); }
                if (MessageToChar[a] == 'M') { MessageToChar[a] = randomizer.RandCharMethod('M', 'М'); }
                if (MessageToChar[a] == 'O') { MessageToChar[a] = randomizer.RandCharMethod('O', 'О'); }
                if (MessageToChar[a] == 'P') { MessageToChar[a] = randomizer.RandCharMethod('P', 'Р'); }
                if (MessageToChar[a] == 'C') { MessageToChar[a] = randomizer.RandCharMethod('C', 'С'); }
                if (MessageToChar[a] == 'T') { MessageToChar[a] = randomizer.RandCharMethod('T', 'Т'); }
                if (MessageToChar[a] == 'X') { MessageToChar[a] = randomizer.RandCharMethod('X', 'Х'); }
                // нижний регистр
                if (MessageToChar[a] == 'a') { MessageToChar[a] = randomizer.RandCharMethod('a', 'а'); }
                if (MessageToChar[a] == 'e') { MessageToChar[a] = randomizer.RandCharMethod('e', 'е'); }
                if (MessageToChar[a] == 'k') { MessageToChar[a] = randomizer.RandCharMethod('k', 'к'); }
                if (MessageToChar[a] == 'm') { MessageToChar[a] = randomizer.RandCharMethod('m', 'м'); }
                if (MessageToChar[a] == 'o') { MessageToChar[a] = randomizer.RandCharMethod('o', 'о'); }
                if (MessageToChar[a] == 'p') { MessageToChar[a] = randomizer.RandCharMethod('p', 'р'); }
                if (MessageToChar[a] == 'c') { MessageToChar[a] = randomizer.RandCharMethod('c', 'с'); }
                if (MessageToChar[a] == 'x') { MessageToChar[a] = randomizer.RandCharMethod('x', 'х'); }

            }
            RandomizeMessage = new string(MessageToChar);
            string messageToSendUser = RandomizeMessage + " @PlanCrypto_Group";

            for (int i = 1; i < usersCountPerAccount; i++)
            {
                try
                {
                    var resolved = await client.Contacts_ResolveUsername(usernames[i]); // username without the @
                    await client.SendMessageAsync(resolved, $"{messageToSendUser}");

                    Console.WriteLine($"{i} - отправлено");
                    int rndDelay = randomizer.RandimizeDelay(delayToInt) * 1000;
                    Thread.Sleep(rndDelay);
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
            //if(все пользователи отправлены) то удалить файл usernames

            File.WriteAllText("Usernames.txt", "");
            for (int i = usersCountPerAccount + 1; i < usernames.Length; i++)
            {
                File.AppendAllText("Usernames.txt", $"\n{usernames[i]}");

            }
            logger.LogSuccess($"Профиль отработал -> {user.last_name} {user.username} {user.phone} ");
        }
    }
}
