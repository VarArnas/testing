using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        try
        {
            IWebElement loginButton = driver.FindElement(By.XPath("//a[@href='/gift-cards']"));
            loginButton.Click();

            IWebElement giftcard = driver.FindElement(By.XPath("//div[@class='product-grid']//div[@class='item-box'][.//span[@class='price actual-price'][number(normalize-space()) > 99]]//a"));
            giftcard.Click();

            IWebElement recipient = driver.FindElement(By.XPath("//div[@class='giftcard']//input[@class='recipient-name']"));
            recipient.SendKeys("gaga");

            IWebElement yours = driver.FindElement(By.XPath("//div[@class='giftcard']//input[@class='sender-name']"));   
            yours.SendKeys("lala");

            IWebElement qty = driver.FindElement(By.XPath("//div[@class='add-to-cart']//div[@class='add-to-cart-panel']//descendant::input[@type='text']"));
            qty.Clear();
            qty.SendKeys("5000");


            IWebElement cart = driver.FindElement(By.XPath("//input[@type='button'][@id='add-to-cart-button-4']"));
            cart.Click();

            IWebElement wish = wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
                return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button'][@id='add-to-wishlist-button-4']")) : null;
            });
            wish.Click();


            Console.ReadLine();
        }
        finally
        {
            driver.Quit();
        }
    }
}