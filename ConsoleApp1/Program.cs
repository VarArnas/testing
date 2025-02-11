using ConsoleApp1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{

    static IWebDriver? driver = null; 
    static IWebDriver? testDriver = null;
    static void Main()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        BrowserSequences sequences = new BrowserSequences(driver, wait);

        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nShutting down...");

            driver?.Quit(); 
            testDriver?.Quit(); 

            e.Cancel = true; 
        };

        try
        {
            sequences.lab4_create_acc();
            driver.Quit();

            testDriver = new ChromeDriver();
            WebDriverWait testWait = new WebDriverWait(testDriver, TimeSpan.FromSeconds(10));
            testDriver.Manage().Window.Maximize();
            Tests tests = new Tests(testDriver, testWait);

            tests.lab4_test1();

            Console.ReadLine();
        }
        finally
        {
            driver?.Quit();
            testDriver?.Quit();
        }
    }
}