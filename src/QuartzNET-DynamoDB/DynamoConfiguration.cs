﻿namespace Quartz.DynamoDB
{
    public class DynamoConfiguration
    {
        public static string JobDetailTableName => "Job";

		public static string JobGroupTableName => "JobGroup";

        public static string TriggerTableName => "Trigger";

		public static string TriggerGroupTableName => "TriggerGroup";

        public static string SchedulerTableName => "Scheduler";

    }
}
