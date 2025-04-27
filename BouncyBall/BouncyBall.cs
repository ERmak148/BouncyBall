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

        public override void OnEnabled()
        {
            instance = this;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
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

