using ConsoleApp1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        BrowserSequences sequences = new BrowserSequences(driver, wait);

        try
        {
            //sequences.lab2();
            //sequences.lab3_1();
            //sequences.lab3_2();


            Console.ReadLine();
        }
        finally
        {
            driver.Quit();
        }
    }
}