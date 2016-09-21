using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpliftTests
{
    [TestFixture]
    class BackupControllerTests
    {

        [SetUp]
        public void SetUp()
        {
            /*
            _backupArchiveStoreMock = new Mock<IBackupArchiveMap>();
            _wepAppSettingsMock = new Mock<IConfiguration>();
            _coreServiceMock = new Mock<ICoreService>();

            var testItem = new LastBackupItem { Id = "MyItem", ConfigHash = "2FD4E1C67A2D28FCED849EE1BB76E7391B93EB12", LastRestoreTime = DateTime.Now };
            _backupArchiveStoreMock.SetupGet(p => p.LastCreatedOrRestored)
                .Returns(() => testItem);

            var backupArchiveManager = new BackupArchiveManager(_backupArchiveStoreMock.Object, _wepAppSettingsMock.Object, _coreServiceMock.Object);

            _objectUnderTest = new BackupController(backupArchiveManager);

            _wepAppSettingsMock.SetupGet(p => p["ASAWebApp.ZMQRepService"])
                .Returns(() => "tcp://127.0.0.1:9798");
             * */
        }

        /// <summary>
        /// Test of get for all BackupArchive meta data.
        /// </summary>
        [Test]
        public void Test_Something()
        {
            /*
            // Arrange
            var dataMocks = GenerateTestData(new string[] { "Happy", "Brown", "Cow", "In", "Lush", "Green", "Field" }, 3);
            var dataMockObjects = dataMocks.Select(x => x.Object);

            _backupArchiveStoreMock.Setup(p => p.GetEnumerator())
                .Returns(dataMockObjects.GetEnumerator());

            // setup the controller's internal state
            ApiControllerHelper.SetupApiControllerEnvironment(_objectUnderTest, HttpMethod.Get, "http://localhost/api/Backup");

            // Act
            IHttpActionResult actionResult = _objectUnderTest.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<List<BackupMetaDTO>>;

            // Assert
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
            Assert.That(contentResult.Content.Count, Is.EqualTo(7));
            Assert.That(contentResult.Content[0].Id, Is.EqualTo("Happy"));
            Assert.That(contentResult.Content.Where(p => p.Master == true).Count(), Is.EqualTo(3));
             * */
        }
    }
}