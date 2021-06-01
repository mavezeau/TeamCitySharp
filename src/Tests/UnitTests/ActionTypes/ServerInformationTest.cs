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
            var teamcity = new TeamCityClient("build.mvstelecom.ru");
            teamcity.ConnectWithAccessToken("eyJ0eXAiOiAiVENWMiJ9.V0R5T1k5R0tPTklESUctNndDQjhkOVFkYzVn.OTczMGJhZGItMzVkYy00Y2QyLThmYWUtODg1NTk0N2RlYWFl");

            var p = teamcity.Projects.ById("ExtraCheckTest");

            //p.BuildTypes.BuildType.Where(x=>x.)

            var aa = teamcity.Tests.ByProjectLocator(ProjectLocator.WithId("ExtraCheckTest"));
            var cc = aa.TestOccurrence.Select(x => x.BuildName).ToList();
            
            var bb = aa.TestOccurrence.Select(x => x.Build.BuildTypeId).ToList();

            var buildName = "ExtraCheckTest_Iridium360ExtraCheckTest";

            ///Название билда
            var name = buildName.Split('_').ElementAtOrDefault(1);

            var builds = teamcity.Projects.ById("ExtraCheckTest").BuildTypes.BuildType;
            var tests = teamcity.Tests.ByProjectLocator(ProjectLocator.WithId("ExtraCheckTest"));

            var targetTests = tests.TestOccurrence.Where(x => x.BuildName == name).ToList();

            var targetBuild = teamcity.Builds.ByBuildLocator(BuildLocator.WithNumber(targetTests[0].Build.Number));
            var prevBuild = teamcity.Builds.ByBuildLocator(BuildLocator.WithNumber(targetTests[0].Build.Number));

            var groups = new Dictionary<string, List<TestOccurrence>>();

            foreach (var build in builds)
            {
                groups.Add(build.BuildName, tests.TestOccurrence.Where(x => x.BuildName == build.BuildName).ToList());
                //var aaa = teamcity.Builds.ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(build.Id), maxResults: 5));
            }

        }
    }
}