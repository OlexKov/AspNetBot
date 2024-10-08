using AspNetBot.Helpers;
using AspNetBot.Jobs;
using Quartz;

namespace AspNetBot.Extentions
{
    public static class JobsServicesExtentions
    {
        public static void AddJobs(this IServiceCollection services)
        {
            var jobSchedules = new List<JobSchedule>
            {
                 new(typeof(SendNotificationJob), "NotificationJob", "NotificationTrigger", "0/5 * * ? * * *")
                // ... more jobs
            };

            services.AddQuartz(q =>
            {
                foreach (var schedule in jobSchedules)
                {
                    q.AddJob(schedule.JobType,configure:opts => opts.WithIdentity(schedule.JobIdentity));
                    q.AddTrigger(opts => opts
                        .ForJob(schedule.JobIdentity)
                        .WithIdentity(schedule.TriggerIdentity)
                        .WithCronSchedule(schedule.CronExpression));
                }
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
