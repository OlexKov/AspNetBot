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
                 new(typeof(SendTeachersDayNotificationJob), "TeachersDayNotificationJob", "SendTeachersDayNotificationJobTrigger", "0 50 * ? * * *")
                // ... more jobs
            };

            services.AddQuartz(q =>
            {
                q.AddJob<TelegramBotJob>(opts => opts.WithIdentity("TelegramBotJob"));
                q.AddTrigger(opts => opts
                    .ForJob("TelegramBotJob")
                    .WithIdentity("TelegramBotJobTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithRepeatCount(0)));

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
