# Rayman 1 Tools (work in process)

<img alt="Logo" src="/img/Showcase.gif" width="526">

# TODO:
There are currently multiple issues preventing me from creating a release of this tool. If anyone wants to help out, please let me know or make a pull request! I'd welcome any help with this, no matter how small. Below is a list of what is left to do for the tool before it can be released.

### General
* Create an icon, specify its path in MainForm.cs as an attribute and use for all forms.
* Add multiple versions of Rayman 1 supported by BizHawk, such as all PS1 versions, GBA, Saturn etc. Also include the Rayman 2 2D prototype since it uses the same engine.
* Fix general TODO notes in the code.

### Pause the game
Find better way to pause the game when the debug menu is showing (see `GameVersion_Rayman1.IsPaused`). The current way is very hacky, enabling PLACE_RAY with speed 0, thus freezing Rayman and not allowing him to move. The problem with this is that it only works in a level and not in menus or the world map. Other option I've tried are to set gele to an invalid value (gele = freeze, used for when the cage medallion shows or you reach the exit sign), but this has some weird side effects such as resetting your progress (why?), and pausing the emulator itself, which seems to prevent the update functions to be called each frame, thus not drawing the menu UI.

### Level switching
Currently level switching works between levels in the currently loaded world (see `GameVersion_Rayman1.Menu`). This is done by simulating the behavior of reaching the exit sign or entering a bonus level in-game. There are multiple issues here:
* This code only works when in a level. You can not enter a level from a menu due to the menu loop not checking any of the relevant values, and it can not be done from the world map either as whenever the world map loop exits it will always overwrite the `num_level_choice` and `num_world_choice` values to the value based on where you are on the world map (the only exception is if `ModeDemo` is true, but that has other side-effects). The world map issue could be solved by using `API.MemEvents` to hook into whenever these values get written to and then immediately overwrite them back to what we want, but a lot could go wrong when doing such a thing due to the next issue:
* There is no consistent way to know which game loop you are in (menu, world map, level etc.). This makes it very difficult to know which values to modify. We could find this by hooking into when certain functions get called, but ideally everything would be done using the game values for better support between the multiple versions of the game.
* You can't change worlds without first entering the world map. This is due to how the game loops work. The game only changes which world is loaded whenever you enter a level from the world map. Changing levels from within a world will always use the same world you are currently in.
* An option is available to change the loaded menu, but this only works if you're in a level or the world map.

### Game overlays
An important part of the tool is to implement game overlays. Ideally you'd be able to with your mouse hover over and select game objects to see some variables for them (such as hitPoints, objType, speedX/Y etc.) as well as viewing game collision. There are multiple issues here:
* For PS1 specifically we've got issues with the positioning which I could not figure out. As per most PS1 games there's horizontal overscan which is rendered as part of the game draw buffer, but this is not part of the game's position calculations, so must be added in when the sprites are actually drawn. In `GameVersion_Rayman1.UpdateUI` I hard-coded it to be 18, but this is not ideal. The default PS1 settings in BizHawk will also display the game in double scale it seems, which means all coordinates need to be multiplied by 2 in this case.
* The tile collision overlays, which draws actual bitmap images to the screen, causes the game to lag and become unplayable, even with caching enabled.
* Object collision seems nearly impossible to show. The game has multiple different object collision systems it checks, most of which are hard-coded. A few general ones are used such as the ZDC and hitSprite systems we could use.

### Menu UI
The biggest issue here is size and positioning for the menu UI. I made an option where you can choose between displaying it in the game or emulator. If it displays in the emulator it looks a lot cleaner, but won't adjust to the game size/positioning, while showing in-game looks very low-res due to using the game resolution. This would also all have to be adjusted based on the game resolution as the GBA version uses a different aspect-ratio and resolution than PS1, so the size/position values should ideally not be hard-coded, see `ToolManager.UpdateUI`.

# Compile & run
* Clone or download the repo
* Copy a BizHawk (2.6.0) installation to a folder named `BizHawk` in the same directory as the `src` folder and readme file
* Open `RayCarrot.BizHawk.R1Tool.sln` in Visual Studio and build
* Open BizHawk, load a supported game and run the tool from `Tools > External Tool > Rayman 1 Tool`
