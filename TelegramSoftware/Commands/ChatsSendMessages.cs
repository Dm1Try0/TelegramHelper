using TelegramSoftware.Models;
using TL;

namespace TelegramSoftware.Commands
{
    internal class ChatsSendMessages : BaseModel
    {
        Logger logger = new();
        Randomizer randomizer = new Randomizer();
        public async Task ChatsMessages()
        {
            string chatPath = Path.Combine("ChatsSendMessages", $"Chats.txt");
            string mesPath = Path.Combine("ChatsSendMessages", $"MessageToChat.txt");
            string[] chatsCheck = File.ReadAllLines(chatPath);
            if (chatsCheck.Length < 1)
            {
                logger.LogError($"Нет пользователей для отправки. Исправьте и перезапустите программу.{chatPath}");
                logger.LogInfo("Нажмите любую кнопку для завершения.");
                Console.ReadKey();
                return;

            }
            logger.LogSuccess("Файл с пользователями получен.");

            string MessageToSend = File.ReadAllText(mesPath);
            if (MessageToSend.Length < 5)
            {
                logger.LogError("Сообщение должно быть не менее 5 символов. Исправьте и перезапустите программу.");
                logger.LogInfo("Нажмите любую кнопку для завершения.");
                Console.ReadKey();
                return;
            }
            logger.LogSuccess("Файл с Сообщением загружен.\n");

            string delay = ReadLineMessageInfo("Введите задержку в секундах перед отправкой в каждый чат.");

            string resendCount = ReadLineMessageInfo("Сколько раз отправлять в каждый чат?");

            string timeoutResend = ReadLineMessageInfo("Задержка перед циклами отправки в чаты (в секундах) 3000 секунд(45 минут)");

            int timeoutResendtoInt = Convert.ToInt32(timeoutResend);
            int resendCountToInt = Convert.ToInt32(resendCount);
            int delayToInt = Convert.ToInt32(delay);

            string[] phonesSplit = GetAllProfiles();

            Console.WriteLine("\n");
            string whoisProfile = ReadLineMessageInfo("Какой профиль будет рассылать?");
            int whoisInt = Convert.ToInt32(whoisProfile);
            try
            {
                string Config(string what)
                {
                    if (what == "api_id") return $"{API_Settings.Current.api_id}";
                    if (what == "api_hash") return $"{API_Settings.Current.api_hash}";
                    if (what == "phone_number") return $"{phonesSplit[whoisInt]}";
                    if (what == "verification_code") return null; // let WTelegramClient ask the user with a console prompt 
                    if (what == "session_pathname") return $"{phonesSplit[whoisInt]}.session";
                    return null;
                }

                using var client = new WTelegram.Client(Config);

                var user = await client.LoginUserIfNeeded();

                logger.LogSuccess($"Аккаунт {user.username ?? user.first_name + " " + user.last_name} (id {user.id})");
                logger.LogSuccess("\nЗапуск рассылки");

                string[] arrChats = File.ReadAllLines(chatPath);

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
                    if (MessageToChar[a] == 'o') { MessageToChar[a] = randomizer.RandCharMethod('o', 'о'); }
                    if (MessageToChar[a] == 'p') { MessageToChar[a] = randomizer.RandCharMethod('p', 'р'); }
                    if (MessageToChar[a] == 'c') { MessageToChar[a] = randomizer.RandCharMethod('c', 'с'); }
                    if (MessageToChar[a] == 'x') { MessageToChar[a] = randomizer.RandCharMethod('x', 'х'); }

                }
                RandomizeMessage = new string(MessageToChar);
                var messageToSendUser = RandomizeMessage;
                var chats = await client.Messages_GetAllChats();
                int timeMethodDelay = timeoutResendtoInt * 1000; //задержка пере циклами отправки

                var entities = client.HtmlToEntities(ref messageToSendUser);

                for (int c = 0; resendCountToInt > c; c++) // отправка в чаты снова
                {
                    for (int i = 0; i < arrChats.Length; i++)
                    {
                        try
                        {

                            var resolved = await client.Contacts_ResolveUsername($"{arrChats[i]}"); // without the @

                            if (resolved.Chat is Channel channel)
                                await client.Channels_JoinChannel(channel);
                            Thread.Sleep(30);
                            logger.LogSuccess("Вступил в чат.");

                            var sent = await client.SendMessageAsync(resolved, messageToSendUser, entities: entities);

                            logger.LogSuccess($"{resolved.Chat.MainUsername} - отправлено");
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
                    logger.LogSuccess($"Профиль отработал -> {user.last_name} {user.username} {user.phone} ");
                    Thread.Sleep(timeMethodDelay);
                }
            }
            catch (Exception e) { logger.LogError("Ошибка в методе отправки. Позвоните по номеру 102 что бы сообщить об ошибке."); }
        }
    }
}
