using Moq;
using MySarAssistModels.Interfaces;
using MySarAssistModels.People;
using MySarAssistModels.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySarAssistUnitTests
{
    [TestClass]
    public class OrganizationSyncServiceTests
    {
        [TestMethod]
        public async Task SyncOrganizationsAsync_WhenApiReturnsNull_DoesNotUpsert()
        {
            var mockRestService = new Mock<IRestService>();
            var mockOrgStore = new Mock<IDataStore<Organization>>();

            mockRestService.Setup(s => s.GetOrganizationsAsync())
                           .ReturnsAsync((List<Organization>?)null);

            var sut = new OrganizationSyncService(mockRestService.Object, mockOrgStore.Object);
            await sut.SyncOrganizationsAsync();

            mockOrgStore.Verify(s => s.UpsertItemAsync(It.IsAny<Organization>()), Times.Never);
        }

        [TestMethod]
        public async Task SyncOrganizationsAsync_WhenApiReturnsEmptyList_DoesNotUpsert()
        {
            var mockRestService = new Mock<IRestService>();
            var mockOrgStore = new Mock<IDataStore<Organization>>();

            mockRestService.Setup(s => s.GetOrganizationsAsync())
                           .ReturnsAsync(new List<Organization>());

            var sut = new OrganizationSyncService(mockRestService.Object, mockOrgStore.Object);
            await sut.SyncOrganizationsAsync();

            mockOrgStore.Verify(s => s.UpsertItemAsync(It.IsAny<Organization>()), Times.Never);
        }

        [TestMethod]
        public async Task SyncOrganizationsAsync_WhenApiReturnsTwoOrgs_UpsertsEach()
        {
            var mockRestService = new Mock<IRestService>();
            var mockOrgStore = new Mock<IDataStore<Organization>>();

            var fakeOrgs = new List<Organization>
            {
                new Organization { OrganizationID = Guid.NewGuid(), OrganizationName = "Org A" },
                new Organization { OrganizationID = Guid.NewGuid(), OrganizationName = "Org B" }
            };

            mockRestService.Setup(s => s.GetOrganizationsAsync()).ReturnsAsync(fakeOrgs);
            mockOrgStore.Setup(s => s.UpsertItemAsync(It.IsAny<Organization>())).ReturnsAsync(true);

            var sut = new OrganizationSyncService(mockRestService.Object, mockOrgStore.Object);
            await sut.SyncOrganizationsAsync();

            mockOrgStore.Verify(s => s.UpsertItemAsync(It.IsAny<Organization>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task SyncOrganizationsAsync_WhenApiThrows_PropagatesException()
        {
            var mockRestService = new Mock<IRestService>();
            var mockOrgStore = new Mock<IDataStore<Organization>>();

            mockRestService.Setup(s => s.GetOrganizationsAsync())
                           .ThrowsAsync(new HttpRequestException("Network error"));

            var sut = new OrganizationSyncService(mockRestService.Object, mockOrgStore.Object);

            bool exceptionThrown = false;
            try
            {
                await sut.SyncOrganizationsAsync();
            }
            catch (HttpRequestException)
            {
                exceptionThrown = true;
            }

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                exceptionThrown, "Expected HttpRequestException to be propagated.");
        }
    }
}