namespace TestWebAPI.Data.Models
{
    public class ExperimentStatistic
    {
        public string Experiment { get; set; } = null!;
        public int CountDevices { get; set; }
        public Dictionary<string, int> Options { get; set; } = new Dictionary<string, int>();
    }
}
