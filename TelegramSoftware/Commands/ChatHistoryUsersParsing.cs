using TelegramSoftware.Models;
using TL;

namespace TelegramSoftware.Commands
{
    internal class ChatHistoryUsersParsing : BaseModel
    {
        public async Task ParseUsers()
        {
            Logger logger = new();
            Randomizer randomizer = new Randomizer();
            try
            {
                logger.LogInfo("Выберите ваш профиль из авторизованных или введите новый что бы авторизовать новый аккаунт");
                Console.Write("=======>");

                string[] phonesSplit = GetAllProfiles();

                string profileChange = ReadLineMessageInfo("Введите номер профиля");
                Console.Write("=======>");
                int profileChangeInt = Convert.ToInt32(profileChange);

                string Config(string what)
                {
                    if (what == "api_id") return $"{API_Settings.Current.api_id}";
                    if (what == "api_hash") return $"{API_Settings.Current.api_hash}";
                    if (what == "phone_number") return $"{phonesSplit[profileChangeInt]}";
                    if (what == "verification_code") return null; // let WTelegramClient ask the user with a console prompt 
                    if (what == "session_pathname") return $"{phonesSplit[profileChangeInt]}.session";
                    return null;
                }

                using var client = new WTelegram.Client(Config);

                var user = await client.LoginUserIfNeeded();

                string delay = ReadLineMessageInfo("Введите задержку перед парсингом каждых 100 сообщений в секундах. (рекомендовано 1 минута)");
                int delayToInt = Convert.ToInt32(delay);
                logger.LogSuccess($"We are logged-in as {user.username ?? user.first_name + " " + user.last_name} (id {user.id})");

                long parseChat = Convert.ToInt64(ReadLineMessageInfo("Введите ID чата для парсинга пользователей (ID не юзернейм)"));

                string chatName = ReadLineMessageInfo("Введите Название файла в котором сохранять пользователей");
                var chats = await client.Messages_GetAllChats();
                InputPeer peer = chats.chats[parseChat];
                int allUsersAdded = 0;
                logger.LogSuccess($"{parseChat} - начинаю парсинг\nФАЙЛ БУДЕТ СОХРАНЕН В ПАПКЕ ");
                string fileWrite = Path.Combine("Parse", $"{chatName}.txt");
                if (!File.Exists($"{fileWrite}"))
                {
                    File.AppendAllText(fileWrite, $"\n");
                }
                for (int offset_id = 0; ;)
                {
                    var messages = await client.Messages_GetHistory(peer, offset_id);
                    if (messages.Messages.Length == 0) break;
                    bool contact = true;
                    long userscount = 0;

                    foreach (var msgBase in messages.Messages)
                    {
                        var from = messages.UserOrChat(msgBase.From ?? msgBase.Peer); // from can be User/Chat/Channel

                        if (msgBase is Message msg)
                        {
                            if (from.MainUsername != null)
                            {
                                contact = true;
                                string[] arrUsers = File.ReadAllLines(fileWrite);
                                foreach (var u in arrUsers)
                                {
                                    if (contact == false)
                                    {
                                        break;
                                    }
                                    if (u == from.MainUsername)
                                    {
                                        contact = false;
                                    }
                                }
                                if (contact == true)
                                {
                                    File.AppendAllText(fileWrite, $"\n{from.MainUsername}");
                                    userscount++;
                                    allUsersAdded++;
                                }

                            }

                        }

                    }
                    logger.LogSuccess($"Добавлено новых пользователей:   {userscount}");
                    userscount = 0;
                    int rndDelay = randomizer.RandimizeDelay(delayToInt) * 1000;
                    Thread.Sleep(rndDelay);
                    offset_id = messages.Messages[^1].ID;
                }
                logger.LogSuccess($"\nДобавлено за всё время работы. {allUsersAdded}\n");
                logger.LogSuccess("Закончил работу");
            }
            catch (Exception ex) { logger.LogError(ex.Message); }
        }
    }
}
