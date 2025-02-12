using ConsoleApp1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{

    static IWebDriver? driver = null; 

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

            e.Cancel = true; 
        };

        try
        {
            //sequences.lab2();
            //sequences.lab3_1();
            sequences.lab3_2();

            Console.ReadLine();
        }
        finally
        {
            driver?.Quit();
        }
    }
}