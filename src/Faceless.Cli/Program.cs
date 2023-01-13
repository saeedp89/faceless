using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

var baseUrl = "https://technolife.ir/";
var url =
    $"{baseUrl}product/list/164_163_130/تمامی-کامپیوترها-و-لپتاپها";
using IWebDriver driver = new ChromeDriver();
driver.Navigate().GoToUrl(url);
var element = driver.FindElement(By.Id("productsList"));
var products = element.FindElements(By.XPath("ul/li"));
foreach (var item in products)
{
    var productId = item?.GetAttribute("id");
    if (!string.IsNullOrWhiteSpace(productId))
    {
        var productUrl = $"{baseUrl}product-{productId}";
        using IWebDriver productDriver = new ChromeDriver();
        productDriver.Navigate().GoToUrl(productUrl);

    }
}

Console.Out.WriteLine("Press Enter To Terminate...");
Console.In.ReadLine();