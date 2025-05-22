using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace BouncyBall
{
    public class BouncyBall : Plugin<Config>
    {
        public BouncyBall()
        {
        }

        public override PluginPriority Priority => PluginPriority.Medium;
        public static BouncyBall instance;

        private EventsHandler _handler;

        public override void OnEnabled()
        {
            _handler = new EventsHandler();
            Exiled.Events.Handlers.Server.WaitingForPlayers += _handler.OnWaitingForPlayers;
            
            instance = this;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= _handler.OnWaitingForPlayers;
            _handler = null;
            
            instance = null;
            base.OnDisabled();
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
    }
}

