using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Domains;
using GoRegister.TestingCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Domains
{
    public class DomainServiceShould : DatabaseContextTest
    {
        private IMapper _mapper;

        public DomainServiceShould()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new DomainMappingProfile())));

            using (var db = GetDatabase())
            {
                db.Clients.Add(new Client { Id = 1, Name = "Test Client" });
                db.TenantUrls.AddRange(
                    new TenantUrl { Id = 101, ClientId = null, Host = "localhost:5021", IsSubdomainHost = false },
                    new TenantUrl { Id = 202, ClientId = 1, Host = "testdomain.com", IsSubdomainHost = false }
                    );
                db.SaveChanges();
            }
        }

        [Fact]
        public async Task SaveDomain()
        {
            using (var db = GetDatabase())
            {
                var sut = new DomainService(db, _mapper);
                var model = new DomainModel { Id = 0, Host = "newtestdomain.com" };
                await sut.SaveAsync(model);

                var result = db.TenantUrls.Last();

                result.Id.Should().BeGreaterThan(0);
                result.Host.Should().Be("newtestdomain.com");
            }
        }

        [Fact]
        public async Task Save_DomainWithClient()
        {
            using (var db = GetDatabase())
            {
                var sut = new DomainService(db, _mapper);
                var model = new DomainModel { Id = 0, Host = "newtestdomain.com", ClientId = 1 };
                await sut.SaveAsync(model);

                var result = db.TenantUrls.Where(u => u.Client.Id == 1).ToList();

                result.Count.Should().Be(2);
            }
        }

        [Fact]
        public async Task Set_ClientIdOfZero_ToNull()
        {
            using (var db = GetDatabase())
            {
                var sut = new DomainService(db, _mapper);
                var model = new DomainModel { Id = 0, Host = "newdomain.com", ClientId = 0 };
                var result = await sut.SaveAsync(model);

                result.Value.ClientId.Should().Be(null);
            }
        }

        [Fact]
        public async Task UpdateDomain()
        {
            using (var db = GetDatabase())
            {
                db.TenantUrls.Add(new TenantUrl { Id = 100, Host = "domain-to-update.com" });
                db.SaveChanges();

                var sut = new DomainService(db, _mapper);
                var model = new DomainModel { Id = 100, Host = "updated-domain.com" };
                await sut.SaveAsync(model);

                var result = await db.TenantUrls.FindAsync(100);

                result.Host.Should().Be("updated-domain.com");
            }
        }

        [Fact]
        public async Task NotAllow_DuplicateNames()
        {
            using (var db = GetDatabase())
            {
                var sut = new DomainService(db, _mapper);
                var model = new DomainModel {Host = "testdomain.com" };
                var result = await sut.SaveAsync(model);

                result.Failed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task DeleteDomain()
        {
            using (var db = GetDatabase())
            {
                db.TenantUrls.Add(new TenantUrl { Id = 1001, ClientId = null, Host = "localhost:1001", IsSubdomainHost = false });
                db.SaveChanges();
            }
            using (var db = GetDatabase())
            {
                var sut = new DomainService(db, _mapper);
                await sut.DeleteDomainsAsync(1001);
            }
            using (var db = GetDatabase())
            {
                var domains = db.TenantUrls.Find(1001);
                domains.Should().BeNull();
            }

        }

    }
}
