﻿using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.DynamoDB.DataModel.Storage;

namespace Quartz.DynamoDB.DataModel
{
    /// <summary>
    /// A wrapper class for a Quartz JobDetail instance that can be serialized and stored in Amazon DynamoDB.
    /// </summary>
	public class DynamoJob : IInitialisableFromDynamoRecord, IConvertibleToDynamoRecord, IDynamoTableType
    {
        private readonly SimpleTypeLoadHelper _typeHelper = new SimpleTypeLoadHelper();
		private readonly JobDataMapConverter jobDataMapConverter = new JobDataMapConverter();

		public DynamoJob()
		{
		}

        public DynamoJob(IJobDetail job)
        {
            this.Job = job;
        }

		public DynamoJob(Dictionary<string, AttributeValue> record)
		{
			InitialiseFromDynamoRecord (record);
		}

		public void InitialiseFromDynamoRecord(Dictionary<string, AttributeValue> record)
		{
			JobDetailImpl job = new JobDetailImpl();
			job.Key = new JobKey(record["Name"].S, record["Group"].S);
			job.Description = record["Description"].NULL ? string.Empty : record["Description"].S;
			job.JobType = _typeHelper.LoadType(record["JobType"].S);
			job.JobDataMap = (JobDataMap)jobDataMapConverter.FromEntry(record["JobDataMap"]);
			job.Durable = record["Durable"].BOOL;
			job.RequestsRecovery = record["RequestsRecovery"].BOOL;

			Job = job;
		}

		public string DynamoTableName  
		{
			get 
			{
				return DynamoConfiguration.JobDetailTableName;
			}
		}

		public Dictionary<string, AttributeValue> Key 
		{ 
			get 
			{
				return Job.Key.ToDictionary ();
			}
		}

        public IJobDetail Job { get; set; }

        public Dictionary<string, AttributeValue> ToDynamo()
        {
            Dictionary<string, AttributeValue> record = new Dictionary<string, AttributeValue>();
            
            record.Add("Name", new AttributeValue { S = Job.Key.Name });
            record.Add("Group", new AttributeValue { S = Job.Key.Group });
            record.Add("Description", string.IsNullOrWhiteSpace(Job.Description) ? new AttributeValue { NULL = true } : new AttributeValue { S = Job.Description });
            record.Add("JobType", new AttributeValue { S = GetStorableJobTypeName(Job.JobType) });
			record.Add("JobDataMap",jobDataMapConverter.ToEntry(Job.JobDataMap));
            record.Add("Durable", new AttributeValue { BOOL = Job.Durable });
            record.Add("PersistJobDataAfterExecution", new AttributeValue { BOOL = Job.PersistJobDataAfterExecution });
            record.Add("ConcurrentExecutionDisallowed", new AttributeValue { BOOL = Job.ConcurrentExecutionDisallowed });
            record.Add("RequestsRecovery", new AttributeValue { BOOL = Job.RequestsRecovery });

            return record;
        }

        private static string GetStorableJobTypeName(System.Type jobType)
        {
            return jobType.FullName + ", " + jobType.Assembly.GetName().Name;
        }
    }
}
