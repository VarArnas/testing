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
            //sequences.lab2();
            //sequences.lab3_1();
            sequences.lab3_2();

            //testTests(sequences);
            Console.ReadLine();
        }
        finally
        {
            driver?.Quit();
            testDriver?.Quit();
        }
    }

    private static void testTests(BrowserSequences sequences)
    {
        sequences.lab4_create_acc();
        driver.Quit();

        testDriver = new ChromeDriver();
        WebDriverWait testWait = new WebDriverWait(testDriver, TimeSpan.FromSeconds(10));
        testDriver.Manage().Window.Maximize();
        Tests tests = new Tests(testDriver, testWait);


        tests.lab4_test1();
        testDriver.Manage().Cookies.DeleteAllCookies();
        testDriver.Quit();

        tests._driver = new ChromeDriver();
        tests._driver.Manage().Window.Maximize();
        tests._wait = new WebDriverWait(tests._driver, TimeSpan.FromSeconds(10));
        tests.lab4_test2();
    }
}