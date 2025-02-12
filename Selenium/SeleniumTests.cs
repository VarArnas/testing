using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium;

[TestFixture]
public class SeleniumTests
{
    private IWebDriver _testDriver;
    private WebDriverWait _wait;
    private string _email;
    private string _password;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        createAccount();
    }

    [SetUp]
    public void SetUp()
    {
        _testDriver = new ChromeDriver();
        _wait = new WebDriverWait(_testDriver, TimeSpan.FromSeconds(10));
        _testDriver.Manage().Window.Maximize();
        _testDriver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
    }

    [TearDown]
    public void TearDown()
    {
        _testDriver.Manage().Cookies.DeleteAllCookies();
        _testDriver.Quit();
        _testDriver.Dispose();
    }

    public void createAccount()
    {
        IWebDriver _driver = new ChromeDriver();
        WebDriverWait _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        _driver.Manage().Window.Maximize();

        _driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

        _driver.FindElement(By.XPath("//a[@href='/login'][@class='ico-login']")).Click();

        _driver.FindElement(By.XPath("//div[@class='new-wrapper register-block']//input[@type='button' and @value='Register']")).Click();

        _driver.FindElement(By.XPath("//div[@class='page registration-page']//input[@id='FirstName']")).SendKeys("Name");

        _driver.FindElement(By.XPath("//div[@class='page registration-page']//input[@id='LastName']")).SendKeys("Last NAme");

        string uniqueEmail = $"testuser{DateTime.Now.Ticks}@example.com";
        string password = "12345AAa.";

        _driver.FindElement(By.XPath("//div[@class='page registration-page']//input[@id='Email']")).SendKeys(uniqueEmail);

        _driver.FindElement(By.XPath("//div[@class='fieldset'][descendant::label[@for='Password']]//input[preceding-sibling::label[@for='Password']]")).SendKeys(password);

        _driver.FindElement(By.XPath("//div[@class='fieldset'][descendant::label[@for='Password']]//input[preceding-sibling::label[@for='ConfirmPassword']]")).SendKeys(password);

        _driver.FindElement(By.XPath("//div[@class='page-body']//input[@type='submit']")).Click();

        _driver.FindElement(By.XPath("//div[@class='page-body']//input[@type='button' and @value='Continue']")).Click();

        _email = uniqueEmail;
        _password = password;

        _driver.Quit();
    }

    [Test]
    public void lab4_test1()
    {

        List<string> products = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "../../../info/data1.txt"))
        .Select(line => line.Trim())
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .ToList();

        _testDriver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

        _testDriver.FindElement(By.XPath("//a[@href='/login'][@class='ico-login']")).Click();

        _testDriver.FindElement(By.XPath("//form[@action='/login'][@method='post']//input[@id='Email' and preceding-sibling::label[@for='Email']]")).SendKeys(_email);

        _testDriver.FindElement(By.XPath("//form[@action='/login'][@method='post']//input[@id='Password' and preceding-sibling::label[@for='Password']]")).SendKeys(_password);

        _testDriver.FindElement(By.XPath("//div[@class='buttons']/input[@type='submit' and @value='Log in']")).Click();

        _testDriver.FindElement(By.XPath("//a[@href='/digital-downloads']")).Click();

        foreach (var product in products)
        {
            _wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
                return !element.Displayed ? d.FindElement(By.XPath($"//div[@class='product-grid']//h2[descendant::a[text()='{product}']]//following-sibling::div[@class='add-info']//input[@type='button' and @value='Add to cart']")) : null;
            }).Click();
        }

        _testDriver.FindElement(By.XPath("//div[@class='header-links']//li[@id='topcartlink']/a[@href='/cart']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='terms-of-service']//input[@id='termsofservice']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='checkout-buttons']//button[@type='submit' and @id='checkout']")).Click();

        //form
        _testDriver.FindElement(By.XPath("//div[@class='edit-address']//select[preceding-sibling::label[@for='BillingNewAddress_CountryId']]//option[@value='86']")).Click();

        _testDriver.FindElement(By.XPath("//input[@id='BillingNewAddress_City' and @type='text']")).SendKeys("gaga");

        _testDriver.FindElement(By.XPath("//input[@id='BillingNewAddress_Address1' and @type='text']")).SendKeys("nana");

        _testDriver.FindElement(By.XPath("//input[@id='BillingNewAddress_ZipPostalCode' and @type='text']")).SendKeys("54638");

        _testDriver.FindElement(By.XPath("//input[@id='BillingNewAddress_PhoneNumber' and @type='text']")).SendKeys("+65454638542");

        _testDriver.FindElement(By.XPath("//input[@type='button' and @title='Continue']")).Click();

        //payment
        _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='billing-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button' and @onclick='PaymentMethod.save()']")) : null;
        }).Click();

        _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='payment-method-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button' and @onclick='PaymentInfo.save()']")) : null;
        }).Click();

        _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='payment-info-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button' and @onclick='ConfirmOrder.save()']")) : null;
        }).Click();

        string orderNr = _wait.Until(d =>
        {
            IWebElement? element = null;
            try
            {
                element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='confirm-order-please-wait']"));
            }
            catch (NoSuchElementException)
            {
                return d.FindElement(By.XPath("//div[@class='section order-completed']//ul[@class='details']/li[1]"));
            }
            return !element.Displayed ? d.FindElement(By.XPath("//div[@class='section order-completed']//ul[@class='details']/li[1]")) : null;
        }).Text.Trim();

        Assert.That(orderNr.Length, Is.GreaterThan(0), "There should be an order number for succesful order");
    }

    [Test]
    public void lab4_test2()
    {
        List<string> products = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "../../../info/data2.txt"))
        .Select(line => line.Trim())
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .ToList();

        _testDriver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

        _testDriver.FindElement(By.XPath("//a[@href='/login'][@class='ico-login']")).Click();

        _testDriver.FindElement(By.XPath("//form[@action='/login'][@method='post']//input[@id='Email' and preceding-sibling::label[@for='Email']]")).SendKeys(_email);

        _testDriver.FindElement(By.XPath("//form[@action='/login'][@method='post']//input[@id='Password' and preceding-sibling::label[@for='Password']]")).SendKeys(_password);

        _testDriver.FindElement(By.XPath("//div[@class='buttons']/input[@type='submit' and @value='Log in']")).Click();

        _testDriver.FindElement(By.XPath("//a[@href='/digital-downloads']")).Click();

        foreach (var product in products)
        {
            _wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
                return !element.Displayed ? d.FindElement(By.XPath($"//div[@class='product-grid']//h2[descendant::a[text()='{product}']]//following-sibling::div[@class='add-info']//input[@type='button' and @value='Add to cart']")) : null;
            }).Click();
        }

        _testDriver.FindElement(By.XPath("//div[@class='header-links']//li[@id='topcartlink']/a[@href='/cart']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='terms-of-service']//input[@id='termsofservice']")).Click();

        _testDriver.FindElement(By.XPath("//div[@class='checkout-buttons']//button[@type='submit' and @id='checkout']")).Click();

        //form
        _testDriver.FindElement(By.XPath("//input[@type='button' and @title='Continue']")).Click();

        //payment
        _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='billing-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button' and @onclick='PaymentMethod.save()']")) : null;
        }).Click();

        _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='payment-method-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button' and @onclick='PaymentInfo.save()']")) : null;
        }).Click();

        _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='payment-info-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button' and @onclick='ConfirmOrder.save()']")) : null;
        }).Click();

        string orderNr = _wait.Until(d =>
        {
            IWebElement? element = null;
            try
            {
                element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='confirm-order-please-wait']"));
            }
            catch (NoSuchElementException)
            {
                return d.FindElement(By.XPath("//div[@class='section order-completed']//ul[@class='details']/li[1]"));
            }
            return !element.Displayed ? d.FindElement(By.XPath("//div[@class='section order-completed']//ul[@class='details']/li[1]")) : null;
        }).Text.Trim();

        Assert.That(orderNr.Length, Is.GreaterThan(0), "There should be an order number for succesful order");
    }
}
