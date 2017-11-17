using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Main {
    public class ProcessConfig {
        [JsonProperty("type")]
        public string Type;
        [JsonProperty("address")]
        public string Address;
        [JsonProperty("port")]
        public int Port;
        [JsonProperty("options")]
        public dynamic Options;
    }
}
