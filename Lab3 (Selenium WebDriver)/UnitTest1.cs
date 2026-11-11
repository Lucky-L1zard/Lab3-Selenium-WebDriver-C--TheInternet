namespace Lab3__Selenium_WebDriver_;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

public class Tests
{
    ChromeDriver driver;
    private const string homeUrl = "https://the-internet.herokuapp.com/";
    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    private IAlert WaitForAlert(int timeoutSeconds = 6)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        return wait.Until(d =>
        {
            try
            {
                return d.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        });
    }

    [Test]
    public void ForgotPasswordTest()
    {
        const string FPElementURL = homeUrl + "forgot_password";
        const string testEmail = "petrokalushniy@337.com";
        const string successAnswer = "Your e-mail's been sent!";
        const string redirectUrlAfterCorrectAnswer = homeUrl + "email_sent";

        driver.Navigate().GoToUrl(FPElementURL);
        Assert.That(driver.Url, Is.EqualTo(FPElementURL), "Our reqest was redirected to another url.");
        Assert.That(driver.Title.ToString(), Is.EqualTo("The Internet"), "Title on current page not equal to expected.");

        var textArea = driver.FindElement(By.Name("email"));
        Assert.That(textArea, Is.Not.Null, "Element on the previous step wasn`t found or equal to null.");

        textArea.Clear();
        textArea.SendKeys(testEmail);
        Assert.That(textArea.GetAttribute("value"), Is.EqualTo(testEmail), "Value of text area isn`t equal to entered test email.");

        driver.FindElement(By.Id("form_submit")).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        wait.Until(d => d.Url == redirectUrlAfterCorrectAnswer);

        var answer = driver.FindElement(By.Id("content"));

        Assert.That(driver.Url, Is.EqualTo(redirectUrlAfterCorrectAnswer), "URL after submit does not match with expected redirected.");
        Assert.That(answer.Text, Is.EqualTo(successAnswer), "Success message text does not match with recived.");
    }

    [Test]
    public void HorizontalSliderTest()
    {
        const string HSElementURL = homeUrl + "horizontal_slider";
        const string expectedH3 = "Horizontal Slider";
        const string minExpectedRangeValue = "0.0";
        const string maxExpectedRangeValue = "5.0";
        const string expectedDefaultRangeValue = "0";
        const string rangeTestValue = "4.5";
        int neededRightArrowClicks = (int)(float.Parse( rangeTestValue.Replace('.',',')) * 2);

        driver.Navigate().GoToUrl(HSElementURL);
        Assert.That(driver.Url, Is.EqualTo(HSElementURL), "Our reqest was redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");

        var sliderContainer = driver.FindElement(By.ClassName("sliderContainer"));
        var slider = sliderContainer.FindElement(By.XPath("//*[@id=\"content\"]/div/div/input"));
        var sliderValue = sliderContainer.FindElement(By.Id("range"));
        Assert.That(slider.GetAttribute("min"), Is.EqualTo(minExpectedRangeValue), $"Minimal value isn`t equal to expected \"{minExpectedRangeValue}\".");
        Assert.That(slider.GetAttribute("max"), Is.EqualTo(maxExpectedRangeValue), $"Maximal value isn`t equal to expected \"{maxExpectedRangeValue}\".");

        string currentValue = sliderValue.Text;
        Assert.That(currentValue, Is.EqualTo(expectedDefaultRangeValue), $"Default value isn`t equal to expected default value: \"{expectedDefaultRangeValue}\"");

        //The slider variable contains a slider bar, we move it to the right from its current position (moving from 0 to 4.5 requires 9 clicks of the right arrow key).
        for (int i = 0; i < neededRightArrowClicks; i++)
        {
            slider.SendKeys(Keys.ArrowRight);
        }
        Assert.That(sliderValue.Text, Is.EqualTo(rangeTestValue), $"Value of slider isn`t equal to entered: \"{rangeTestValue}\"");
    }

    [Test]
    public void DropdownTest()
    {
        const string DElementUrl = homeUrl + "dropdown";
        const string expectedH3 = "Dropdown List";
        const string expectedDefaultSelOptTxt = "Please select an option";
        const string expectedSelOpt1Txt = "Option 1";
        const string expectedSelOpt2Txt = "Option 2";

        driver.Navigate().GoToUrl(DElementUrl);
        Assert.That(driver.Url, Is.EqualTo(DElementUrl), "Our request was redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");

        var dropdownElement = new SelectElement(driver.FindElement(By.Id("dropdown")));

        Assert.That(dropdownElement.SelectedOption.Text, Is.EqualTo(expectedDefaultSelOptTxt), "Default selected option is incorrect.");

        dropdownElement.SelectByText("Option 1");
        Assert.That(dropdownElement.SelectedOption.Text, Is.EqualTo(expectedSelOpt1Txt), "Selected option is not Option 1.");

        dropdownElement.SelectByText("Option 2");
        Assert.That(dropdownElement.SelectedOption.Text, Is.EqualTo(expectedSelOpt2Txt), "Selected option is not Option 2.");
    }

    [Test]
    public void TyposTest()
    {
        const string TElementURL = homeUrl + "typos";
        const string expectedH3 = "Typos";
        const string expectedTypoText1 = "Sometimes you'll see a typo, other times you won't.";
        const string expectedTypoText2 = "Sometimes you'll see a typo, other times you won,t.";

        driver.Navigate().GoToUrl(TElementURL);
        Assert.That(driver.Url, Is.EqualTo(TElementURL), "Our requests were redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");

        var paragraphs = driver.FindElements(By.CssSelector("#content > div > p"));
        Assert.That(paragraphs, Is.Not.Empty, "Paragraphs on Typos page were not found.");

        string typoText = paragraphs[1].Text;
        Assert.That(typoText, Is.Not.Empty, "The paragraph text is empty.");
        Assert.That(typoText, Is.EqualTo(expectedTypoText1).Or.EqualTo(expectedTypoText2), "Example typo isn`t equal to any of expected variants of text");
    }

    [Test]
    public void EntryAdTest()
    {
        const string EAElementURL = homeUrl + "entry_ad";
        const string expectedH3 = "Entry Ad";
        const string modalTitle = "THIS IS A MODAL WINDOW";

        driver.Navigate().GoToUrl(EAElementURL);
        Assert.That(driver.Url, Is.EqualTo(EAElementURL), "Our requests were redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
        var modalWindow = wait.Until(d => d.FindElement(By.CssSelector("#modal > div.modal")));
        Assert.That(modalWindow.Displayed, Is.True, "Modal window is not displayed. (can fail bcs of incorrect work of page).");
        
        var modalHeading = driver.FindElement(By.CssSelector("#modal > div.modal > div.modal-title > h3"));
        Assert.That(modalHeading.Text, Is.EqualTo(modalTitle), "Modal window title is incorrect.");

        var closeButton = driver.FindElement(By.CssSelector("#modal > div.modal > div.modal-footer"));
        closeButton.Click();
        wait.Until(d => !modalWindow.Displayed);
        Assert.That(modalWindow.Displayed, Is.False, "Modal window was not closed.");
    }

    [Test]
    public void FileDownloadTest()
    {
        const string FDElementUrl = homeUrl + "download";
        const string expectedH3 = "File Downloader";
        //const string exampleFileName = "report.txt"; The file disappeared on the next day.

        driver.Navigate().GoToUrl(FDElementUrl);
        Assert.That(driver.Url, Is.EqualTo(FDElementUrl), "Our requests were redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");


        var fileLinks = driver.FindElements(By.CssSelector("#content > div > a"));
        Assert.That(fileLinks, Is.Not.Empty, "File download links were not found.");

        ////We use HttpClient and the Head() method to check the availability of each file based on its URL (href)
        using (var httpClient = new HttpClient())
        {
            foreach (var link in fileLinks)
            {
                string? fileUrl = link.GetAttribute("href");
                string fileName = link.Text;
                var response = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, fileUrl)).Result;
                Assert.That(response.IsSuccessStatusCode, Is.True, $"File \"{fileName}\" is not accessible on the server (Status code: {response.StatusCode}).");
            }
        }
    }

    [Test]
    public void BasicAuthTest()
    {
        const string username = "admin";
        const string password = "admin";
        const string wrongUsername = "wrong_user";
        const string wrongPassword = "wrong_password";
        const string expectedSuccessMessage = "Congratulations! You must have the proper credentials.";
        //It is not possible to enter data into this browser window using a standard FindElement (it cannot access this element).
        //So we pass the login credentials directly in the URL:
        string authUrl = $"https://{username}:{password}@the-internet.herokuapp.com/basic_auth";
        string negativeAuthUrl = $"https://{wrongUsername}:{wrongPassword}@the-internet.herokuapp.com/basic_auth";

        driver.Navigate().GoToUrl(authUrl);
        Assert.That(driver.Url, Is.EqualTo(authUrl), "Our requests were redirected to another url.");

        var wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var contentParagraph1 = wait1.Until(d => d.FindElement(By.CssSelector("#content > div > p")));
        Assert.That(contentParagraph1.Text.Trim(), Is.EqualTo(expectedSuccessMessage), "Basic authentication failed or unexpected message shown.");

        driver.Navigate().GoToUrl(negativeAuthUrl);
        Assert.That(driver.Url, Is.EqualTo(negativeAuthUrl), "Our requests were redirected to another url.");
        var wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        bool isSuccessTextPresent = false;
        try
        {
            var contentParagraph2 = driver.FindElement(By.CssSelector("#content > div > p"));
            if (contentParagraph2.Text.Contains(expectedSuccessMessage))
            {
                isSuccessTextPresent = true;
            }
        }
        catch (NoSuchElementException)
        {
            isSuccessTextPresent = false;
        }
        Assert.That(isSuccessTextPresent, Is.False, "Negative test failed: unauthorized user was granted access!");
    }

    [Test]
    public void DynamicLoadingNavigationTest()
    {
        const string DLNElementsMainUrl = homeUrl + "dynamic_loading";
        const string expectedH3 = "Dynamically Loaded Page Elements";
        const string Example1Url = DLNElementsMainUrl + "/1";
        const string Example2Url = DLNElementsMainUrl + "/2";
        const string expectedLoadedText = "Hello World!";

        driver.Navigate().GoToUrl(DLNElementsMainUrl);
        Assert.That(driver.Url, Is.EqualTo(DLNElementsMainUrl), "Our requests were redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");

        var example1Link = driver.FindElement(By.LinkText("Example 1: Element on page that is hidden"));
        Assert.That(example1Link, Is.Not.Null, "Example 1 link was not found.");
        example1Link.Click();

        //Example 1
        Assert.That(driver.Url, Is.EqualTo(Example1Url), "URL to Example 1 is incorrect.");

        var startButton1 = driver.FindElement(By.XPath("//div[@id='start']/button"));
        var loadedHiddenElement = driver.FindElement(By.Id("finish"));
        Assert.That(loadedHiddenElement.Displayed, Is.False, "Element should be hidden initially.");
        startButton1.Click();

        var wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
        wait1.Until(d => d.FindElement(By.Id("finish")).Displayed);
        var loadedElement1 = driver.FindElement(By.Id("finish"));
        Assert.That(loadedElement1.Text.Trim(), Is.EqualTo(expectedLoadedText), "Loaded text in Example 1 does not match with expected.");

        //Back to main page
        driver.Navigate().Back();
        Assert.That(driver.Url, Is.EqualTo(DLNElementsMainUrl), "Failed to return to Dynamic Loading main page.");

        var example2Link = driver.FindElement(By.LinkText("Example 2: Element rendered after the fact"));
        Assert.That(example2Link, Is.Not.Null, "Example 2 link was not found.");     
        example2Link.Click();

        //Example 2
        Assert.That(driver.Url, Is.EqualTo(Example2Url), "URL to Example 2 is incorrect.");
        bool elementExistsBeforeStart = true;
        try
        {
            driver.FindElement(By.Id("finish"));
        }
        catch (NoSuchElementException)
        {
            elementExistsBeforeStart = false;
        }
        Assert.That(elementExistsBeforeStart, Is.False, "Element shouldn't exist in DOM before start.");

        var startButton2 = driver.FindElement(By.XPath("//div[@id='start']/button"));
        startButton2.Click();
        var wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
        wait2.Until(d => d.FindElement(By.Id("finish")).Displayed);
        var loadedElement2 = driver.FindElement(By.Id("finish"));
        Assert.That(loadedElement2.Text.Trim(), Is.EqualTo(expectedLoadedText), "Loaded text in Example 2 does not match with expected.");
    }

    [Test]
    public void ContextMenuTest()
    {
        const string CMElementUrl = homeUrl + "context_menu";
        const string expectedH3 = "Context Menu";
        const string expectedAlertText = "You selected a context menu";

        driver.Navigate().GoToUrl(CMElementUrl);
        Assert.That(driver.Url, Is.EqualTo(CMElementUrl), "Our requests were redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedH3), "Title (h3) on current page not equal to expected.");

        var hotSpot = driver.FindElement(By.Id("hot-spot"));
        Assert.That(hotSpot, Is.Not.Null, "Hot-spot element was not found.");

        Actions actions = new Actions(driver);
        actions.ContextClick(hotSpot).Perform();
        IAlert alert = WaitForAlert(5);
        Assert.That(alert, Is.Not.Null, "Context menu alert did not appear.");
        Assert.That(alert.Text, Is.EqualTo(expectedAlertText), "Alert text isn`t equal to expected.");

        alert.Accept();
    }

    [Test]
    public void RedirectLinkAndStatusCodesCompleteTest()
    {
        const string RedirectorUrl = homeUrl + "redirector";
        const string expectedRedirectionH3 = "Redirection";
        const string statusCodesPageUrl = homeUrl + "status_codes";
        const string expectedStatusCodesH3 = "Status Codes";
        var expectedCodes = new List<string> { "200", "301", "404", "500" };

        driver.Navigate().GoToUrl(RedirectorUrl);
        Assert.That(driver.Url, Is.EqualTo(RedirectorUrl), "Our requests were redirected to another url.");
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedRedirectionH3), "Title (h3) on redirection page not equal to expected.");

        var redirectLink = driver.FindElement(By.Id("redirect"));
        Assert.That(redirectLink, Is.Not.Null, "Redirect link was not found.");
        
        redirectLink.Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Url == statusCodesPageUrl);
        Assert.That(driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")).Text, Is.EqualTo(expectedStatusCodesH3), "Title (h3) on status codes page not equal to expected.");

        foreach (var code in expectedCodes)
        {
            var codeLink = driver.FindElement(By.LinkText(code));
            Assert.That(codeLink, Is.Not.Null, $"Link for status code {code} was not found.");
            codeLink.Click();
            string expectedSubUrl = statusCodesPageUrl + "/" + code;
            wait.Until(d => d.Url == expectedSubUrl);
            var responseParagraph = driver.FindElement(By.CssSelector("#content > div > p"));
            Assert.That(responseParagraph.Text, Does.Contain(code), $"Response message does not contain status code {code}.");

            var backLink = driver.FindElement(By.LinkText("here"));
            Assert.That(backLink, Is.Not.Null, "Link, that redirect back on status codes page, was not found.");
            backLink.Click();
            wait.Until(d => d.Url == statusCodesPageUrl);
            Assert.That(driver.Url, Is.EqualTo(statusCodesPageUrl), "Failed to return to statusCodes page.");
        }
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose(); //added for IDE (without it show error, but work correct), previous method also dispose driver.
    }
}
