using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsoleApp1;

public sealed class BrowserSequences
{
    public readonly IWebDriver _driver;
    public readonly WebDriverWait _wait;

    public BrowserSequences(IWebDriver driver, WebDriverWait wait) 
    {
        _driver = driver;
        _wait = wait;
    }

    public void lab2()
    {
        _driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

        IWebElement loginButton = _driver.FindElement(By.XPath("//a[@href='/gift-cards']"));
        loginButton.Click();

        IWebElement giftcard = _driver.FindElement(By.XPath("//div[@class='details'][descendant::span[@class='price actual-price'][number(normalize-space()) > 99]]//a"));
        giftcard.Click();

        IWebElement recipient = _driver.FindElement(By.XPath("//div[@class='giftcard']//input[@class='recipient-name']"));
        recipient.SendKeys("gaga");

        IWebElement yours = _driver.FindElement(By.XPath("//div[@class='giftcard']//input[@class='sender-name']"));
        yours.SendKeys("lala");

        IWebElement qty = _driver.FindElement(By.XPath("//div[@class='add-to-cart']//div[@class='add-to-cart-panel']/descendant::input[@type='text']"));
        qty.Clear();
        qty.SendKeys("5000");

        IWebElement cart = _driver.FindElement(By.XPath("//input[@type='button'][@id='add-to-cart-button-4']"));
        cart.Click();

        IWebElement wish = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
            return !element.Displayed ? d.FindElement(By.XPath("//input[@type='button'][@id='add-to-wishlist-button-4']")) : null;
        });
        wish.Click();

        IWebElement jewelry = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//div[@id='bar-notification']"));
            return !element.Displayed ? d.FindElement(By.XPath("//a[@href='/jewelry']")) : null;
        });
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", jewelry);
        jewelry.Click();

        IWebElement createJewelry = _driver.FindElement(By.XPath("//a[@href='/create-it-yourself-jewelry']"));
        createJewelry.Click();

        IWebElement material = _driver.FindElement(By.XPath("//select[@id='product_attribute_71_9_15']/option[@value='47']"));
        material.Click();

        IWebElement length = _driver.FindElement(By.XPath("//input[@id='product_attribute_71_10_16']"));
        length.SendKeys("80");

        IWebElement pendant = _driver.FindElement(By.XPath("//input[@id='product_attribute_71_11_17_50' and ./following-sibling::label[text()='Star ']]"));
        pendant.Click();

        IWebElement jewelryQty = _driver.FindElement(By.XPath("//div[@class='add-to-cart-panel']/child::input[@type='text']"));
        jewelryQty.Clear();
        jewelryQty.SendKeys("26");

        IWebElement jewelryCart = _driver.FindElement(By.XPath("//div[@class='add-to-cart-panel']/child::input[@type='button'][1]"));
        jewelryCart.Click();

        IWebElement jewelryWish = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
            return !element.Displayed ? d.FindElement(By.XPath("//div[@class='add-to-cart-panel']/child::input[@type='button'][2]")) : null;
        });
        jewelryWish.Click();

        IWebElement wishlist = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
            return !element.Displayed ? d.FindElement(By.XPath("//ul/descendant::a[@href='/wishlist'][@class='ico-wishlist']")) : null;
        });
        wishlist.Click();

        IReadOnlyCollection<IWebElement> addingToCart = _driver.FindElements(By.XPath("//table[@class='cart']/descendant::tr/descendant::input[@type='checkbox'][@name='addtocart']"));
        foreach (var element in addingToCart)
        {
            element.Click();
        }

        IWebElement finalCart = _driver.FindElement(By.XPath("//div[@class='wishlist-content']/descendant::div[@class='common-buttons']/input[@name='addtocartbutton']"));
        finalCart.Click();

        IWebElement finalPrice = _driver.FindElement(By.XPath("//table[@class='cart-total']/descendant::td[@class='cart-total-right']/descendant::span[@class='product-price order-total']/child::strong"));
        if (finalPrice.Text.Trim() == "1002600.00")
        {
            Console.WriteLine("The price is correct");
        }
    }
}
