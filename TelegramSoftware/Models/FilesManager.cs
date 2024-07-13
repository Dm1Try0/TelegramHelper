namespace TelegramSoftware.Models
{
    internal class FilesManager
    {

        /// <summary>
        /// Check folders and files, create if not exists
        /// </summary>
        public void CreateFolders()
        {
            //folders paths
            string baseFolderProxy = "Proxy";
            string baseFolderName = "Settings";
            string baseFolderForParse = "Parse";
            string baseFolderForInvite = "Inviting";
            string baseFolderForSendMessages = "SendMassages";
            string baseFolderForChatsSendMessages = "ChatsSendMessages";
            //file paths
            string[] ArrFiles = new string[8];
            string[] ArrFolders = new string[6];
            ArrFiles[0] = Path.Combine("Proxy", $"Socks5.txt");
            ArrFiles[1] = Path.Combine("Proxy", $"MTProxy.txt");
            ArrFiles[2] = Path.Combine("Inviting", $"Usernames.txt");
            ArrFiles[3] = Path.Combine("ChatsSendMessages", $"MessageToChat.txt");
            ArrFiles[4] = Path.Combine("ChatsSendMessages", $"Chats.txt");
            ArrFiles[5] = Path.Combine("SendMassages", "Message.txt");
            ArrFiles[6] = Path.Combine("SendMassages", "Usernames.txt");
            ArrFiles[7] = Path.Combine("Settings", "Accounts.txt");
            //folders to array
            ArrFolders[0] = baseFolderProxy;
            ArrFolders[1] = baseFolderName;
            ArrFolders[2] = baseFolderForParse;
            ArrFolders[3] = baseFolderForInvite;
            ArrFolders[4] = baseFolderForChatsSendMessages;
            ArrFolders[5] = baseFolderForSendMessages;


            foreach (string file in ArrFolders)
            {
                DirectoryExist(file);
            }
            foreach (string file in ArrFiles)
            {
                FileExist(file);
            }

        }
        /// <summary>
        /// Check exist file and create
        /// </summary>
        /// <param name="path"></param>
        private void FileExist(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }
        /// <summary>
        /// Check folder and create
        /// </summary>
        /// <param name="path"></param>
        private void DirectoryExist(string path)
        {
            if (!Directory.Exists(path))
            {
                string createdir = Directory.CreateDirectory(path).FullName;
            }
        }

    }
}
