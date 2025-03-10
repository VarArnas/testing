using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ConsoleApp1;
using System.Text.RegularExpressions;

namespace Selenium;

[TestFixture]
public class SeleniumTests2
{
    private IWebDriver _testDriver;
    private WebDriverWait _testWait;
    private string _email;
    private string _password;
    private string? newEmail;
    private string? newPassword;
    private record Votes(int ExcellentNum, int GoodNum, int PoorNum, int VeryBadNum, int Total);

    [SetUp]
    public void SetUp()
    {
        List<string> creds = GetCredentials();
        _email = creds[0];
        _password = creds[1];
        _testDriver = new ChromeDriver();
        _testWait = new WebDriverWait(_testDriver, TimeSpan.FromSeconds(10));
        _testDriver.Manage().Window.Maximize();
        _testDriver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
    }


    [TearDown]
    public void TearDown()
    {
        _testDriver.Quit();
        _testDriver.Dispose();
    }

    private static List<string> GetCredentials()
    {
        List<string> products = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "../../../info/credentials.txt"))
       .Select(line => line.Trim())
       .Where(line => !string.IsNullOrWhiteSpace(line))
       .ToList();
        return products;
    }

    private void CreateAccount()
    {
        _testDriver.FindElement(By.XPath("//a[@href='/login'][@class='ico-login']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='new-wrapper register-block']//input[@type='button' and @value='Register']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='page registration-page']//input[@id='FirstName']")).SendKeys("Name");

        _testDriver.FindElement(By.XPath("//div[@class='page registration-page']//input[@id='LastName']")).SendKeys("Last NAme");
            
        string uniqueEmail = $"testuser{DateTime.Now.Ticks}@example.com";
        string password = "12345AAa.";

        _testDriver.FindElement(By.XPath("//div[@class='page registration-page']//input[@id='Email']")).SendKeys(uniqueEmail);

        _testDriver.FindElement(By.XPath("//div[@class='fieldset'][descendant::label[@for='Password']]//input[preceding-sibling::label[@for='Password']]")).SendKeys(password);

        _testDriver.FindElement(By.XPath("//div[@class='fieldset'][descendant::label[@for='Password']]//input[preceding-sibling::label[@for='ConfirmPassword']]")).SendKeys(password);

        _testDriver.FindElement(By.XPath("//div[@class='page-body']//input[@type='submit']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='page-body']//input[@type='button' and @value='Continue']")).Click();

        newEmail = uniqueEmail;
        newPassword = password;

        Console.WriteLine("worked!!!");
    }

    private Votes TakeAndReturnVotes(IWebDriver driver)
    {
        var votes = driver.FindElements(By.XPath("//ul[@class='poll-results']/li"));
        var total = driver.FindElement(By.XPath("//span[@class='poll-total-votes']"));
        Regex regex = new Regex(@"(\d+) vote\(s\)");
        var organizedVotes = new Votes(ExcellentNum: int.Parse(regex.Match(votes[0].Text).Groups[1].Value),
                                        GoodNum: int.Parse(regex.Match(votes[1].Text).Groups[1].Value),
                                        PoorNum: int.Parse(regex.Match(votes[2].Text).Groups[1].Value),
                                        VeryBadNum: int.Parse(regex.Match(votes[3].Text).Groups[1].Value),
                                        Total: int.Parse(regex.Match(total.Text).Groups[1].Value));

        return organizedVotes;
    } 

    private Votes TakeCurrentVotes()
    {
        IWebDriver _driver = new ChromeDriver();
        WebDriverWait _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        _driver.Manage().Window.Maximize();
        _driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");


        _driver.FindElement(By.XPath("//a[@href='/login']")).Click();
        _driver.FindElement(By.XPath("//input[@id='Email' and @class='email']")).SendKeys(_email);
        _driver.FindElement(By.XPath("//input[@id='Password' and @class='password']")).SendKeys(_password);
        _driver.FindElement(By.XPath("//input[@type='submit' and @value='Log in']")).Click();
        string emailCheck = _wait.Until(d =>
        {
            string? currentEmail;
            try
            {
                currentEmail = d.FindElement(By.XPath("//a[@href='/customer/info' and @class='account']")).Text;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            return currentEmail;
        });

        Assert.That(emailCheck, Is.EqualTo(_email), "The emails aren't the same, bad log in!!");

        var votes = TakeAndReturnVotes(_driver);
        Console.WriteLine("worked22!!!");

        _driver.Quit();
        _driver.Dispose();
        return votes;
    }

    [Test]
    public void TestVoting()
    {
        var votes = TakeCurrentVotes();
        Assert.Multiple(() =>
        {
            Assert.That(votes.ExcellentNum, Is.GreaterThan(0), "The Excellent number was not initialized correctly!");
            Assert.That(votes.GoodNum, Is.GreaterThan(0), "The Good number was not initialized correctly!");
            Assert.That(votes.PoorNum, Is.GreaterThan(0), "The Poor number was not initialized correctly!");
            Assert.That(votes.VeryBadNum, Is.GreaterThan(0), "The Very Bad number was not initialized correctly!");
            Assert.That(votes.Total, Is.GreaterThan(0), "The Total number was not initialized correctly!");
        });

        CreateAccount();
        Assert.Multiple(() =>
        {
            Assert.That(newEmail, Is.Not.Null, "The account creation was not succesful!!");
            Assert.That(newPassword, Is.Not.Null, "The account creation was not succesful!!");
        });

        string emailCheck2 = _testWait.Until(d =>
        {
            string? currentEmail;
            try
            {
                currentEmail = d.FindElement(By.XPath("//a[@href='/customer/info' and @class='account']")).Text;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            return currentEmail;
        });

        Assert.That(emailCheck2, Is.EqualTo(newEmail), "The emails aren't the same, account creation not succesful!!");

        _testDriver.FindElement(By.XPath("//input[@id='pollanswers-1' and @type='radio']")).Click();
        _testDriver.FindElement(By.XPath("//input[@type='button' and @value='Vote']")).Click();

        _testWait.Until(d =>
        {
            IWebElement element;
            try
            {
                element = d.FindElement(By.XPath("//span[@id='poll-voting-progress-1']"));
            }
            catch (NoSuchElementException)
            {
                return d.FindElement(By.XPath("//div[@class='master-wrapper-main']"));
            }
            return !element.Displayed ? element : null;
        });

        var newVotes = TakeAndReturnVotes(_testDriver);

        Assert.Multiple(() =>
        {
            Assert.That(newVotes.ExcellentNum, Is.GreaterThan(0), "The Excellent number was not initialized correctly!");
            Assert.That(newVotes.GoodNum, Is.GreaterThan(0), "The Good number was not initialized correctly!");
            Assert.That(newVotes.PoorNum, Is.GreaterThan(0), "The Poor number was not initialized correctly!");
            Assert.That(newVotes.VeryBadNum, Is.GreaterThan(0), "The Very Bad number was not initialized correctly!");
            Assert.That(newVotes.Total, Is.GreaterThan(0), "The Total number was not initialized correctly!");
        });

        Assert.Multiple(() =>
        {
            Assert.That(votes.ExcellentNum, Is.Not.EqualTo(newVotes.ExcellentNum), "The excellent number did not change!!!");
            Assert.That(votes.Total, Is.Not.EqualTo(newVotes.Total), "The total number did not change!!!");
            Assert.That(votes.GoodNum, Is.EqualTo(newVotes.GoodNum), "The good number did change!!!");
            Assert.That(votes.PoorNum, Is.EqualTo(newVotes.PoorNum), "The poor number did change!!!");
            Assert.That(votes.VeryBadNum, Is.EqualTo(newVotes.VeryBadNum), "The very bad number did change!!!");
        });
    }
}
