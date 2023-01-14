using Faceless.Domain.Entities;
using Faceless.Infrastructure;
using Faceless.Repositories.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Faceless.Application;

public class ProductCrawler : IProductCrawler
{
    private readonly IProductRepository _productRepository;
    private readonly IProductPicturesRepository _galleryRepository;
    private readonly IPictureRepository _pictureRepository;

    public ProductCrawler(IProductRepository productRepository, IProductPicturesRepository galleryRepository, IPictureRepository pictureRepository)
    {
        _productRepository = productRepository;
        _galleryRepository = galleryRepository;
        _pictureRepository = pictureRepository;
    }

    public void StartCrawling()
    {
        List<string> listOfProductUrlsToCrawl = new();
        var pageNumber = 0;
        while (true)
        {
            pageNumber++;
            var categoryProductsPaginatedUrl =
                GetCategoryProductsUrlByPageNumber(pageNumber);

            var driver = NavigateToCategoryProductsPage(categoryProductsPaginatedUrl);


            var allValidUrls =
                ExtractAllValidProductUrlsFromPage(driver);

            if (!allValidUrls.Any())
            {
                driver.Quit();
                driver.Dispose();
                break;
            }

            listOfProductUrlsToCrawl.AddRange(allValidUrls);
            driver.Quit();
            driver.Dispose();
        }

        foreach (var productUrl in listOfProductUrlsToCrawl)
        {
            IWebDriver driver = NavigateToProductPage(productUrl);
            var title = GetProductTitle(driver);
            var price = GetProductPrice(driver);
            var size = GetProductSize(driver);
            var weight = GetProductWeight(driver);
            var gallery = ExtractGallery(driver);
            var id = productUrl.Split('-').Last().Trim();
            var product = new Product(title, price, size, weight, id);
            _productRepository.AddAsync(product).GetAwaiter().GetResult();
            var pics=gallery.Select(e=>new Pic)
        }
    }

    private IWebDriver NavigateToProductPage(string productUrl)
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments(new[] { "headless" });
        var driver = new ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl(productUrl);
        return driver;
    }

    private IEnumerable<string> ExtractAllValidProductUrlsFromPage(IWebDriver driver)
    {
        var allValidProductIds =
            FindAllValidProductIds(driver);

        if (!allValidProductIds.Any())
            return Enumerable.Empty<string>();

        var validUrls = allValidProductIds.Select(x => $"{DestinationWebSite.BaseUrl}product-{x}");

        return validUrls.ToList();
    }

    private IWebDriver NavigateToCategoryProductsPage(string url)
    {
        var driver = new ChromeDriver();
        driver.Navigate().GoToUrl(url);
        return driver;
    }

    IEnumerable<string> ExtractGallery(IWebDriver browser)
    {
        var imageXPath = @"//*[@id=""productP1""]/div[2]";
        var imageDiv = browser
            .FindElement(By.XPath(imageXPath));
        imageDiv
            .FindElements(By.XPath("ul/li/button"))
            .FirstOrDefault()?.Click();
        var fig = browser
            .FindElement(By
                .XPath("/html/body/div[3]/div/div/div/figure/figcaption"));
        var liTags = fig.FindElements(By.XPath("ul/li/button/img"));
        var list = liTags.Select(l => l.GetAttribute("src")).Where(src => !string.IsNullOrWhiteSpace(src)).ToList();
        return list;
    }


    string GetProductSize(IWebDriver driver)
    {
        var dimensionsXPath = "//*[@id=\"accordion__panel-0\"]/li[1]/div[2]";
        var dim = driver.FindElement(
            By.XPath(dimensionsXPath)).Text;
        return dim;
    }

    string GetProductWeight(IWebDriver driver)
    {
        var weightXPath = "//*[@id=\"accordion__panel-0\"]/li[2]/div[2]";
        var weight = driver.FindElement(
            By.XPath(weightXPath)).Text;
        return weight;
    }

    string GetProductTitle(IWebDriver webDriver)
    {
        var prodTitleXPath = "//*[@id=\"pdp_name\"]";
        var persianTitleElement = webDriver
            .FindElement(By.XPath(prodTitleXPath))
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




    string GetCategoryProductsUrlByPageNumber(int pageNumber)
    {
        return
            $"{DestinationWebSite.BaseUrl}product/list/164_163_130/{DestinationWebSite.Category}?page={pageNumber}";
    }


    IEnumerable<string> FindAllValidProductIds(IWebDriver webDriver)
        => webDriver
            .FindElement(By.Id("productsList"))
            .FindElements(By.XPath("ul/li"))
            .Where(x =>
                x.GetAttribute("id") != null)
            .Select(x => x.GetAttribute("id"));
}