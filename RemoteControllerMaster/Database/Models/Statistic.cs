namespace RemoteControllerMaster.Database.Models
{
    public class Statistic
    {
        public Guid StatisticId { get; set; }
        public Guid MachineId { get; set; }
        public DateTime Date { get; set; }
        public string Variable { get; set; }
        public string Value { get; set; }
    }
}
