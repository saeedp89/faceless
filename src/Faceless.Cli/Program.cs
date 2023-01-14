using System.Collections.ObjectModel;
using Faceless.Domain;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;



StartScraping();
ScrapeSingleProduct();

List<string> ExtractGallery(ChromeDriver chromeDriver)
{
    var imageXPath = @"//*[@id=""productP1""]/div[2]";
    var imageDiv = chromeDriver
        .FindElement(By.XPath(imageXPath));
    imageDiv
        .FindElements(By.XPath("ul/li/button"))
        .FirstOrDefault()?.Click();
    // /html/body/div[3]/div/div/div/figure/figcaption/ul
    var fig = chromeDriver.FindElement(By.XPath("/html/body/div[3]/div/div/div/figure/figcaption"));
    var liTags = fig.FindElements(By.XPath("ul/li/button/img"));
    var list = liTags.Select(l => l.GetAttribute("src")).Where(src => !string.IsNullOrWhiteSpace(src)).ToList();
    return list;
}

void ScrapeSingleProduct()
{
    var url =
        "https://www.technolife.ir/product-5811/%D9%84%D9%BE-%D8%AA%D8%A7%D9%BE-16-%D8%A7%DB%8C%D9%86%DA%86%DB%8C-%D8%A7%DB%8C%D8%B3%D9%88%D8%B3-rog-zephyrus-m16-gu603zm-k8045";
    url =
        "https://www.technolife.ir/product-6744/%D9%84%D9%BE-%D8%AA%D8%A7%D9%BE-15.6-%D8%A7%DB%8C%D9%86%DA%86%DB%8C-%D9%84%D9%86%D9%88%D9%88-%D9%85%D8%AF%D9%84-ideapad-5-i5-8g-512g-";
    url =
        "https://www.technolife.ir/product-1793/%D9%84%D9%BE-%D8%AA%D8%A7%D9%BE-15-%D8%A7%DB%8C%D9%86%DA%86%DB%8C-%D8%A7%DB%8C%D8%B3%D9%88%D8%B3-%D9%85%D8%AF%D9%84-x%DB%B5%DB%B4%DB%B3ma";
    /* url =
         "https://www.technolife.ir/product-9469/-%D9%84%D9%BE-%D8%AA%D8%A7%D9%BE-13.3-%D8%A7%DB%8C%D9%86%DA%86%DB%8C-%D8%A7%D9%BE%D9%84-%D9%85%D8%AF%D9%84-macbook-pro-mneq3-2022-lla";*/
    using ChromeDriver driver = new ChromeDriver();
    driver.Navigate().GoToUrl(url);
    var attr = GetProductAttribute(driver);

    //var gallery = ExtractGallery(driver);
    // var localTitle = GetProductTitle(driver);
    //var p = GetProductPrice(driver);
}

(string,string) GetProductAttribute(IWebDriver driver)
{
    var dimensionsXPath = "//*[@id=\"accordion__panel-0\"]/li[1]/div[2]";
    var dim = driver.FindElement(
        By.XPath(dimensionsXPath)).Text;
    var weight = driver.FindElement(
        By.XPath("//*[@id=\"accordion__panel-0\"]/li[2]/div[2]")).Text;
    return (dim,weight);
}

string GetProductTitle(IWebDriver chromeDriver)
{
    var persiantitleXPath = "//*[@id=\"pdp_name\"]";
    var persianTitleElement = chromeDriver.FindElement(By.XPath(persiantitleXPath))
        .Text;
    return persianTitleElement;
}

string GetProductPrice(IWebDriver driver)
{
    var productInfoLeftSideXPath = "//*[@id=\"productP1\"]/div[3]";
    //*[@id="productP1"]/div[3]/div/span
    var price = driver.FindElement(By.XPath(productInfoLeftSideXPath));
    var nextDivSection = price.FindElements(By.TagName("div")).ToList();
    return nextDivSection.Any(x => x.GetAttribute("class").Contains("unavailable"))
        ? "0"
        : driver.FindElement(By.XPath("//*[@id=\"productP1\"]/div[3]/div[2]/div/h6/span[1]")).Text;
}

void StartScraping()
{
    var baseUrl = "https://technolife.ir/";
    var category = "تمامی-کامپیوترها-و-لپتاپها";
    var pageNumber = 1;
    var catProductUrl = GetCategoryProductsUrlByPageNumber(pageNumber);

    var productListTagId = "productsList";

    using IWebDriver driver = new ChromeDriver();

    driver.Navigate().GoToUrl(catProductUrl);

    while (true)
    {
        var result = ExtractAllProductsInPage(driver);
        Console.WriteLine(result);
        if (result.ShouldTerminate)
            break;
        // foreach result.Data should call db to save it there
        var nextPage = GetCategoryProductsUrlByPageNumber(pageNumber++);
        Console.WriteLine($"Next page: {nextPage}");
        driver.Navigate().GoToUrl(nextPage);
    }


    Console.WriteLine("Press Enter To Terminate...");
    Console.ReadLine();

    ExtractResult ExtractAllProductsInPage(IWebDriver driver)
    {
        var productBundleElements = FindAllValidProductBundleElements(driver);
        var result = ExtractResult.Default;
        if (!productBundleElements.Any())
            return result;

        Console.WriteLine($"Number of products: {productBundleElements.Count()}");
        var allProductsInPage = new List<Product>();
        foreach (var bundle in productBundleElements)
        {
            var productId = bundle.GetAttribute("id");
            var productUrl = $"{baseUrl}product-{productId}";
            try
            {
                using IWebDriver productDriver = new ChromeDriver();

                productDriver.Navigate().GoToUrl(productUrl);
                Product product = BuildProduct(productDriver);
                allProductsInPage.Add(product);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            // extract product data
        }

        if (allProductsInPage.Count == 0)
        {
            return result;
        }

        return new ExtractResult(false, allProductsInPage);
    }

    Product BuildProduct(IWebDriver webDriver)
    {
        //throw new NotImplementedException();
        return new Product("", 0.0m, 0.0m, 0.0m, 0.0m,
            "");
    }

    IEnumerable<IWebElement> FindAllValidProductBundleElements(IWebDriver webDriver)
        => webDriver.FindElement(By.Id("productsList"))
            .FindElements(By.XPath("ul/li")).Where(x =>
                x.GetAttribute("id") != null);

    string GetCategoryProductsUrlByPageNumber(int pageNumber)
    {
        return
            $"{baseUrl}product/list/164_163_130/{category}?page={pageNumber}";
    }
}
//*[@id="productsList"]/div/div/div/button[7]