﻿using System;
using Quartz.Simpl;
using Quartz.Spi;
using Xunit;

namespace Quartz.DynamoDB.Tests
{
    public class TriggerGroupGetTests
    {
        IJobStore _sut;

        public TriggerGroupGetTests()
        {
            _sut = new JobStore();
            var signaler = new Quartz.DynamoDB.Tests.Integration.RamJobStoreTests.SampleSignaler();
            var loadHelper = new SimpleTypeLoadHelper();

            _sut.Initialize(loadHelper, signaler);
        }

        /// <summary>
        /// Get paused trigger groups returns one record.
        /// </summary>
        [Fact]
        [Trait("Category", "Integration")]
        public void GetPausedTriggerGroupReturnsOneRecord()
        {
            //create a trigger group by calling for it to be paused.
            string triggerGroup = Guid.NewGuid().ToString();
            _sut.PauseTriggers(Quartz.Impl.Matchers.GroupMatcher<TriggerKey>.GroupEquals(triggerGroup));

            var result = _sut.GetPausedTriggerGroups();

            Assert.True(result.Contains(triggerGroup));
        }
    }
}

