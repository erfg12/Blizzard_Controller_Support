namespace Blizzard_Controller;
public class GameSettings
{
    public class ProcessNames
    {
        public static readonly string SC2ProcName = "SC2_x64";
        public static readonly string WC3ProcName = "Warcraft III";
        public static readonly string SC1ProcName = "StarCraft";
        public static readonly string WC1ProcName = "Warcraft";
        public static readonly string WC2ProcName = "Warcraft II";
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
        public static readonly int sideOffset = 20;
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
        string cloudPath = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + "\\AppData\\Roaming\\Blizzard\\StarCraft\\Cloud";
        string localFile = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + "\\Documents\\StarCraft\\CSettings.json";
        List<string> cloudFiles = new();
        if (!File.Exists(localFile))
        {
            localFile = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + "\\OneDrive\\Documents\\StarCraft\\CSettings.json";
            if (!File.Exists(localFile))
                return false;
        }
        foreach (var d in Directory.GetDirectories(cloudPath))
        {
            foreach (var f in Directory.GetFiles(d))
            {
                if (Path.GetFileName(f).Equals("CSettings.json"))
                    cloudFiles.Add(f);
            }
        }
        string jsonString = File.ReadAllText(localFile);
        JsonNode jsonNode = JsonNode.Parse(jsonString);
        jsonNode["Hotkeys"] = String.IsNullOrEmpty(stringToWrite) ? new JsonObject() : stringToWrite;
        jsonString = jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(localFile, jsonString);
        foreach (var cf in cloudFiles)
            File.WriteAllText(cf, jsonString);

        return true;
    }
}
