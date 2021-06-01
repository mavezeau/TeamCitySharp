using TeamCitySharp.DomainEntities;

namespace TeamCitySharp.ActionTypes
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using TeamCitySharp.Connection;
    using TeamCitySharp.Locators;

    [TestFixture]
    public class ServerInformationTest
    {
        private ServerInformation testee;
        private ITeamCityCaller teamCityCaller;

        [SetUp]
        public void SetUp()
        {
            this.teamCityCaller = A.Fake<ITeamCityCaller>();
            this.testee = new ServerInformation(this.teamCityCaller);
        }

        [TestCase(true, true, true, true)]
        [TestCase(false, false, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, true)]
        public void CreatesBackupWithSelectedParts(bool includeBuildLogs, bool includeConfigurations, bool includeDatabase,
                                                   bool includePersonalChanges)
        {
            const string Filename = "Filename";
            var backupOptions = new BackupOptions
            {
                Filename = Filename,
                IncludeBuildLogs = includeBuildLogs,
                IncludeConfigurations = includeConfigurations,
                IncludeDatabase = includeDatabase,
                IncludePersonalChanges = includePersonalChanges
            };

            this.testee.TriggerServerInstanceBackup(backupOptions);

            A.CallTo(() => this.teamCityCaller.StartBackup(string.Concat(
              "/server/backup?fileName=",
              Filename,
              "&includeBuildLogs=" + includeBuildLogs,
              "&includeConfigs=" + includeConfigurations,
              "&includeDatabase=" + includeDatabase,
              "&includePersonalChanges=" + includePersonalChanges)))
             .MustHaveHappened();
        }

        [Test]
        public void GetsBackupStatus()
        {
            const string Status = "Idle";

            A.CallTo(() => this.teamCityCaller.GetRaw("/server/backup")).Returns(Status);

            string status = this.testee.GetBackupStatus();

            status.Should().Be(Status);
        }


        [Test]
        public void Test()
        {
            var projectName = "ExtraCheckTest";
            var api = new TeamCityClient("build.mvstelecom.ru");
            api.ConnectWithAccessToken("eyJ0eXAiOiAiVENWMiJ9.V0R5T1k5R0tPTklESUctNndDQjhkOVFkYzVn.OTczMGJhZGItMzVkYy00Y2QyLThmYWUtODg1NTk0N2RlYWFl");

            var project = api.Projects.ById(projectName);
            var tests = api.Tests.ByProjectLocator(ProjectLocator.WithId(projectName));

            var disabled = project.BuildTypes.BuildType
                .Where(x => x.Name.StartsWith("[disabled]"))
                .Select(x => x.Id)
                .ToList();

            var actualTests = tests.TestOccurrence
                .Where(x => !disabled.Contains(x.Build.BuildTypeId))
                .ToList();
        }
    }
}