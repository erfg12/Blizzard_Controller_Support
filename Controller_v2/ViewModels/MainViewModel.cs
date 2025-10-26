using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Blizzard_Controller;
using Blizzard_Controller.Configuration;
using Blizzard_Controller.Input;
using MsBox.Avalonia;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.ObjectModel;

namespace Controller_v2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// Get the shared AppSettings instance that synchronizes with the Controller project
    /// </summary>
    public AppSettings Settings => AppSettings.Instance;

    /// <summary>
    /// Expose individual properties for easier binding (delegates to shared settings)
    /// </summary>
    public double Deadzone
    {
        get => Settings.Deadzone;
        set => Settings.Deadzone = value;
    }

    public int CursorSpeed
    {
        get => Settings.CursorSpeed;
        set => Settings.CursorSpeed = value;
    }

    public bool VariableCursorSpeed
    {
        get => Settings.VariableCursorSpeed;
        set => Settings.VariableCursorSpeed = value;
    }

    public string GameDetectLabel
    {
        get => Settings.GameDetectLabel;
        set => Settings.GameDetectLabel = value;
    }

    public string ControllerDetectLabel
    {
        get => Settings.ControllerDetectLabel;
        set => Settings.ControllerDetectLabel = value;
    }

    public string SelectedButtonImageItem
{
    get => Settings.SelectedButtonImageItem;
    set => Settings.SelectedButtonImageItem = value;
}

    public MainViewModel()
    {
        // Subscribe to property changes from the shared settings to update the UI
        Settings.PropertyChanged += (sender, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(AppSettings.Deadzone):
                    OnPropertyChanged(nameof(Deadzone));
                    break;
                case nameof(AppSettings.CursorSpeed):
                    OnPropertyChanged(nameof(CursorSpeed));
                    break;
                case nameof(AppSettings.VariableCursorSpeed):
                    OnPropertyChanged(nameof(VariableCursorSpeed));
                    break;
                case nameof(AppSettings.GameDetectLabel):
                    OnPropertyChanged(nameof(GameDetectLabel));
                    break;
                case nameof(AppSettings.ControllerDetectLabel):
                    OnPropertyChanged(nameof(ControllerDetectLabel));
                    break;
                    case nameof(AppSettings.SelectedButtonImageItem):
                    OnPropertyChanged(nameof(SelectedButtonImageItem));
                    break;
            }
        };
    }

    public ObservableCollection<string> ButtonImageItems { get; } = new() { "Xbox", "Playstation" };

    [RelayCommand]
    public void OnExitButtonClick()
    {
        ControllerInputs.shuttingDown = true;
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }

        Task.Run(async () =>
        {
            // Wait briefly for background tasks to exit
            await Task.Delay(1500);
            Environment.Exit(0);
        });
    }

    [RelayCommand]
    public async void OnStartBattleNetButtonClick()
    {
        if (!GameSettings.startGame())
            await MessageBoxManager
            .GetMessageBoxStandard("ERROR", "Could not locate Battle.net directory.")
            .ShowAsync();
    }

    [RelayCommand]
    public async Task OnTerranGridLayoutButtonClickAsync()
    {
        if (!GameSettings.writeToCSettingsFile("STR_RSRCH_STIM=w\nSTR_RSRCH_MAGNA=q\nSTR_RSRCH_EMP=q\nSTR_RSRCH_MINES=w\nSTR_RSRCH_SIEGE=e\nSTR_RSRCH_DEFMTX=m\nSTR_RSRCH_IRRADIATE=w\nSTR_RSRCH_YAMATO=q\nSTR_RSRCH_SHIP_CLOAK=q\nSTR_RSRCH_MAN_CLOAK=w\nSTR_USESTIM=z\nSTR_USEMAGNA=x\nSTR_USEMINES=z\nSTR_SCANNERSWEEP=q\nSTR_SIEGE_MODE=z\nSTR_TANK_MODE=z\nSTR_DEFMTX=z\nSTR_USEEMP=x\nSTR_IRRADIATE=c\nSTR_YAMATO=z\nSTR_CLOAK=z\nSTR_DECLOAK=z\nSTR_RSRCH_BURROW=e\nSTR_RSRCH_INFEST=i\nSTR_RSRCH_INFBROOD=q\nSTR_RSRCH_PLAGUE=s\nSTR_RSRCH_PARASITE=r\nSTR_RSRCH_BLOODBOIL=q\nSTR_RSRCH_ENSNARE=w\nSTR_RSRCH_CONSUME=w\nSTR_BURROW=c\nSTR_DEBURROW=c\nSTR_INFEST=d\nSTR_INFBROOD=x\nSTR_PLAGUE=z\nSTR_PARASITE=z\nSTR_BLOODBOIL=x\nSTR_CONSUME=d\nSTR_KERRIGAN_CONSUME=u\nSTR_ENSNARE=c\nSTR_RSRCH_PSISTORM=q\nSTR_RSRCH_HALLUCINATION=w\nSTR_RSRCH_RECALL=q\nSTR_RSRCH_STASIS=w\nSTR_RSRCH_SUMMON_ARCHON=a\nSTR_PSISTORM=z\nSTR_HALLUCINATION=x\nSTR_RECALL=z\nSTR_STASIS=x\nSTR_MAKE_P_ARCHON=c\nSTR_UP_T_ARMOR=w\nSTR_UP_T_VEHICLE_PLATING=a\nSTR_UP_T_SHIP_PLATING=s\nSTR_UP_Z_CARAPACE=e\nSTR_UP_Z_PLATING=w\nSTR_UP_P_ARMOR=w\nSTR_UP_P_PLATING=w\nSTR_UP_T_MAN_GUNS=q\nSTR_UP_T_VEHICLE_GUNS=q\nSTR_UP_T_SHIP_GUNS=w\nSTR_UP_Z_MELEE_ATTACKS=q\nSTR_UP_Z_MISSILE_ATTACKS=w\nSTR_UP_Z_FLYER_ATTACKS=q\nSTR_UP_P_GND_WEAPONS=q\nSTR_UP_P_AIR_WEAPONS=q\nSTR_UP_P_SHIELDS=e\nSTR_UP_MARINE_GUN_RANGE=q\nSTR_UP_VULTURE_SPEED=q\nSTR_UP_VESSEL_ENERGY=e\nSTR_UP_GHOST_SIGHT=a\nSTR_UP_GHOST_ENERGY=s\nSTR_UP_WRAITH_ENERGY=w\nSTR_UP_CRUISER_ENERGY=w\nSTR_UP_OVERLORD_TRANSPORT=a\nSTR_UP_OVERLORD_SIGHT=s\nSTR_UP_OVERLORD_SPEED=l\nSTR_UP_ZERGLING_SPEED=q\nSTR_UP_ZERGLING_ATTACK_SPEED=w\nSTR_UP_HYDRALISK_SPEED=q\nSTR_UP_HYDRALISK_ATTACK_RANGE=w\nSTR_UP_QUEEN_ENERGY=e\nSTR_UP_DEFILER_ENERGY=e\nSTR_UP_DRAGOON_ATTACK_RANGE=e\nSTR_UP_ZEALOT_SPEED=q\nSTR_UP_SCARAB_DAMAGE=q\nSTR_UP_REAVER_CAPACITY=w\nSTR_UP_SHUTTLE_SPEED=e\nSTR_UP_OBSERVER_SIGHT=w\nSTR_UP_OBSERVER_SPEED=q\nSTR_UP_TEMPLAR_ENERGY=e\nSTR_UP_SCOUT_SIGHT=q\nSTR_UP_SCOUT_SPEED=w\nSTR_UP_CARRIER_CAPACITY=e\nSTR_UP_ARBITER_ENERGY=e\nSTR_MAKE_Z_ZERGLING=w\nSTR_MAKE_Z_HYDRALISK=a\nSTR_MAKE_Z_ULTRALISK=x\nSTR_MAKE_Z_DRONE=q\nSTR_MAKE_Z_OVERLORD=e\nSTR_MAKE_Z_MUTALID=s\nSTR_GUARDIAN_ASPECT=z\nSTR_MAKE_Z_QUEEN=z\nSTR_MAKE_Z_DEFILER=c\nSTR_MAKE_Z_AVENGER=d\nSTR_MAKE_Z_INFESTED=q\nSTR_MAKE_T_MARINE=q\nSTR_MAKE_T_GHOST=e\nSTR_MAKE_T_FIREBAT=w\nSTR_MAKE_T_VULTURE=q\nSTR_MAKE_T_GOLIATH=e\nSTR_MAKE_T_TANK=w\nSTR_MAKE_T_SCV=q\nSTR_MAKE_T_WRAITH=q\nSTR_MAKE_T_VESSEL=e\nSTR_MAKE_T_DROPSHIP=w\nSTR_MAKE_T_BCRUISER=a\nSTR_MAKE_T_NUKE=q\nSTR_MAKE_P_OBSERVER=e\nSTR_MAKE_P_PROBE=q\nSTR_MAKE_P_ZEALOT=q\nSTR_MAKE_P_DRAGOON=w\nSTR_MAKE_P_TEMPLAR=e\nSTR_MAKE_P_SHUTTLE=q\nSTR_MAKE_P_SCOUT=q\nSTR_MAKE_P_ARBITER=e\nSTR_MAKE_P_CARRIER=w\nSTR_MAKE_P_INTERCEPTOR=z\nSTR_MAKE_P_REAVER=w\nSTR_MAKE_P_SCARAB=z\nSTR_BLD_HATCHERY=q\nSTR_BLD_CREEP_COLONY=w\nSTR_BLD_ZEXTRACTOR=e\nSTR_BLD_SPAWNING=a\nSTR_BLD_EVO_CHAMBER=s\nSTR_BLD_HYDRA_DEN=z\nSTR_BLD_NYDUS=e\nSTR_BLD_SPIRE=q\nSTR_BLD_NEST=w\nSTR_BLD_ULTRA_CAVERN=a\nSTR_BLD_DEFILER_MOUND=s\nSTR_BLD_LAIR=z\nSTR_BLD_HIVE=z\nSTR_BLD_GREATERSPIRE=z\nSTR_BLD_SPORE_COLONY=z\nSTR_BLD_SUNKEN_COLONY=x\nSTR_NYDUS_EXIT=q\nSTR_BLD_NEXUS=q\nSTR_BLD_PYLON=w\nSTR_BLD_ASSIMILATOR=e\nSTR_BLD_GATEWAY=a\nSTR_BLD_FORGE=s\nSTR_BLD_PHOTON=d\nSTR_BLD_CYBER_CORE=z\nSTR_BLD_SHIELDBATT=x\nSTR_BLD_ROBOTICS=q\nSTR_BLD_OBSERVATORY=z\nSTR_BLD_CITADEL=e\nSTR_BLD_ARCHIVES=d\nSTR_BLD_STARGATE=w\nSTR_BLD_FLEET_BEACON=s\nSTR_BLD_TRIBUNAL=x\nSTR_BLD_ROBOTICS_BAY=a\nSTR_BLD_TCOMMANDCTR=q\nSTR_BLD_DEPOT=w\nSTR_BLD_REFINERY=e\nSTR_BLD_BARRACKS=a\nSTR_BLD_ENGINEERING=s\nSTR_BLD_TURRET=d\nSTR_BLD_ACADEMY=z\nSTR_BLD_PILLBOX=x\nSTR_BLD_FACTORY=q\nSTR_BLD_TSTARPORT=w\nSTR_BLD_SCIENCE_FAC=e\nSTR_BLD_ARMORY=a\nSTR_BLD_COMSAT=z\nSTR_BLD_SILO=x\nSTR_BLD_DOCKS=z\nSTR_BLD_COVERT_OPS=z\nSTR_BLD_PHYSICS=x\nSTR_BLD_MACHINE=z\nSTR_MOVE=q\nSTR_STOP=w\nSTR_ATTACK=e\nSTR_PATROL=a\nSTR_HOLD=s\nSTR_WAYPOINTS=w\nSTR_LAND=c\nSTR_LIFTOFF=c\nSTR_RALLYPOINT=k\nSTR_RECHARGE=q\nSTR_SELECT_LARVA=q\nSTR_GATHER=s\nSTR_RETURN=d\nSTR_REPAIR=a\nSTR_BUILD=z\nSTR_BLD_ADVANCED=x\nSTR_MUTATE=z\nSTR_MUTATE_ADV=x\nSTR_MORPH_ADV=v\nSTR_PICKUP=x\nSTR_UNLOAD=c\nSTR_NUKESTRIKE=c\nSTR_PLACE_COP=p\nSTR_RSRCH_CURE=a\nSTR_RSRCH_MYOPIA=s\nSTR_HEAL=z\nSTR_CURE=x\nSTR_MYOPIA=c\nSTR_RSRCH_LURKERASPECT=a\nSTR_RSRCH_DISRUPTOR=a\nSTR_RSRCH_MINDCONTROL=a\nSTR_RSRCH_PSYFEEDBACK=f\nSTR_RSRCH_PARALIZE=s\nSTR_MAKE_P_DARCHON=c\nSTR_DISRUPTOR=z\nSTR_MINDCONTROL=x\nSTR_PSYFEEDBACK=z\nSTR_USEPARALIZE=c\nSTR_UP_MEDIC_ENERGY=d\nSTR_UP_T_MISSILE_BOOST=a\nSTR_UP_Z_ULTRA_SPEED=q\nSTR_UP_Z_ULTRA_ARMOR=w\nSTR_UP_CORSAIR_ENERGY=s\nSTR_UP_DARCHON_ENERGY=d\nSTR_DEVOURER_ASPECT=x\nSTR_MAKE_Z_LURKER=z\nSTR_MAKE_T_MEDIC=a\nSTR_MAKE_T_FRIGATE=s\nSTR_MAKE_P_CORSAIR=a\nSTR_MAKE_P_DTEMPLAR=a\n"))
            await MessageBoxManager
            .GetMessageBoxStandard($"ERROR: Could not find CSettings.json file! Did you install the game on your {Environment.GetEnvironmentVariable("HOMEDRIVE")} drive?", "ERROR")
            .ShowAsync();
        else
            await MessageBoxManager
            .GetMessageBoxStandard("CSettings.json file modified successfully!", "SUCCESS").ShowAsync();
    }

    [RelayCommand]
    public async Task OnZergGridLayoutButtonClickAsync()
    {
        if (!GameSettings.writeToCSettingsFile("STR_RSRCH_STIM=w\nSTR_RSRCH_MAGNA=q\nSTR_RSRCH_EMP=q\nSTR_RSRCH_MINES=w\nSTR_RSRCH_SIEGE=e\nSTR_RSRCH_DEFMTX=m\nSTR_RSRCH_IRRADIATE=w\nSTR_RSRCH_YAMATO=q\nSTR_RSRCH_SHIP_CLOAK=q\nSTR_RSRCH_MAN_CLOAK=w\nSTR_USESTIM=z\nSTR_USEMAGNA=x\nSTR_USEMINES=z\nSTR_SCANNERSWEEP=q\nSTR_SIEGE_MODE=z\nSTR_TANK_MODE=z\nSTR_DEFMTX=z\nSTR_USEEMP=x\nSTR_IRRADIATE=c\nSTR_YAMATO=z\nSTR_CLOAK=z\nSTR_DECLOAK=z\nSTR_RSRCH_BURROW=e\nSTR_RSRCH_INFEST=i\nSTR_RSRCH_INFBROOD=q\nSTR_RSRCH_PLAGUE=s\nSTR_RSRCH_PARASITE=r\nSTR_RSRCH_BLOODBOIL=q\nSTR_RSRCH_ENSNARE=w\nSTR_RSRCH_CONSUME=w\nSTR_BURROW=c\nSTR_DEBURROW=c\nSTR_INFEST=d\nSTR_INFBROOD=x\nSTR_PLAGUE=z\nSTR_PARASITE=z\nSTR_BLOODBOIL=x\nSTR_CONSUME=d\nSTR_KERRIGAN_CONSUME=u\nSTR_ENSNARE=c\nSTR_RSRCH_PSISTORM=q\nSTR_RSRCH_HALLUCINATION=w\nSTR_RSRCH_RECALL=q\nSTR_RSRCH_STASIS=w\nSTR_RSRCH_SUMMON_ARCHON=a\nSTR_PSISTORM=z\nSTR_HALLUCINATION=x\nSTR_RECALL=z\nSTR_STASIS=x\nSTR_MAKE_P_ARCHON=c\nSTR_UP_T_ARMOR=w\nSTR_UP_T_VEHICLE_PLATING=a\nSTR_UP_T_SHIP_PLATING=s\nSTR_UP_Z_CARAPACE=e\nSTR_UP_Z_PLATING=w\nSTR_UP_P_ARMOR=w\nSTR_UP_P_PLATING=w\nSTR_UP_T_MAN_GUNS=q\nSTR_UP_T_VEHICLE_GUNS=q\nSTR_UP_T_SHIP_GUNS=w\nSTR_UP_Z_MELEE_ATTACKS=q\nSTR_UP_Z_MISSILE_ATTACKS=w\nSTR_UP_Z_FLYER_ATTACKS=q\nSTR_UP_P_GND_WEAPONS=q\nSTR_UP_P_AIR_WEAPONS=q\nSTR_UP_P_SHIELDS=e\nSTR_UP_MARINE_GUN_RANGE=q\nSTR_UP_VULTURE_SPEED=q\nSTR_UP_VESSEL_ENERGY=e\nSTR_UP_GHOST_SIGHT=a\nSTR_UP_GHOST_ENERGY=s\nSTR_UP_WRAITH_ENERGY=w\nSTR_UP_CRUISER_ENERGY=w\nSTR_UP_OVERLORD_TRANSPORT=a\nSTR_UP_OVERLORD_SIGHT=s\nSTR_UP_OVERLORD_SPEED=d\nSTR_UP_ZERGLING_SPEED=q\nSTR_UP_ZERGLING_ATTACK_SPEED=w\nSTR_UP_HYDRALISK_SPEED=q\nSTR_UP_HYDRALISK_ATTACK_RANGE=w\nSTR_UP_QUEEN_ENERGY=e\nSTR_UP_DEFILER_ENERGY=e\nSTR_UP_DRAGOON_ATTACK_RANGE=e\nSTR_UP_ZEALOT_SPEED=q\nSTR_UP_SCARAB_DAMAGE=q\nSTR_UP_REAVER_CAPACITY=w\nSTR_UP_SHUTTLE_SPEED=e\nSTR_UP_OBSERVER_SIGHT=w\nSTR_UP_OBSERVER_SPEED=q\nSTR_UP_TEMPLAR_ENERGY=e\nSTR_UP_SCOUT_SIGHT=q\nSTR_UP_SCOUT_SPEED=w\nSTR_UP_CARRIER_CAPACITY=e\nSTR_UP_ARBITER_ENERGY=e\nSTR_MAKE_Z_ZERGLING=w\nSTR_MAKE_Z_HYDRALISK=a\nSTR_MAKE_Z_ULTRALISK=x\nSTR_MAKE_Z_DRONE=q\nSTR_MAKE_Z_OVERLORD=e\nSTR_MAKE_Z_MUTALID=s\nSTR_GUARDIAN_ASPECT=z\nSTR_MAKE_Z_QUEEN=z\nSTR_MAKE_Z_DEFILER=c\nSTR_MAKE_Z_AVENGER=d\nSTR_MAKE_Z_INFESTED=q\nSTR_MAKE_T_MARINE=q\nSTR_MAKE_T_GHOST=e\nSTR_MAKE_T_FIREBAT=d\nSTR_MAKE_T_VULTURE=v\nSTR_MAKE_T_GOLIATH=g\nSTR_MAKE_T_TANK=t\nSTR_MAKE_T_SCV=q\nSTR_MAKE_T_WRAITH=q\nSTR_MAKE_T_VESSEL=e\nSTR_MAKE_T_DROPSHIP=d\nSTR_MAKE_T_BCRUISER=a\nSTR_MAKE_T_NUKE=q\nSTR_MAKE_P_OBSERVER=e\nSTR_MAKE_P_PROBE=q\nSTR_MAKE_P_ZEALOT=q\nSTR_MAKE_P_DRAGOON=d\nSTR_MAKE_P_TEMPLAR=e\nSTR_MAKE_P_SHUTTLE=q\nSTR_MAKE_P_SCOUT=q\nSTR_MAKE_P_ARBITER=e\nSTR_MAKE_P_CARRIER=d\nSTR_MAKE_P_INTERCEPTOR=z\nSTR_MAKE_P_REAVER=d\nSTR_MAKE_P_SCARAB=z\nSTR_BLD_HATCHERY=q\nSTR_BLD_CREEP_COLONY=w\nSTR_BLD_ZEXTRACTOR=e\nSTR_BLD_SPAWNING=a\nSTR_BLD_EVO_CHAMBER=s\nSTR_BLD_HYDRA_DEN=z\nSTR_BLD_NYDUS=e\nSTR_BLD_SPIRE=q\nSTR_BLD_NEST=w\nSTR_BLD_ULTRA_CAVERN=a\nSTR_BLD_DEFILER_MOUND=s\nSTR_BLD_LAIR=z\nSTR_BLD_HIVE=z\nSTR_BLD_GREATERSPIRE=z\nSTR_BLD_SPORE_COLONY=z\nSTR_BLD_SUNKEN_COLONY=x\nSTR_NYDUS_EXIT=q\nSTR_BLD_NEXUS=q\nSTR_BLD_PYLON=w\nSTR_BLD_ASSIMILATOR=e\nSTR_BLD_GATEWAY=a\nSTR_BLD_FORGE=s\nSTR_BLD_PHOTON=d\nSTR_BLD_CYBER_CORE=z\nSTR_BLD_SHIELDBATT=x\nSTR_BLD_ROBOTICS=q\nSTR_BLD_OBSERVATORY=z\nSTR_BLD_CITADEL=e\nSTR_BLD_ARCHIVES=d\nSTR_BLD_STARGATE=w\nSTR_BLD_FLEET_BEACON=s\nSTR_BLD_TRIBUNAL=x\nSTR_BLD_ROBOTICS_BAY=a\nSTR_BLD_TCOMMANDCTR=q\nSTR_BLD_DEPOT=w\nSTR_BLD_REFINERY=e\nSTR_BLD_BARRACKS=a\nSTR_BLD_ENGINEERING=s\nSTR_BLD_TURRET=d\nSTR_BLD_ACADEMY=z\nSTR_BLD_PILLBOX=x\nSTR_BLD_FACTORY=q\nSTR_BLD_TSTARPORT=w\nSTR_BLD_SCIENCE_FAC=e\nSTR_BLD_ARMORY=a\nSTR_BLD_COMSAT=z\nSTR_BLD_SILO=x\nSTR_BLD_DOCKS=z\nSTR_BLD_COVERT_OPS=z\nSTR_BLD_PHYSICS=x\nSTR_BLD_MACHINE=z\nSTR_MOVE=q\nSTR_STOP=w\nSTR_ATTACK=e\nSTR_PATROL=a\nSTR_HOLD=s\nSTR_WAYPOINTS=w\nSTR_LAND=c\nSTR_LIFTOFF=c\nSTR_RALLYPOINT=w\nSTR_RECHARGE=q\nSTR_SELECT_LARVA=q\nSTR_GATHER=s\nSTR_RETURN=d\nSTR_REPAIR=a\nSTR_BUILD=z\nSTR_BLD_ADVANCED=x\nSTR_MUTATE=z\nSTR_MUTATE_ADV=x\nSTR_MORPH_ADV=v\nSTR_PICKUP=x\nSTR_UNLOAD=c\nSTR_NUKESTRIKE=c\nSTR_PLACE_COP=p\nSTR_RSRCH_CURE=a\nSTR_RSRCH_MYOPIA=s\nSTR_HEAL=z\nSTR_CURE=x\nSTR_MYOPIA=c\nSTR_RSRCH_LURKERASPECT=a\nSTR_RSRCH_DISRUPTOR=a\nSTR_RSRCH_MINDCONTROL=a\nSTR_RSRCH_PSYFEEDBACK=f\nSTR_RSRCH_PARALIZE=s\nSTR_MAKE_P_DARCHON=c\nSTR_DISRUPTOR=z\nSTR_MINDCONTROL=x\nSTR_PSYFEEDBACK=z\nSTR_USEPARALIZE=c\nSTR_UP_MEDIC_ENERGY=d\nSTR_UP_T_MISSILE_BOOST=a\nSTR_UP_Z_ULTRA_SPEED=q\nSTR_UP_Z_ULTRA_ARMOR=w\nSTR_UP_CORSAIR_ENERGY=s\nSTR_UP_DARCHON_ENERGY=d\nSTR_DEVOURER_ASPECT=x\nSTR_MAKE_Z_LURKER=z\nSTR_MAKE_T_MEDIC=a\nSTR_MAKE_T_FRIGATE=s\nSTR_MAKE_P_CORSAIR=a\nSTR_MAKE_P_DTEMPLAR=a\n"))
            await MessageBoxManager
            .GetMessageBoxStandard($"ERROR: Could not find CSettings.json file! Did you install the game on your {Environment.GetEnvironmentVariable("HOMEDRIVE")} drive?", "ERROR").ShowAsync();
        else
            await MessageBoxManager
            .GetMessageBoxStandard("CSettings.json file modified successfully!", "SUCCESS").ShowAsync();
    }

    [RelayCommand]
    public async Task OnRestoreDefaultHotkeysButtonClickAsync()
    {
        if (!GameSettings.writeToCSettingsFile(""))
            await MessageBoxManager
            .GetMessageBoxStandard($"ERROR: Could not find CSettings.json file! Did you install the game on your {Environment.GetEnvironmentVariable("HOMEDRIVE")} drive?", "ERROR").ShowAsync();
        else
            await MessageBoxManager
            .GetMessageBoxStandard("CSettings.json file modified successfully!", "SUCCESS").ShowAsync();
    }
}
