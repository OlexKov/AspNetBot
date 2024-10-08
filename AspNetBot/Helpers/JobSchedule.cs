namespace AspNetBot.Helpers
{
    public class JobSchedule
    {
        public Type JobType { get; }
        public string JobIdentity { get; }
        public string TriggerIdentity { get; }
        public string CronExpression { get; }

        public JobSchedule(Type jobType, string jobIdentity, string triggerIdentity, string cronExpression)
        {
            JobType = jobType;
            JobIdentity = jobIdentity;
            TriggerIdentity = triggerIdentity;
            CronExpression = cronExpression;
        }
    }
}
