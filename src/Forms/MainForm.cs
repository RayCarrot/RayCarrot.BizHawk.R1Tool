using BizHawk.Client.Common;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RayCarrot.BizHawk.R1Tool
{
    [ExternalTool(AppConstants.Title, Description = AppConstants.Description)]
    //[ExternalToolEmbeddedIcon(CustomMainForm.IconPath)] // TODO: Icon
    public partial class MainForm : Form, IExternalToolForm, IBizHawkAPI
    {
        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            // Set text
            Text = $"{AppConstants.Title} - Version {AppConstants.Version.ToString(3)}";

            // Set supported game versions
            SupportedVersions = new SupportedGameVersion[]
            {
                new SupportedGameVersion("Rayman 1 (PS1 - US)", "Rayman (USA)", () => new GameVersion_Rayman1_PS1_US(AddLog, this))
            };

            // Add restart options for supported game versions
            foreach (var v in SupportedVersions)
            {
                var item = restartToolStripMenuItem.DropDownItems.Add(v.Title);
                item.Click += (s, o) =>
                {
                    SelectedVersion = v;
                    Init();
                };
            }
        }

        #endregion

        #region Properties

        public ToolManager? Manager { get; private set; }
        public SupportedGameVersion[] SupportedVersions { get; }
        public SupportedGameVersion? SelectedVersion { get; set; }

        #endregion

        #region API

        [RequiredApi]
        public IMemoryApi Mem { get; set; }

        [RequiredApi]
        public IGuiApi Gui { get; set; }

        [RequiredApi]
        public IEmulationApi Emu { get; set; }

        [RequiredApi]
        public IEmuClientApi EmuClient { get; set; }

        [RequiredApi]
        public IGameInfoApi GameInfo { get; set; }

        [RequiredApi]
        public IInputApi Input { get; set; }

        [RequiredApi]
        public IUserDataApi Data { get; set; }

        [RequiredApi]
        public IMemoryEventsApi MemEvents { get; set; }

        #endregion

        #region APIHawk

        public void UpdateValues(ToolFormUpdateType type)
        {
            switch (type)
            {
                case ToolFormUpdateType.General:
                    Init();
                    break;
                
                case ToolFormUpdateType.PreFrame:
                    if (Manager == null)
                        return;

                    Gui.WithSurface(DisplaySurfaceID.EmuCore, () => Manager.UpdateUI());
                    Manager.HandleButtons();
                    break;

                case ToolFormUpdateType.PostFrame:
                    if (Manager == null)
                        return;

                    Manager.Update();
                    break;
            }
        }

        public void Restart() => Init();

        public bool AskSaveChanges() => true;

        #endregion

        #region Methods

        private void Init()
        {
            // Stop running previous instance
            Manager = null;

            var selectedVersion = SelectedVersion;
            var gameTitle = GameInfo.GetRomName();

            // If no version is selected we attempt to find the version from the game title
            if (selectedVersion == null)
            {
                // If there is no title we assume no game is loaded
                if (String.IsNullOrWhiteSpace(gameTitle) || gameTitle == "Null")
                {
                    AddLog($"Please load a supported game");
                    toolStripStatusLabel1.Text = $"Not running";
                    return;
                }

                // Attempt to find the game version based on matching title
                foreach (var v in SupportedVersions)
                {
                    if (gameTitle != v.GameName) 
                        continue;

                    selectedVersion = v;
                    break;
                }
            }

            // If no version was found we can't create a manager
            if (selectedVersion == null)
            {
                AddLog($"The loaded game '{gameTitle}' is not supported!");
                toolStripStatusLabel1.Text = $"Not running";
                return;
            }

            AddLog($"Started");
            toolStripStatusLabel1.Text = $"Running - Game: {selectedVersion.Title}";

            // Create a manager for the selected game version
            Manager = new ToolManager(this, selectedVersion.NewInstanceFunc());
        }

        private void AddLog(string x) => logTextBox.AppendText($"{x}{Environment.NewLine}");

        #endregion

        #region Event Handlers

        private void RestartAutoStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedVersion = null;
            Init();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void LogDataToolStripMenuItem_Click(object sender, EventArgs e) => Manager?.GameVersion.LogData();

        private void GitHubToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start(AppConstants.GitHubURL);

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutForm().Show();

        #endregion

        #region Classes

        public class SupportedGameVersion
        {
            public SupportedGameVersion(string title, string gameName, Func<IGameVersion> newInstanceFunc)
            {
                Title = title;
                GameName = gameName;
                NewInstanceFunc = newInstanceFunc;
            }

            public string Title { get; }
            public string GameName { get; }
            public Func<IGameVersion> NewInstanceFunc { get; }
        }

        #endregion
    }
}