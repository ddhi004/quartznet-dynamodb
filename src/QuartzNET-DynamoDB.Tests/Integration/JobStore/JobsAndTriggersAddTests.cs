﻿using System.Collections.Generic;
using Quartz.DynamoDB.Tests.Integration;
using Quartz.Job;
using Quartz.Simpl;
using Quartz.Spi;
using Xunit;

namespace Quartz.DynamoDB.Tests
{
    /// <summary>
    /// Contains tests related to the addition of Jobs and Triggers.
    /// </summary>
    public class JobsAndTriggersAddTests
    {
        private readonly IJobStore _sut;

        public JobsAndTriggersAddTests()
        {
            _sut = new JobStore();
            var signaler = new RamJobStoreTests.SampleSignaler();
            var loadHelper = new SimpleTypeLoadHelper();

            _sut.Initialize(loadHelper, signaler);
        }

        /// <summary>
        /// Tests that after storing new jobs and triggers, they can be retrieved.
        /// </summary>
        [Fact]
        [Trait("Category", "Integration")]
        public void StoreNewJobsAndTriggers()
        {
            var jobdetail1 = TestJobFactory.CreateTestJob();
            var trigger1 = TestTriggerFactory.CreateTestTrigger(jobdetail1.Name, jobdetail1.Group);

            var jobdetail2 = TestJobFactory.CreateTestJob();
            var trigger2 = TestTriggerFactory.CreateTestTrigger(jobdetail2.Name, jobdetail2.Group);

            var triggersAndJobs = new Dictionary<IJobDetail, Collection.ISet<ITrigger>>
            {
                {jobdetail1, new Collection.HashSet<ITrigger>(new[] {trigger1})},
                {jobdetail2, new Collection.HashSet<ITrigger>(new[] {trigger2})}
            };

            _sut.StoreJobsAndTriggers(triggersAndJobs, true);

            var retrievedJob1 = _sut.RetrieveJob(new JobKey(jobdetail1.Name, jobdetail1.Group));
            Assert.NotNull(retrievedJob1);
            Assert.Equal(jobdetail1.Name, retrievedJob1.Key.Name);
            Assert.Equal(jobdetail1.Group, retrievedJob1.Key.Group);
            Assert.Equal(typeof(NoOpJob), retrievedJob1.JobType);

            var retrievedtriggers1 = _sut.GetTriggersForJob(jobdetail1.Key);
            Assert.Equal(1, retrievedtriggers1.Count);
            Assert.Equal(trigger1.Key, retrievedtriggers1[0].Key);

            var retrievedJob2 = _sut.RetrieveJob(new JobKey(jobdetail2.Name, jobdetail2.Group));
            Assert.NotNull(retrievedJob2);
            Assert.Equal(jobdetail2.Name, retrievedJob2.Key.Name);
            Assert.Equal(jobdetail2.Group, retrievedJob2.Key.Group);
            Assert.Equal(typeof(NoOpJob), retrievedJob2.JobType);

            var retrievedtriggers2 = _sut.GetTriggersForJob(jobdetail2.Key);
            Assert.Equal(1, retrievedtriggers2.Count);
            Assert.Equal(trigger2.Key, retrievedtriggers2[0].Key);
        }
    }
}