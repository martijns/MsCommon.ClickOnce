using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace MsCommon.ClickOnce
{
    [Serializable]
    public class AppConfiguration<T> where T : class, new()
    {
        private static string configPath;

        private static Random random = new Random();

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = LoadConfiguration();
                return _instance;
            }
        }

        private static string GetConfigFilePath()
        {
            if (configPath != null)
                return configPath;

            string[] locations = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), AppVersion.AppName + ".xml"),
                Path.Combine(Path.GetTempPath(), AppVersion.AppName + ".xml")
            };
            
            // See if we already have a config file
            string path = locations.Where(l => File.Exists(l)).FirstOrDefault();
            if (path != null)
            {
                configPath = path;
                return configPath;
            }

            // See if we can use one of these paths
            path = locations.Where(l => HasWriteAccessToDir(Path.GetDirectoryName(l))).FirstOrDefault();
            if (path != null)
            {
                configPath = path;
                return configPath;
            }

            throw new ApplicationException("Cannot find a directory in which to save the configuration file. Attempted directories: " + string.Join(", ", locations));
        }

        private static bool HasWriteAccessToDir(string dirname)
        {
            if (string.IsNullOrEmpty(dirname) || !Directory.Exists(dirname))
                return false;

            try
            {
                // User probably doesn't have ACL rights on the folder, check by creating a temp file
                string randomfilename = Path.Combine(dirname, Path.GetRandomFileName());
                File.WriteAllText(randomfilename, AppVersion.AppName + " - Dummy file to check file system access");
                File.Delete(randomfilename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static T LoadConfiguration()
        {
            FileInfo file = new FileInfo(GetConfigFilePath());
            if (file.Exists)
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    using (FileStream fs = file.OpenRead())
                    {
                        object obj = ser.Deserialize(fs);
                        if (obj is T)
                            return obj as T;
                    }
                }
                catch (Exception)
                {
                    // loading failed, start anew
                }
            }
            return new T();
        }

        private static void SaveConfiguration(T config)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    FileInfo file = new FileInfo(GetConfigFilePath());
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    using (FileStream fs = file.Create())
                    {
                        ser.Serialize(fs, config);
                    }
                }
                catch (IOException)
                {
                    // Can happen when the application is grouped in the taskbar and the whole group is closed
                    Thread.Sleep(random.Next(50, 150));
                    continue;
                }
                break;
            }
        }

        public void Save()
        {
            SaveConfiguration(this as T);
        }
    }
}
