using ConsoleApp1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize(); 
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        BrowserSequences sequences = new BrowserSequences(driver, wait);

        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nShutting down");
            driver.Quit(); 
            e.Cancel = true; 
        };

        try
        {
            //sequences.lab2();
            //sequences.lab3_1();
            //sequences.lab3_2();
            sequences.lab4_create_acc();
            driver.Quit();

            Thread.Sleep(1000);

            IWebDriver testDriver = new ChromeDriver();
            WebDriverWait testWait = new WebDriverWait(testDriver, TimeSpan.FromSeconds(10));
            testDriver.Manage().Window.Maximize();
            Tests tests = new Tests(testDriver, testWait);

            tests.lab4_test1();

            Console.ReadLine();
        }
        finally
        {
            driver.Quit();
        }
    }
}