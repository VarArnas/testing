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

        IWebElement giftcard = _driver.FindElement(By.XPath("//div[@class='product-grid']//div[@class='item-box'][.//span[@class='price actual-price'][number(normalize-space()) > 99]]//a"));
        giftcard.Click();

        IWebElement recipient = _driver.FindElement(By.XPath("//div[@class='giftcard']//input[@class='recipient-name']"));
        recipient.SendKeys("gaga");

        IWebElement yours = _driver.FindElement(By.XPath("//div[@class='giftcard']//input[@class='sender-name']"));
        yours.SendKeys("lala");

        IWebElement qty = _driver.FindElement(By.XPath("//div[@class='add-to-cart']//div[@class='add-to-cart-panel']//descendant::input[@type='text']"));
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

        IWebElement finalCart = _driver.FindElement(By.XPath("//div[@class='wishlist-content']/descendant::div[@class='common-buttons']/child::input[@name='addtocartbutton']"));
        finalCart.Click();

        IWebElement finalPrice = _driver.FindElement(By.XPath("//table[@class='cart-total']/descendant::td[@class='cart-total-right']/descendant::span[@class='product-price order-total']/child::strong"));
        if (finalPrice.Text.Trim() == "1002600.00")
        {
            Console.WriteLine("The price is correct");
        }
    }

    public void lab3_1()
    {
        _driver.Navigate().GoToUrl("https://demoqa.com/");

        IWebElement widget = _driver.FindElement(By.XPath("//div[@class='card mt-4 top-card'][descendant::h5[text()='Widgets']]"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", widget);
        widget.Click();

        IWebElement progressBar = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//ul[@class='menu-list']/child::li[@id='item-4' and child::span[text()='Progress Bar']]"));
            return element.Displayed && element.Enabled ? element : null;
        });
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", progressBar);
        progressBar.Click();

        IWebElement start = _driver.FindElement(By.XPath("//div[@id='progressBarContainer']/child::button[@id='startStopButton']"));
        start.Click();

        IWebElement reset = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//div[@id='progressBarContainer']/child::div[@id='progressBar']/child::div[@role='progressbar']"));
            return element.Text.Trim() == "100%" ? d.FindElement(By.XPath("//div[@id='progressBarContainer']/child::button[@id='resetButton']")) : null;
        });
        reset.Click();

        IWebElement progressNumber = _wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(By.XPath("//div[@id='progressBarContainer']/child::button[@id='startStopButton']"));
                return d.FindElement(By.XPath("//div[@id='progressBarContainer']/child::div[@id='progressBar']/child::div[@role='progressbar']"));
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });

        if (progressNumber.Text.Trim() == "0%")
        {
            Console.WriteLine("progress is at 0 percent");
        } 
    }

    public void lab3_2()
    {
        _driver.Navigate().GoToUrl("https://demoqa.com/");

        IWebElement elements = _driver.FindElement(By.XPath("//div[@class='card mt-4 top-card'][descendant::h5[text()='Elements']]"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", elements);
        elements.Click();

        IWebElement webTables = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//ul[@class='menu-list']/child::li[descendant::span[text()='Web Tables']]"));
            return element.Displayed && element.Enabled ? element : null;
        });
        webTables.Click();

        IWebElement addition = _driver.FindElement(By.XPath("//button[@id='addNewRecordButton']"));

        while(true)
        {

            addition.Click();

            IWebElement fName = _wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(By.XPath("//div[@id='firstName-wrapper']/descendant::input[@id='firstName']"));
                    return element;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
            fName.SendKeys("lala");
            IWebElement lName = _driver.FindElement(By.XPath("//div[@id='lastName-wrapper']/descendant::input[@id='lastName']"));
            lName.SendKeys("lala");
            IWebElement email = _driver.FindElement(By.XPath("//div[@id='userEmail-wrapper']/descendant::input[@id='userEmail']"));
            email.SendKeys("aasd@gmail.com");
            IWebElement age = _driver.FindElement(By.XPath("//div[@id='age-wrapper']/descendant::input[@id='age']"));
            age.SendKeys("559");
            IWebElement salary = _driver.FindElement(By.XPath("//div[@id='salary-wrapper']/descendant::input[@id='salary']"));
            salary.SendKeys("559");
            IWebElement department = _driver.FindElement(By.XPath("//div[@id='department-wrapper']/descendant::input[@id='department']"));
            department.SendKeys("lala");
            IWebElement submit = _driver.FindElement(By.XPath("//div[@class='modal-content'][descendant::div[text()='Registration Form']]//button[@id='submit']"));
            submit.Click();

            IWebElement checkIfNewRow = _driver.FindElement(By.XPath("//div[@class='-pagination']/div[@class='-next']/button"));
            if (checkIfNewRow.Enabled)
            {
                break;
            }
        }

        IWebElement next = _driver.FindElement(By.XPath("//div[@class='-pagination']/div[@class='-next']/button"));
        IWebElement random = _driver.FindElement(By.XPath("//div[@class='col-12 mt-4 col-md-6']"));
        random.Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", next);
        next.Click();

        var totalPages = Convert.ToInt32(_driver.FindElement(By.XPath("//div[@class='-pagination']//span[@class='-totalPages']")).Text.Trim());
        IWebElement delete = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//div[@class='-pagination']/div[@class='-next']/button"));
            return !element.Enabled ? d.FindElement(By.XPath("//div[@class='rt-tbody']/div[@class='rt-tr-group'][1]//div[@class='action-buttons']/span[@title='Delete']")) : null;
        });
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", delete);
        delete.Click();

        IWebElement pageInput = _driver.FindElement(By.XPath("//div[@class='-pagination']//input[@aria-label='jump to page']"));
        pageInput.Click();

        IWebElement nextClick = _driver.FindElement(By.XPath("//div[@class='-pagination']/div[@class='-next']/button"));
        nextClick.Click();

        var totalPagesAfter = Convert.ToInt32(_driver.FindElement(By.XPath("//div[@class='-pagination']//span[@class='-totalPages']")).Text.Trim());
        if(totalPages - totalPagesAfter == 1)
        {
            Console.WriteLine("Total pages decreased by 1");
        }

        var currentPage = _driver.FindElement(By.XPath("//div[@class='-pagination']//input[@aria-label='jump to page']")).GetAttribute("value");
        if (currentPage == "1")
        {
            Console.WriteLine("Went back to the first page");
        }
    }
}
