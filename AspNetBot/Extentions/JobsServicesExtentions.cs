using AspNetBot.Helpers;
using AspNetBot.Jobs;
using AspNetBot.Telegram;
using Quartz;

namespace AspNetBot.Extentions
{
    public static class JobsServicesExtentions
    {
        public static void AddJobs(this IServiceCollection services)
        {
            var jobSchedules = new List<JobSchedule>
            {
                 new(
                     typeof(SendNotificationJob),
                    "Вчитель",
                    "наша команда щиро вітає вас з Днем Вчителя та бажає всього найкращого !!!!",
                    "TeacherDayNotificationJob", "TeacherDayNotificationJobTrigger", "0 50 * ? * * *"),
                 new(
                     typeof(SendNotificationJob),
                     "Водій",
                     "наша команда щиро вітає вас з Днем Водія та бажає всього найкращого !!!!",
                     "DriverDayNotificationJob", "DriverDayNotificationJobTrigger", "0 50 * ? * * *"),
                 new(
                     typeof(SendNotificationJob),
                     "Програміст",
                     "наша команда щиро вітає вас з Днем Програмістa та бажає всього найкращого !!!!",
                     "ProgrammerDayNotificationJob", "ProgrammerDayNotificationJobTrigger", "0 6 23 ? * * *"),
                 new(
                     typeof(SendNotificationJob),
                     "Будівельник",
                     "наша команда щиро вітає вас з Днем Будівельникa та бажає всього найкращого !!!!",
                     "BuilderDayNotificationJob", "BuilderDayNotificationJobTrigger", "0 50 * ? * * *"),
                  new(
                      typeof(SendNotificationJob),
                     "Лікар",
                     "наша команда щиро вітає вас з Днем Лікаря та бажає всього найкращого !!!!",
                     "DoctorDayNotificationJob", "DoctorDayNotificationJobTrigger", "0 50 * ? * * *"),
                  new(
                      typeof(SendNotificationJob),
                     "Військовий",
                     "наша команда щиро вітає вас з Днем Військового та бажає всього найкращого !!!!",
                     "MilitaryDayNotificationJob", "MilitaryDayNotificationJobTrigger", "0 50 * ? * * *"),
                  new(
                      typeof(SendNotificationJob),
                     "Інженер",
                     "наша команда щиро вітає вас з Днем Інженера та бажає всього найкращого !!!!",
                     "EngineerDayNotificationJob", "EngineerDayNotificationJobTrigger", "0 50 * ? * * *"),
                  new(
                      typeof(SendNotificationJob),
                     "Пілот",
                     "наша команда щиро вітає вас з Днем Пілота та бажає всього найкращого !!!!",
                     "PilotDayNotificationJob", "PilotDayNotificationJobTrigger", "0 50 * ? * * *"),
                  new(
                     typeof(SendNotificationJob),
                     "Юрист",
                     "наша команда щиро вітає вас з Днем Юриста та бажає всього найкращого !!!!",
                     "LawyerDayNotificationJob", "LawyerDayNotificationJobTrigger", "0 50 * ? * * *")

                // ... more jobs
            };

            services.AddQuartz(q =>
            {
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(maxConcurrency: 10);
                q.UseInMemoryStore();


                q.AddJob<TelegramBot>(opts => opts.WithIdentity("TelegramBotJob"));
                q.AddTrigger(opts => opts
                    .ForJob("TelegramBotJob")
                    .WithIdentity("TelegramBotJobTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithRepeatCount(0)));

                foreach (var schedule in jobSchedules)
                {
                    q.AddJob(schedule.JobType,configure:opts => opts
                        .WithIdentity(schedule.JobIdentity)
                        .UsingJobData("profession", schedule.ProfessionName)
                        .UsingJobData("message", schedule.Message));
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
