using Exiled.API.Interfaces;

namespace USB079
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool IsFullRp { get; set; } = false;
    }
}