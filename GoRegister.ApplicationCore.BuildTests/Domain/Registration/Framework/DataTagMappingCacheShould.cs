using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.TestingCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Registration.Framework
{
    public class DataTagMappingCacheShould : DatabaseContextTest
    {
        
        [Fact]
        public void HandleMultipleTagsOfSameType()
        {
            using (var db = GetDatabase())
            {
                var fields = new List<Field>
                {
                    new FirstNameField { DataTag = "Name" },
                    new FirstNameField { DataTag = "Name" }
                };
                db.AddRange(fields);
                db.SaveChanges();

                var sut = new DataTagMappingCache(db);
                var result = sut.Get();

                result.Count.Should().Be(1);
            }
        }
    }
}
