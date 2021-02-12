using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RayCarrot.BizHawk.R1Tool
{
    public class ToolManager
    {
        #region Constructor

        public ToolManager(IBizHawkAPI api, IGameVersion gameVersion)
        {
            // Set properties
            API = api;
            GameVersion = gameVersion;

            // Create properties for keeping track of selection and inputs
            SelectedMenuItem = new List<int>()
            {
                0
            };
            PreviousInputs = new Dictionary<string, bool>();

            // Get the menu
            Menu = GameVersion.Menu.Append(new MenuItem()
            {
                Title = () => "Setting",
                Children = new MenuItem[]
                {
                    new MenuItem()
                    {
                        Title = () => !RenderUIInGame ? "Display in game" : "Display in emulator",
                        OnSelected = x =>
                        {
                            RenderUIInGame = !RenderUIInGame;
                            return false;
                        }
                    }
                }.Concat(GameVersion.SettingsMenuItems).ToArray()
            }).ToArray();
            Values = GameVersion.Values;
            RenderUIInGame = true;
        }

        #endregion

        #region Protected Properties

        protected List<int> SelectedMenuItem { get; }
        protected Dictionary<string, bool> PreviousInputs { get; }

        #endregion

        #region Public Properties

        public IBizHawkAPI API { get; }
        public IGameVersion GameVersion { get; }
        public MenuItem[] Menu { get; }
        public ValueItem[] Values { get; }
        public bool ShowUI { get; set; }
        public bool RenderUIInGame { get; set; }

        #endregion

        #region Protected Methods

        protected MenuItem[] GetCurrentMenu()
        {
            var m = Menu;

            foreach (var s in SelectedMenuItem.Take(SelectedMenuItem.Count - 1))
                m = m[s].Children;

            return m;
        }
        protected int GetSelectedMenuItemIndex => SelectedMenuItem.Last();
        protected MenuItem GetSelectedMenuItem => GetCurrentMenu()[GetSelectedMenuItemIndex];
        protected bool HasBackOption => SelectedMenuItem.Count > 1;

        #endregion

        #region Public Methods

        public void UpdateUI()
        {
            // Update game version UI
            GameVersion.UpdateUI(ShowUI);

            // Don't draw the UI if we're not showing it
            if (!ShowUI)
                return;

            // Add selection index if not set
            if (!SelectedMenuItem.Any())
                SelectedMenuItem.Add(0);

            // Get current menu
            var menu = GetCurrentMenu();
            var selectedIndex = GetSelectedMenuItemIndex;

            // TODO: Allow these to be modified from settings
            // TODO: Only draw within the game frame
            // UI values
            int xPos = 50;
            Color defaultColor = Color.White;
            Color highlightColor = Color.Yellow;
            int yPos = 50;
            const int lineHeight = 20;

            // Add every menu item
            for (var i = 0; i < menu.Length; i++)
            {
                // Get the menu
                var m = menu[i];

                // Get the name
                var name = m.DisplayName;

                // If there are options for the item we add horizontal scroll indicators
                if (m.Options.Any())
                {
                    if (m.CanDecreaseOptionsIndex)
                        name = $"< {name}";
                    if (m.CanIncreaseOptionsIndex)
                        name = $"{name} >";
                }
                // If there are children we show an indicator
                if (m.Children.Any())
                    name = $"{name} ->";

                // Add the text to the UI
                DrawString(xPos, yPos, name, i == selectedIndex ? highlightColor : defaultColor);

                // Increment the y position
                yPos += lineHeight;
            }

            // Add the back option to return to previous menu
            if (HasBackOption)
            {
                yPos += lineHeight / 2;
                DrawString(xPos, yPos, "<- Back", selectedIndex == menu.Length ? highlightColor : defaultColor);
            }
            
            Color valueColor = Color.Orange;
            int valueYPos = 5;
            int valueXPos = 200;
            
            // Add values
            foreach (var v in Values)
            {
                DrawString(valueXPos, valueYPos, v.DisplayText, valueColor);
                valueYPos += lineHeight;
            }

            void DrawString(int x, int y, string str, Color c)
            {
                if (RenderUIInGame)
                    API.Gui.DrawString(x, y, str, c, fontsize: 12, fontstyle: "Bold", backcolor: Color.Black);
                else
                    API.Gui.Text(x, y, str, c);
            }
        }

        public void HandleButtons()
        {
            // Get the current inputs
            var input = API.Input.Get();

            // Helper for getting the state of a key, making sure it's been released before pressed again
            bool getKeyState(string name, bool singleInput = true)
            {
                if (!input.ContainsKey(name))
                {
                    PreviousInputs[name] = false;
                    return false;
                }

                if (PreviousInputs.ContainsKey(name) && PreviousInputs[name] && singleInput)
                {
                    PreviousInputs[name] = input[name];
                    return false;
                }

                PreviousInputs[name] = input[name];
                return input[name];
            }

            // Toggle showing the UI
            if (getKeyState("LeftCtrl", false) && getKeyState("X"))
                ToggleShowUI();

            // Don't handle inputs if we're not showing the UI
            if (!ShowUI)
                return;

            // Get properties
            var menu = GetCurrentMenu();
            var selectedIndex = GetSelectedMenuItemIndex;
            var menuItem = menu.ElementAtOrDefault(selectedIndex);

            // If the user selects the item
            if (getKeyState("Enter") || getKeyState("Space"))
            {
                // If null we go back to the previous menu
                if (menuItem == null)
                {
                    SelectedMenuItem.RemoveAt(SelectedMenuItem.Count - 1);
                    return;
                }

                // Call handler
                var r = menuItem.OnSelected?.Invoke(menuItem);

                // If there are children we navigate to the sub-menu
                if (menuItem.Children.Any())
                    SelectedMenuItem.Add(0);

                // Unpause if the action returned true
                if (r == true)
                {
                    ToggleShowUI();
                    return;
                }
            }
            
            // Menu navigation
            if (getKeyState("Down"))
            {
                if (selectedIndex < menu.Length - (HasBackOption ? 0 : 1))
                    SelectedMenuItem[SelectedMenuItem.Count - 1]++;
                else
                    SelectedMenuItem[SelectedMenuItem.Count - 1] = 0;
            }
            if (getKeyState("Up"))
            {
                if (selectedIndex > 0)
                    SelectedMenuItem[SelectedMenuItem.Count - 1]--;
                else
                    SelectedMenuItem[SelectedMenuItem.Count - 1] = menu.Length - (HasBackOption ? 0 : 1);
            }

            // Horizontal menu navigation
            if (menuItem?.Options.Any() == true)
            {
                if (getKeyState("Left") && menuItem.CanDecreaseOptionsIndex)
                    menuItem.SelectedOption--;
                if (getKeyState("Right") && menuItem.CanIncreaseOptionsIndex)
                    menuItem.SelectedOption++;
            }
        }

        public void ToggleShowUI()
        {
            // Toggle
            ShowUI = !ShowUI;

            // Update
            Update();

            // Pause the game while the debug menu is open
            GameVersion.IsPaused = ShowUI;

        }

        public void Update() => GameVersion.Update(ShowUI);

        #endregion
    }
}