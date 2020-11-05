using System.Configuration;

namespace MMORL.Server
{
    public static class Settings
    {
        public static int Port => ParseInt("Port", 25501);
        public static int ChunkSize => ParseInt("ChunkSize", 16);
        public static float TurnTime => ParseFloat("TurnTime", 0.25f);

        private static int ParseInt(string key, int defaultValue)
        {
            string raw = ConfigurationManager.AppSettings[key];

            if (int.TryParse(raw, out int result))
            {
                return result;
            }

            return defaultValue;
        }

        private static float ParseFloat(string key, float defaultValue)
        {
            string raw = ConfigurationManager.AppSettings[key];

            if (float.TryParse(raw, out float result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}
