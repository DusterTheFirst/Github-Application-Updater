using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Github_Application_Updater.Objects;

namespace Github_Application_Updater.Config {
    public class ConfigManager {
        
        private ConfigFile Config;

        private DebugConsole Console;

        public event EventHandler<ConfigFile> AfterConfigLoaded;
        public event EventHandler<ConfigFile> BeforeConfigSaved;

        public ConfigManager(DebugConsole Console) {
            this.Console = Console;
        }

        public void Load() => Load("config.json");
        public void Load(string path) {
            LoadConfig(path);
            AfterConfigLoaded.BeginInvoke(this, Config, null, null);
        }
        private void LoadConfig(string path) {
            if (!File.Exists(path)) {
                string file = JsonConvert.SerializeObject(new ConfigFile(), Formatting.Indented);
                File.WriteAllText(path, file);
            }
            Config = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(path));
        }

        public void Save() => Save("config.json");
        public void Save(string path) {
            BeforeConfigSaved.BeginInvoke(this, Config, null, null);
            SaveConfig(path);
        }
        private void SaveConfig(string path) {
            string file = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(path, file);
        }

    }
}
