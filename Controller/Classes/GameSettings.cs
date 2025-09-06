
            // if joystick is moving, move the cursor
namespace Blizzard_Controller;
public class GameSettings
{
    public class ProcessNames
    {
#if WINDOWS
        public static readonly string SC2ProcName = "SC2_x64";
        public static readonly string WC3ProcName = "Warcraft III";
        public static readonly string SC1ProcName = "StarCraft";
        public static readonly string WC1ProcName = "Warcraft";
        public static readonly string WC2ProcName = "Warcraft II";
#elif LINUX
        public static readonly string SC2ProcName = "SC2";
        public static readonly string WC3ProcName = "Warcraft III";
        public static readonly string SC1ProcName = "StarCraft";
        public static readonly string WC1ProcName = "Warcraft";
        public static readonly string WC2ProcName = "Warcraft II";
#elif MACOS
        public static readonly string SC2ProcName = "SC2";
        public static readonly string WC3ProcName = "Warcraft III";
        public static readonly string SC1ProcName = "StarCraft";
        public static readonly string WC1ProcName = "unknown";
        public static readonly string WC2ProcName = "unknown";
#endif
    }
    public class StarCraft1
    {
        public static readonly int overlayWidth = 280;
        public static readonly int overlayHeight = 255;
        public static readonly int cellColumns = 4;
        public static readonly int sideOffset = 5;
        public static readonly int bottomOffset = 10;
    }
    public class StarCraft2
    {
        public static readonly int overlayWidth = 320;
        public static readonly int overlayHeight = 220;
        public static readonly int cellColumns = 6;
        public static readonly int sideOffset = 0;
        public static readonly int bottomOffset = 10;
    }
    public class WarCraft3
    {
        public static readonly int overlayWidth = 280;
        public static readonly int overlayHeight = 225;
        public static readonly int cellColumns = 5;
        public static readonly int sideOffset = 10; // changes per aspect ratio
        public static readonly int bottomOffset = 10;
    }
    public class WarCraft2
    {
        public static readonly int overlayWidth = 400;
        public static readonly int overlayHeight = 315;
        public static readonly int cellColumns = 4;
        public static readonly int sideOffset = 0;
        public static readonly int bottomOffset = 0;
    }
    public class WarCraft1
    {
        public static readonly int overlayWidth = 420;
        public static readonly int overlayHeight = 335;
        public static readonly int cellColumns = 4;
        public static readonly int sideOffset = 0;
        public static readonly int bottomOffset = 0;
    }

    public static bool writeToCSettingsFile(string stringToWrite)
    {
        // To-Do: make this cross platform!
#if WINDOWS
        string cloudPath = Path.Combine(Environment.GetEnvironmentVariable("HOMEDRIVE"), Environment.GetEnvironmentVariable("HOMEPATH"), "\\AppData\\Roaming\\Blizzard\\StarCraft\\Cloud");
        string localFile = Path.Combine(Environment.GetEnvironmentVariable("HOMEDRIVE"), Environment.GetEnvironmentVariable("HOMEPATH"), "\\Documents\\StarCraft\\CSettings.json");
        List<string> cloudFiles = new();
        if (!File.Exists(localFile))
        {
            localFile = Path.Combine(Environment.GetEnvironmentVariable("HOMEDRIVE"), Environment.GetEnvironmentVariable("HOMEPATH"), "\\OneDrive\\Documents\\StarCraft\\CSettings.json");
            if (!File.Exists(localFile))
                return false;
        }

        if (Directory.Exists(cloudPath))
        foreach (var d in Directory.GetDirectories(cloudPath))
        {
            foreach (var f in Directory.GetFiles(d))
            {
                if (Path.GetFileName(f).Equals("CSettings.json"))
                    cloudFiles.Add(f);
            }
        }
#elif LINUX
        string linuxUser = Environment.UserName;

        string lutrisPrefix = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Games", "battlenet"
        );

        string cloudPath = Path.Combine( 
            lutrisPrefix,
            "drive_c", "users", linuxUser, "AppData", "Roaming", "Blizzard", "StarCraft", "Cloud"
        );

        string localFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Documents", "StarCraft", "CSettings.json");

        List<string> cloudFiles = new();
        if (!File.Exists(localFile))
        {
            return false;
        }
        
        if (Directory.Exists(cloudPath))
            foreach (var d in Directory.GetDirectories(cloudPath))
            {
                foreach (var f in Directory.GetFiles(d))
                {
                    if (Path.GetFileName(f).Equals("CSettings.json"))
                        cloudFiles.Add(f);
                }
            }
#elif MACOS
        string localFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Library/Application Support/Blizzard/Starcraft/CSettings.json");
        if (!File.Exists(localFile))
        {
            Console.WriteLine($"Failed to write to {localFile}");
            return false;
        }
#endif
        string jsonString = File.ReadAllText(localFile);
        JsonNode jsonNode = JsonNode.Parse(jsonString);
        jsonNode["Hotkeys"] = String.IsNullOrEmpty(stringToWrite) ? new JsonObject() : stringToWrite;
        jsonString = jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(localFile, jsonString);
#if WINDOWS || LINUX
        foreach (var cf in cloudFiles)
            File.WriteAllText(cf, jsonString);
#endif

        return true;
    }

    public static bool startGame(string gameCode = "")
    {
        string bNetDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\Battle.net";
        if (!Directory.Exists(bNetDir))
        {
            return false;
        }

        var p1 = new Process();
        p1.StartInfo.WorkingDirectory = bNetDir;
        p1.StartInfo.FileName = "Battle.net Launcher.exe";
        p1.StartInfo.UseShellExecute = true;
        p1.Start();

        p1.WaitForInputIdle();

        var p = new Process();
        p.StartInfo.WorkingDirectory = bNetDir;
        p.StartInfo.FileName = "Battle.net.exe";
        if (!string.IsNullOrEmpty(gameCode))
            p.StartInfo.Arguments = $"--exec=\"launch {gameCode}\"";
        p.StartInfo.UseShellExecute = true;
        p.Start();

        return true;
    }
}
