namespace AspNetBot.Helpers
{
    public class JobSchedule
    {
        public Type JobType { get; }
        public string JobIdentity { get; }
        public string TriggerIdentity { get; }
        public string CronExpression { get; }
        public string ProfessionName { get; }
        public string Message { get; }

        public JobSchedule(Type jobType, string professonName,string message, string jobIdentity, string triggerIdentity, string cronExpression)
        {
            JobType = jobType;
            JobIdentity = jobIdentity;
            TriggerIdentity = triggerIdentity;
            CronExpression = cronExpression;
            ProfessionName = professonName;
            Message = message;
        }
    }
}
