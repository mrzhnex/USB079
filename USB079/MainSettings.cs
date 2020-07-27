using Exiled.API.Features;

namespace USB079
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(USB079);
        private SetEvents SetEvents { get; set; }

        public override void OnEnabled()
        {
            Global.IsFullRp = Config.IsFullRp;
            Log.Info(nameof(Global.IsFullRp) + ": " + Global.IsFullRp);
            SetEvents = new SetEvents();
            Exiled.Events.Handlers.Server.WaitingForPlayers += SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Player.ChangingRole += SetEvents.OnRoleChanging;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += SetEvents.OnSendingConsoleCommand;
            Exiled.Events.Handlers.Player.Spawning += SetEvents.OnSpawning;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Player.ChangingRole -= SetEvents.OnRoleChanging;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= SetEvents.OnSendingConsoleCommand;
            Exiled.Events.Handlers.Player.Spawning -= SetEvents.OnSpawning;
            Log.Info(Name + " off");
        }
    }
}