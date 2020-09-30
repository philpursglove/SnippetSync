using NUnit.Framework;
using System;
using Flurl.Http.Testing;

namespace SnippetSyncer.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        HttpTest _httpTest;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
        }

        [TearDown]
        public void Teardown()
        {
            _httpTest.Dispose();
        }

        [Test]
        public void Get_Repo_Updated_Date_From_API()
        {
            _httpTest.RespondWithJson(new { updated_at = "2020-05-07T12:00Z" });

            Repository repo = new Repository("https://github.com/testuser/TestRepoName");

            Assert.That(repo.LastUpdated, Is.EqualTo(new DateTime(2020, 5, 7, 12, 0, 0)));
        }
    }
}