## About

This project is to bring Xbox controller support to StarCraft II, StarCraft: Remastered and WarCraft III: Reforged.

NOTE: This does work for WarCraft III Classic and Original StarCraft, but they have to be on the latest patch.

[PLEASE WATCH THIS DEMO VIDEO!](https://www.youtube.com/watch?v=yYyOBewhT2Q)

## System Requirements

- [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [AutoIt Full Installation](https://www.autoitscript.com/site/autoit/downloads/)
- Windows 10 or 11
- StarCraft / StarCraft 2 / WarCraft II: Remastered / WarCraft III
- Game controller
- Modify your game's settings using the instructions below. (click to open)
- Download the pre-compiled program [here](https://github.com/erfg12/Blizzard_Controller_Support/releases) and unzip the files into a folder and run the .exe program.

<details>
<summary>StarCraft: Remastered Game Settings (click here to open)</summary>

- Fullscreen Window Display Mode
    - `Options` > `Graphics` > `Display Mode: Windowed (Fullscreen)` > `Click Accept Button`
- Edit Game Hotkeys
    - Edit `%HOMEPATH%/Documents/StarCraft/CSettings.json` or `%HOMEPATH%/OneDrive/Documents/StarCraft/CSettings.json` with Notepad or VSCode.
    - Replace Hotkeys: with `"Hotkeys": "STR_RSRCH_STIM=w\nSTR_RSRCH_MAGNA=q\nSTR_RSRCH_EMP=q\nSTR_RSRCH_MINES=w\nSTR_RSRCH_SIEGE=e\nSTR_RSRCH_DEFMTX=m\nSTR_RSRCH_IRRADIATE=w\nSTR_RSRCH_YAMATO=q\nSTR_RSRCH_SHIP_CLOAK=q\nSTR_RSRCH_MAN_CLOAK=w\nSTR_USESTIM=z\nSTR_USEMAGNA=x\nSTR_USEMINES=z\nSTR_SCANNERSWEEP=q\nSTR_SIEGE_MODE=z\nSTR_TANK_MODE=z\nSTR_DEFMTX=z\nSTR_USEEMP=x\nSTR_IRRADIATE=c\nSTR_YAMATO=z\nSTR_CLOAK=z\nSTR_DECLOAK=z\nSTR_RSRCH_BURROW=e\nSTR_RSRCH_INFEST=i\nSTR_RSRCH_INFBROOD=q\nSTR_RSRCH_PLAGUE=s\nSTR_RSRCH_PARASITE=r\nSTR_RSRCH_BLOODBOIL=q\nSTR_RSRCH_ENSNARE=w\nSTR_RSRCH_CONSUME=w\nSTR_BURROW=c\nSTR_DEBURROW=c\nSTR_INFEST=d\nSTR_INFBROOD=x\nSTR_PLAGUE=z\nSTR_PARASITE=z\nSTR_BLOODBOIL=x\nSTR_CONSUME=d\nSTR_KERRIGAN_CONSUME=u\nSTR_ENSNARE=c\nSTR_RSRCH_PSISTORM=q\nSTR_RSRCH_HALLUCINATION=w\nSTR_RSRCH_RECALL=q\nSTR_RSRCH_STASIS=w\nSTR_RSRCH_SUMMON_ARCHON=a\nSTR_PSISTORM=z\nSTR_HALLUCINATION=x\nSTR_RECALL=z\nSTR_STASIS=x\nSTR_MAKE_P_ARCHON=c\nSTR_UP_T_ARMOR=w\nSTR_UP_T_VEHICLE_PLATING=a\nSTR_UP_T_SHIP_PLATING=s\nSTR_UP_Z_CARAPACE=e\nSTR_UP_Z_PLATING=w\nSTR_UP_P_ARMOR=w\nSTR_UP_P_PLATING=w\nSTR_UP_T_MAN_GUNS=q\nSTR_UP_T_VEHICLE_GUNS=q\nSTR_UP_T_SHIP_GUNS=w\nSTR_UP_Z_MELEE_ATTACKS=q\nSTR_UP_Z_MISSILE_ATTACKS=w\nSTR_UP_Z_FLYER_ATTACKS=q\nSTR_UP_P_GND_WEAPONS=q\nSTR_UP_P_AIR_WEAPONS=q\nSTR_UP_P_SHIELDS=e\nSTR_UP_MARINE_GUN_RANGE=q\nSTR_UP_VULTURE_SPEED=q\nSTR_UP_VESSEL_ENERGY=e\nSTR_UP_GHOST_SIGHT=a\nSTR_UP_GHOST_ENERGY=s\nSTR_UP_WRAITH_ENERGY=w\nSTR_UP_CRUISER_ENERGY=w\nSTR_UP_OVERLORD_TRANSPORT=a\nSTR_UP_OVERLORD_SIGHT=s\nSTR_UP_OVERLORD_SPEED=l\nSTR_UP_ZERGLING_SPEED=q\nSTR_UP_ZERGLING_ATTACK_SPEED=w\nSTR_UP_HYDRALISK_SPEED=q\nSTR_UP_HYDRALISK_ATTACK_RANGE=w\nSTR_UP_QUEEN_ENERGY=e\nSTR_UP_DEFILER_ENERGY=e\nSTR_UP_DRAGOON_ATTACK_RANGE=e\nSTR_UP_ZEALOT_SPEED=q\nSTR_UP_SCARAB_DAMAGE=q\nSTR_UP_REAVER_CAPACITY=w\nSTR_UP_SHUTTLE_SPEED=e\nSTR_UP_OBSERVER_SIGHT=w\nSTR_UP_OBSERVER_SPEED=q\nSTR_UP_TEMPLAR_ENERGY=e\nSTR_UP_SCOUT_SIGHT=q\nSTR_UP_SCOUT_SPEED=w\nSTR_UP_CARRIER_CAPACITY=e\nSTR_UP_ARBITER_ENERGY=e\nSTR_MAKE_Z_ZERGLING=w\nSTR_MAKE_Z_HYDRALISK=a\nSTR_MAKE_Z_ULTRALISK=x\nSTR_MAKE_Z_DRONE=q\nSTR_MAKE_Z_OVERLORD=e\nSTR_MAKE_Z_MUTALID=s\nSTR_GUARDIAN_ASPECT=z\nSTR_MAKE_Z_QUEEN=z\nSTR_MAKE_Z_DEFILER=c\nSTR_MAKE_Z_AVENGER=d\nSTR_MAKE_Z_INFESTED=q\nSTR_MAKE_T_MARINE=q\nSTR_MAKE_T_GHOST=e\nSTR_MAKE_T_FIREBAT=w\nSTR_MAKE_T_VULTURE=v\nSTR_MAKE_T_GOLIATH=g\nSTR_MAKE_T_TANK=t\nSTR_MAKE_T_SCV=q\nSTR_MAKE_T_WRAITH=q\nSTR_MAKE_T_VESSEL=e\nSTR_MAKE_T_DROPSHIP=w\nSTR_MAKE_T_BCRUISER=a\nSTR_MAKE_T_NUKE=q\nSTR_MAKE_P_OBSERVER=e\nSTR_MAKE_P_PROBE=q\nSTR_MAKE_P_ZEALOT=q\nSTR_MAKE_P_DRAGOON=w\nSTR_MAKE_P_TEMPLAR=e\nSTR_MAKE_P_SHUTTLE=q\nSTR_MAKE_P_SCOUT=q\nSTR_MAKE_P_ARBITER=e\nSTR_MAKE_P_CARRIER=w\nSTR_MAKE_P_INTERCEPTOR=z\nSTR_MAKE_P_REAVER=w\nSTR_MAKE_P_SCARAB=z\nSTR_BLD_HATCHERY=q\nSTR_BLD_CREEP_COLONY=w\nSTR_BLD_ZEXTRACTOR=e\nSTR_BLD_SPAWNING=a\nSTR_BLD_EVO_CHAMBER=s\nSTR_BLD_HYDRA_DEN=z\nSTR_BLD_NYDUS=e\nSTR_BLD_SPIRE=q\nSTR_BLD_NEST=w\nSTR_BLD_ULTRA_CAVERN=a\nSTR_BLD_DEFILER_MOUND=s\nSTR_BLD_LAIR=z\nSTR_BLD_HIVE=z\nSTR_BLD_GREATERSPIRE=z\nSTR_BLD_SPORE_COLONY=z\nSTR_BLD_SUNKEN_COLONY=x\nSTR_NYDUS_EXIT=q\nSTR_BLD_NEXUS=q\nSTR_BLD_PYLON=w\nSTR_BLD_ASSIMILATOR=e\nSTR_BLD_GATEWAY=a\nSTR_BLD_FORGE=s\nSTR_BLD_PHOTON=d\nSTR_BLD_CYBER_CORE=z\nSTR_BLD_SHIELDBATT=x\nSTR_BLD_ROBOTICS=q\nSTR_BLD_OBSERVATORY=z\nSTR_BLD_CITADEL=e\nSTR_BLD_ARCHIVES=d\nSTR_BLD_STARGATE=w\nSTR_BLD_FLEET_BEACON=s\nSTR_BLD_TRIBUNAL=x\nSTR_BLD_ROBOTICS_BAY=a\nSTR_BLD_TCOMMANDCTR=q\nSTR_BLD_DEPOT=w\nSTR_BLD_REFINERY=e\nSTR_BLD_BARRACKS=a\nSTR_BLD_ENGINEERING=s\nSTR_BLD_TURRET=d\nSTR_BLD_ACADEMY=z\nSTR_BLD_PILLBOX=x\nSTR_BLD_FACTORY=q\nSTR_BLD_TSTARPORT=w\nSTR_BLD_SCIENCE_FAC=e\nSTR_BLD_ARMORY=a\nSTR_BLD_COMSAT=z\nSTR_BLD_SILO=x\nSTR_BLD_DOCKS=z\nSTR_BLD_COVERT_OPS=z\nSTR_BLD_PHYSICS=x\nSTR_BLD_MACHINE=z\nSTR_MOVE=q\nSTR_STOP=w\nSTR_ATTACK=e\nSTR_PATROL=a\nSTR_HOLD=s\nSTR_WAYPOINTS=w\nSTR_LAND=c\nSTR_LIFTOFF=c\nSTR_RALLYPOINT=k\nSTR_RECHARGE=q\nSTR_SELECT_LARVA=q\nSTR_GATHER=s\nSTR_RETURN=d\nSTR_REPAIR=a\nSTR_BUILD=z\nSTR_BLD_ADVANCED=x\nSTR_MUTATE=z\nSTR_MUTATE_ADV=x\nSTR_MORPH_ADV=v\nSTR_PICKUP=x\nSTR_UNLOAD=c\nSTR_NUKESTRIKE=c\nSTR_PLACE_COP=p\nSTR_RSRCH_CURE=a\nSTR_RSRCH_MYOPIA=s\nSTR_HEAL=z\nSTR_CURE=x\nSTR_MYOPIA=c\nSTR_RSRCH_LURKERASPECT=a\nSTR_RSRCH_DISRUPTOR=a\nSTR_RSRCH_MINDCONTROL=a\nSTR_RSRCH_PSYFEEDBACK=f\nSTR_RSRCH_PARALIZE=s\nSTR_MAKE_P_DARCHON=c\nSTR_DISRUPTOR=z\nSTR_MINDCONTROL=x\nSTR_PSYFEEDBACK=z\nSTR_USEPARALIZE=c\nSTR_UP_MEDIC_ENERGY=d\nSTR_UP_T_MISSILE_BOOST=a\nSTR_UP_Z_ULTRA_SPEED=q\nSTR_UP_Z_ULTRA_ARMOR=w\nSTR_UP_CORSAIR_ENERGY=s\nSTR_UP_DARCHON_ENERGY=d\nSTR_DEVOURER_ASPECT=x\nSTR_MAKE_Z_LURKER=z\nSTR_MAKE_T_MEDIC=a\nSTR_MAKE_T_FRIGATE=s\nSTR_MAKE_P_CORSAIR=a\nSTR_MAKE_P_DTEMPLAR=a\n",` 
    - Save and Close.
</details>

<details>
<summary>StarCraft 2 Game Settings (click here to open)</summary>

- Enable Grid Hotkeys
    - `Options` > `Hotkeys` > `Selected Profile: Grid` > `Click Accept Button`
- Fullscreen Window Display Mode
    - `Options` > `Graphics` > `Display Mode: Windowed (Fullscreen)` > `Click Accept Button`
</details>

<details>
<summary>WarCraft II: Remastered Game Settings (click here to open)</summary>

- Enable Grid Hotkeys
    - `Options` > `Gameplay` > `Grid layout hot keys` > `Click to check this box`
- Enable Classic UI
    - `Options` > `Preferences` > `UI Scale` > `Classic` > `Click to check the radio button`
</details>

<details>
<summary>WarCraft III: Reforged Game Settings (click here to open)</summary>

- Enable Grid Hotkeys
    - `Options` > `Input` > `Preset Keybindings` > `Grid`
- Fullscreen Window Display Mode
    - `Options` > `Graphics` > `Display Mode: Windowed (Fullscreen)` > `Click Accept Button`
- Keep mouse in Window
    - `Options` > `Input` > `Confine Mouse Cursor` > `On`
</details>

## Additional Information

- If your computer has a touchscreen monitor _(Ex: GPDWin)_ and you touch the screen, it will make the cursor disappear. You have to move the mouse to re-display the cursor. If using a GPDWin, switch it to mouse mode, move it around, then back to game controller mode.

- All games must use a Windowed fullscreen mode for the overlay to show. Please use 1920 x 1080 game display for the overlay to fit properly.

- If you want to use [Steam](https://store.steampowered.com/streaming/) to stream to your game, add Blizzard_Controller.exe as a game to Steam. After launching Blizzard_Controller, click the button "Start Battle.net". Otherwise I recommend using [Rainway](https://rainway.com/), [Parsec](https://parsecgaming.com/) or [Moonlight](https://moonlight-stream.org/) for ease of use.

- Battle.net must have you logged in and be running in the background prior to Blizzard_Controller launching, or else you'll get a "Required application data not found" error message.

- Games (SC1, SC2, WC3) must be launched by the Steam remote user. Please have the games closed prior to the remote session starting or the controller will not work.

## Xbox Controller Controls
- Left Joystick: Move cursor
- Left Joystick Press: Shift Key (queue work)
- Right Joystick: Move camera around. Hold RT to zoom camera in/out.
- A Button: Left mouse click
- X Button: Right mouse click
- Y Button: Select non-busy workers (F1)
- B Button: Select non-busy soldiers (F2)
- Direction Pad: Select group 1-4 (hold RT to create group)
- Right Trigger: Middle mouse click for cursor drag (move left joystick for camera moving)
- RB, LB, LT: Modifier triggers for bottom right command grid

Use RB, LB and LT to select command grid rows.
Use A, X, Y, B and Back to select command grid columns.

The in-game overlay will help guide you to which command button you want to press.

<details>
<summary>Compiling Source Code (click here to open)</summary>

You need [Visual Studio 2022](https://visualstudio.microsoft.com/vs/), 
[VS Installer Projects Extension](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2022InstallerProjects) 
and [AutoIt Full Installation](https://www.autoitscript.com/site/autoit/downloads/)  

The installer for AutoIt should be in a zip when downloaded, unzip the exe and let it sit in your downloads folder to make the Setup project. 
The Setup project looks for `autoit-v3-setup.exe` in your `C:/users/(YOUR_USERNAME)/Downloads` directory.
</details>