using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Allure.Xunit;
using Allure.Xunit.Attributes;

namespace LoginAutomation
{
    [AllureSuite("Login")]
    [AllureFeature("Authentication")]
    public class LoginTests
    {
        public static IEnumerable<object[]> GetTestData()
        {

            using (var reader = new StreamReader("LoginTestData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var r in csv.GetRecords<TestData>().ToList())
                {
                    yield return new object[] { r.Username, r.Password, r.Expected };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestData))]

    
        [AllureStory("Login with CSV credentials")]
        [AllureTag("selenium", "csv")]
        [MemberData(nameof(GetTestData))]
        public void Login_WithCredentials_ShowsExpectedMessage(string username, string password, string expected)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                string path = Path.Combine(AppContext.BaseDirectory, "login.html");
                driver.Navigate().GoToUrl(new Uri(path).AbsoluteUri);

                driver.FindElement(By.Id("username")).SendKeys(username);
                driver.FindElement(By.Id("password")).SendKeys(password);
                driver.FindElement(By.Id("loginButton")).Click();

                var shot = ((ITakesScreenshot)driver).GetScreenshot();
                shot.SaveAsFile($"login_{username}.png");

                string message = driver.FindElement(By.Id("message")).Text;
                Assert.Equal(expected, message);
            }
        }

        public class TestData
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Expected { get; set; }
        }
    }
}