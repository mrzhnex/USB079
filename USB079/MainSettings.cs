using EXILED;

namespace USB079
{
    public class MainSettings : Plugin
    {
        public override string getName => nameof(USB079);
        private SetEvents SetEvents { get; set; }

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.PlayerSpawnEvent += SetEvents.OnPlayerSpawn;
            Events.PlayerDeathEvent += SetEvents.OnPlayerDeath;
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.PlayerSpawnEvent -= SetEvents.OnPlayerSpawn;
            Events.PlayerDeathEvent -= SetEvents.OnPlayerDeath;
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}