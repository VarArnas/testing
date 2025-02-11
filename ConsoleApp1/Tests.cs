using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1;

public class Tests
{
    IWebDriver _driver;
    WebDriverWait _wait;

    public Tests(IWebDriver driver, WebDriverWait wait)
    {
        _driver = driver;
        _wait = wait;
    }

    public void lab4_test1()
    {
        StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "info/login.txt"));
        string email = sr.ReadLine()!;
        string password = sr.ReadLine()!;

        List<string> products = new List<string>();
        var linesRead = File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "info/data1.txt"));
        foreach (var line in linesRead)
        {
            products.Add(line.Trim());
        }

        _driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

        _driver.FindElement(By.XPath("//a[@href='/login'][@class='ico-login']")).Click();

        _driver.FindElement(By.XPath("//form[@action='/login'][@method='post']//input[@id='Email' and preceding-sibling::label[@for='Email']]")).SendKeys(email);

        _driver.FindElement(By.XPath("//form[@action='/login'][@method='post']//input[@id='Password' and preceding-sibling::label[@for='Password']]")).SendKeys(password);

        _driver.FindElement(By.XPath("//div[@class='buttons']/input[@type='submit' and @value='Log in']")).Click();

        _driver.FindElement(By.XPath("//a[@href='/digital-downloads']")).Click();

        foreach (var product in products)
        {
            _wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//div[@class='master-wrapper-content']/div[@class='ajax-loading-block-window']"));
                return !element.Displayed ? d.FindElement(By.XPath($"//div[@class='product-grid']//h2[descendant::a[text()='{product}']]//following-sibling::div[@class='add-info']//input[@type='button' and @value='Add to cart']")) : null;
            }).Click();
        }

        _driver.FindElement(By.XPath("//div[@class='header-links']//li[@id='topcartlink']/a[@href='/cart']")).Click();

        _driver.FindElement(By.XPath("//div[@class='terms-of-service']//input[@id='termsofservice']")).Click();

        _driver.FindElement(By.XPath("//div[@class='checkout-buttons']//button[@type='submit' and @id='checkout']")).Click();

        //form
        _driver.FindElement(By.XPath("//div[@class='edit-address']//select[preceding-sibling::label[@for='BillingNewAddress_CountryId']]//option[@value='86']")).Click();

        _driver.FindElement(By.XPath("//input[@id='BillingNewAddress_City' and @type='text']")).SendKeys("gaga");

        _driver.FindElement(By.XPath("//input[@id='BillingNewAddress_Address1' and @type='text']")).SendKeys("nana");

        _driver.FindElement(By.XPath("//input[@id='BillingNewAddress_ZipPostalCode' and @type='text']")).SendKeys("54638");

        _driver.FindElement(By.XPath("//input[@id='BillingNewAddress_PhoneNumber' and @type='text']")).SendKeys("+65454638542");

        _driver.FindElement(By.XPath("//input[@type='button' and @title='Continue']")).Click();

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

        Thread.Sleep(1000);
        string orderNr = _wait.Until(d =>
        {
            var element = d.FindElement(By.XPath("//span[@class='please-wait' and @id='confirm-order-please-wait']"));
            return !element.Displayed ? d.FindElement(By.XPath("//div[@class='section order-completed']//ul[@class='details']/li[1]")) : null;
        }).Text.Trim();

        Console.WriteLine("afa");


        if (orderNr.Length > 0)
        {
            Console.WriteLine("The purchase was succesful!!");
        }
    }
}
