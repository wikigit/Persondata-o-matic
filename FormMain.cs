using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Microsoft.Win32;
using DotNetWikiBot;

namespace Persondata_o_matic
{
    public partial class FormMain : Form
    {
        Site site;
        Thread loadAheadThread, savePendingThread;
        string currentPageText;
        string currentTemplate;
        int currentTemplateIndex;

        PageListSource pageListSource;
        string pageListSourceValue;
        string origPageText;
        string origName = "", origAlternativeNames = "", origShortDescription = "", origDateOfBirth = "", origPlaceOfBirth = "", origDateOfDeath = "", origPlaceOfDeath = "";
        bool manualEditSummary;

        PageList pageList;
        // Lock covers both lastLoadedPageIndex and lastSavedPageIndex
        object lastLoadedPageIndexLock = new object();
        int lastLoadedPageIndex = -1;
        int lastSavedPageIndex = -1;
        int numLoadAheadPages;
        int numCategoryPagesToRetrieve;
        bool randomizePageOrder;
        bool loadPageListOnDemand;
        Queue<SaveInfo> saveQueue = new Queue<SaveInfo>();

        private static string GetRegexForField(string field)
        {
            //   make sure to look ahead for "|" or "}" since lookup is non-greedy and would otherwise halt immediatelly
            //                          | wikilink   |     | template   |                 \|/
            return field + @"\s*=((?:(?:\[\[[^\]]*\]\])*(?:\{\{[^\}]*\}\})*[^\|}]*?)*)(?=[\|\}])";
        }
        Regex regexPersondataTemplate = new Regex(@"\{\{\s*persondata(?:(?:\{\{[^\}]*\}\})*[^}]*?)*\}\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexName                  = new Regex(GetRegexForField("NAME"),              RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexAlternativeNames      = new Regex(GetRegexForField("ALTERNATIVE NAMES"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexShortDescription      = new Regex(GetRegexForField("SHORT DESCRIPTION"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexPersondataDateOfBirth = new Regex(GetRegexForField("DATE OF BIRTH"),     RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexPlaceOfBirth          = new Regex(GetRegexForField("PLACE OF BIRTH"),    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexDateOfDeath           = new Regex(GetRegexForField("DATE OF DEATH"),     RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexPlaceOfDeath          = new Regex(GetRegexForField("PLACE OF DEATH"),    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        Regex regexInfoboxTemplate = new Regex(@"\{\{\s*[a-zA-Z0-9 ]*infobox(?:(?:\{\{[^\}]*\}\})*[^}]*?)*\}\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexInfoboxDateOfBirth = new Regex(GetRegexForField("birth_date"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        Regex regexInfoboxDateOfDeath = new Regex(GetRegexForField("death_date"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        Regex regexSortKey = new Regex(@"\[\[\s*Category\s*:\s*[^\|\]]*\|\s*([^\]]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            this.Hide();
            loginForm = new FormLogin();
            if (loginForm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
            {
                Application.Exit();
                return;
            }

            loadingDialog = new LoadingDialog();
            loadingDialog.Message = "Logging in...";
            loadingDialog.Show(this);
            this.Enabled = false;
            Thread loginThread = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    errorMessage = null;
                    site = new Site("http://en.wikipedia.org", loginForm.Username, loginForm.Password);
                }
                catch (WikiBotException ex)
                {
                    if (ex.Message.Contains("Login failed"))
                    {
                        errorMessage = "Login failed. Check your username and password and try again.";
                    }
                    else
                    {
                        errorMessage = "Encountered error: " + ex.Message;
                    }
                }
                this.Invoke(new MethodInvoker(CompleteLogin));
            }));
            loginThread.Start();
        }

        FormLogin loginForm;
        string errorMessage;
        LoadingDialog loadingDialog;
        string pageListNext = ""; // Used when loadPageListOnDemand == true to stream page list

        void CompleteLogin()
        {
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage);
                loadingDialog.Hide();
                Login();
                return;
            }

            numLoadAheadPages = loginForm.MaxLoadAheadPages;
            numCategoryPagesToRetrieve = loginForm.MaxCategoryPages;
            randomizePageOrder = loginForm.RandomizeOrder;
            loadPageListOnDemand = !randomizePageOrder;
            pageList = new PageList(site);
            pageListSource = loginForm.PageListSource;
            pageListSourceValue = loginForm.PageListSourceValue;

            if (loadPageListOnDemand)
            {
                // Actually just loading some of the list of pages, more will be loaded on-demand later
                loadingDialog.Message = "Loading list of pages...";
            }
            else
            {
                loadingDialog.Message = "Loading list of pages (up to " + numCategoryPagesToRetrieve + ")...";
            }
            Thread fillFromCategoryThread = new Thread(new ThreadStart(delegate()
            {
                switch (pageListSource)
                {
                    case PageListSource.category:
                        if (loadPageListOnDemand)
                        {
                            pageList.FillSomeFromCategoryEx(pageListSourceValue, ref pageListNext);
                        }
                        else
                        {
                            pageList.FillAllFromCategoryEx(pageListSourceValue, numCategoryPagesToRetrieve);
                        }
                        Invoke(new MethodInvoker(CompletePageListLoad));
                        break;
                    case PageListSource.file:
                        pageList.FillFromFile(pageListSourceValue);
                        Invoke(new MethodInvoker(CompletePageListLoad));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //pageList.FillFromFile("pages.txt"); // just for debugging to load up some pages

            }));
            fillFromCategoryThread.Start();
        }

        void CompletePageListLoad()
        {
            loadingDialog.Hide();

            if (randomizePageOrder)
            {
                Random r = new Random();
                ShufflePageList(r, pageList);
            }

            loadAheadThread = new Thread(new ThreadStart(LoadAhead));
            loadAheadThread.Start();
            savePendingThread = new Thread(new ThreadStart(SavePending));
            savePendingThread.Start();
            ShowNextPage();
        }

        private static void ShufflePageList(Random r, PageList list)
        {
            for (int i = list.Count() - 1; i >= 1; i--)
            {
                int j = r.Next(0, i + 1);
                Page temp = list[j];
                list[j] = list[i];
                list[i] = temp;
            }
        }

        void SavePending()
        {
            while (true)
            {
                SaveInfo saveInfo = null;
                lock (saveQueue)
                {
                    if (saveQueue.Count > 0)
                    {
                        saveInfo = saveQueue.Dequeue();
                    }
                }
                if (saveInfo != null)
                {
                    try
                    {
                        saveInfo.Page.Save(saveInfo.EditSummary, /*isMinorEdit*/false);
                    }
                    catch (Exception e)
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            MessageBox.Show("Could not save page " + saveInfo.Page.title + " due to error:\n" + e.Message);
                        }));
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        void ShowNextPage()
        {
            SetControlsEnabled(false);
            webBrowser.Navigate("about:blank"); // clear the contents

            int nextPage = lastSavedPageIndex + 1;
            if (nextPage >= pageList.Count())
            {
                MessageBox.Show("No more pages.");
                return;
            }

            bool pageAvailable;
            lock (lastLoadedPageIndexLock)
            {
                pageAvailable = (lastLoadedPageIndex >= nextPage);
            }
            if (pageAvailable)
            {
                CompleteWaitForPage();
            }
            else
            {
                loadingDialog.Message = "Loading pages...";
                loadingDialog.Show();
                Thread waitForPageThread = new Thread(new ThreadStart(delegate()
                {
                    while (true)
                    {
                        lock (lastLoadedPageIndexLock)
                        {
                            if (lastLoadedPageIndex >= nextPage)
                            {
                                break;
                            }
                        }
                        Thread.Sleep(100);
                    }
                    this.Invoke(new MethodInvoker(CompleteWaitForPage));
                }));
                waitForPageThread.Start();
            }
        }

        private void CompleteWaitForPage()
        {
            SetControlsEnabled(true);
            this.Enabled = true;
            int nextPage = lastSavedPageIndex + 1;
            Page page = pageList[nextPage];
            UpdateControlsFromPage(page);
            loadingDialog.Hide();
        }

        private void SetControlsEnabled(bool value)
        {
            buttonSaveAndNext.Enabled = value;
            buttonSkip.Enabled = value;
            buttonRemove.Enabled = value;
            textBoxName.Enabled = value;
            textBoxAlternativeNames.Enabled = value;
            textBoxShortDescription.Enabled = value;
            textBoxDateOfBirth.Enabled = value;
            textBoxPlaceOfBirth.Enabled = value;
            textBoxDateOfDeath.Enabled = value;
            textBoxPlaceOfDeath.Enabled = value;
            textBoxEditSummary.Enabled = value;
        }

        private void UpdateControlsFromPage(Page page)
        {
            textBoxWikitextOriginal.Text = Regex.Replace(page.text, @"(?!=\r)\n", "\r\n");
            // Removing comment per consensus at [[Wikipedia_talk:Persondata#Can_we_stop_adding_the_annoying.2C_useless_comment_now.3F]]
            currentPageText = page.text.Replace(" <!-- Metadata: see [[Wikipedia:Persondata]]. -->", "");
            origPageText = page.text;
            textBoxTitle.Text = page.title;
            textBoxWikitext.Text = Regex.Replace(currentPageText, @"(?!=\r)\n", "\r\n");

            if (tabControlPage.SelectedTab == tabPageBrowser)
            {
                NavigateWebBrowserToCurrentPage();
            }

            // Fill in the textboxes with values from the existing template
            Match templateMatch = regexPersondataTemplate.Match(currentPageText);
            currentTemplate = regexPersondataTemplate.Match(currentPageText).Value;
            currentTemplateIndex = templateMatch.Index;

            origName = regexName.Match(currentTemplate).Groups[1].Value.Trim(); // NOTE: set origXXX values before the textbox value, which would trigger recoloring, but it would screw up if origXXX wouldn't be set yet
            textBoxName.Text = origName;
            SetToolTipForFieldTextbox(textBoxName);

            origAlternativeNames = regexAlternativeNames.Match(currentTemplate).Groups[1].Value.Trim();
            textBoxAlternativeNames.Text = origAlternativeNames;
            SetToolTipForFieldTextbox(textBoxAlternativeNames);

            origShortDescription = regexShortDescription.Match(currentTemplate).Groups[1].Value.Trim();
            textBoxShortDescription.Text = origShortDescription;
            SetToolTipForFieldTextbox(textBoxShortDescription);

            origDateOfBirth = regexPersondataDateOfBirth.Match(currentTemplate).Groups[1].Value.Trim();
            textBoxDateOfBirth.Text = origDateOfBirth;
            SetToolTipForFieldTextbox(textBoxDateOfBirth);

            origPlaceOfBirth = regexPlaceOfBirth.Match(currentTemplate).Groups[1].Value.Trim();
            textBoxPlaceOfBirth.Text = origPlaceOfBirth;
            SetToolTipForFieldTextbox(textBoxPlaceOfBirth);

            origDateOfDeath = regexDateOfDeath.Match(currentTemplate).Groups[1].Value.Trim();
            textBoxDateOfDeath.Text = origDateOfDeath;
            SetToolTipForFieldTextbox(textBoxDateOfDeath);

            origPlaceOfDeath = regexPlaceOfDeath.Match(currentTemplate).Groups[1].Value.Trim();
            textBoxPlaceOfDeath.Text = origPlaceOfDeath;
            SetToolTipForFieldTextbox(textBoxPlaceOfDeath);

            // Time to guess values
            if (textBoxName.Text == "") textBoxName.Text = GuessName(page);
            textBoxDateOfBirth.Text = GuessDate(textBoxDateOfBirth.Text, GuessDateType.birth);
            textBoxDateOfDeath.Text = GuessDate(textBoxDateOfDeath.Text, GuessDateType.death);

            manualEditSummary = false;

            UpdateEditSummary();

            string warningText = "";

            if (regexPersondataTemplate.Matches(currentPageText).Count > 1)
            {
                warningText += "* Multiple {{persondata}} templates found!";
            }

            if (warningText != "")
            {
                labelWarnings.Text = "Warning:\n" + warningText;
                labelWarnings.Show();
            }
            else
            {
                labelWarnings.Hide();
            }

            UpdateFocus();
        }

        /// <summary>
        /// Add a tooltip for a given textbox based on its current value as formatted for field value textboxes
        /// </summary>
        private void SetToolTipForFieldTextbox(TextBox textBox)
        {
            toolTipForTextboxes.SetToolTip(textBox, textBox.Text != "" ? "Original value: " + "\"" + textBox.Text + "\"" : "No original value");
        }

        private void NavigateWebBrowserToCurrentPage()
        {
            // Don't try loading when no more pages left
            if (pageList.Count() <= lastSavedPageIndex + 1)
                return;

            // Syntax: http://en.wikipedia.org/w/index.php?title=Elephant&action=render
            string url = site.site + "/w/index.php?action=render&title=" + HttpUtility.UrlEncode(pageList[lastSavedPageIndex + 1].title);
            if (webBrowser.Url == null || webBrowser.Url.ToString() != url)
                webBrowser.Navigate(url);
        }

        private string GuessName(Page page)
        {
            Match sortKeyMatch = regexSortKey.Match(currentPageText);
            if (sortKeyMatch.Success)
            {
                return sortKeyMatch.Groups[1].Value.Trim();
            }
            string[] titleWords = page.title.Split(new char[] { ' ' });
            if (!page.title.Contains(":") && !page.title.Contains("("))
            {
                if (titleWords.Length == 2)
                {
                    return titleWords[1] + ", " + titleWords[0];
                }
                else
                {
                    return page.title;
                }
            }
            return "";
        }

        private const string monthNames = @"(Jan(?:uary)?|Feb(?:ruary)?|Mar(c?:h)?|Apr(?:il)?|May|Jun[ey]?|Aug(?:ust)?|Sep(?:tember)?|Oct(?:ober)?|Nov(?:embr)?|Dec(?:ember)?)";

        private enum GuessDateType { birth, death }
        private const string birthDateTemplates = @"(?:Birth date|Birth year|Birthdate|Date of birth|Dob|Birth date and age|BDA|Birthdate and age|Birthdateandage)\s*\|\s*";
        private const string deathDateTemplates = @"(?:Death date|Date of death|Date of disappearance with age|Dda|Deathdateandage|Event date and age)\s*\|\s*";

        private string GuessDate(string currentValue, GuessDateType guessDateType)
        {
            // First, let's see if we can recognise the existing date
            // Basically, we'd want to fill in more info if possible in case the original data is lacking, 
            // so we don't skip the field right away if it is non-empty

            // This variable will signify how complex the existing value is:
            // > 0 -- no value
            // > 1 -- just 1 data point: year
            // > 2 -- just 2 data points: year and month
            // > 3 -- 3 data points: year, month, day
            // > 4 -- could not parse the value, so shouldn't mess with this
            // This implies if we discover day, month, year, but the current value is just year, we can update it
            // Real example: http://en.wikipedia.org/w/index.php?title=Fuad_Abdurahmanov&diff=480997918&oldid=480997834
            int currentComplexity = 0;

            string currentValueTrimmed = currentValue.Trim();

            if (currentValueTrimmed != "")
            {
                currentComplexity = 4; // by default, we cannot parse it

                // YEAR
                if (Regex.Match(currentValueTrimmed, @"^(\[\[)?[0-9]{4}(\]\])?$").Success)
                    currentComplexity = 1;

                // MONTH YEAR
                else if (Regex.Match(currentValueTrimmed, @"^" + monthNames + @"\s*[0-9]{4}$", RegexOptions.IgnoreCase).Success)
                    currentComplexity = 2;

                // DAY MONTH YEAR
                else if (Regex.Match(currentValueTrimmed, @"^[0-9]{2}(?:th|nd|rd)?\s*" + monthNames + @"\s*[0-9]{4}$", RegexOptions.IgnoreCase).Success)
                    currentComplexity = 3;

                // MONTH DAY YEAR
                else if (Regex.Match(currentValueTrimmed, @"^" + monthNames + @"^[0-9]{2}(?:th|nd|rd)?,?\s*[0-9]{4}$", RegexOptions.IgnoreCase).Success)
                    currentComplexity = 3;

                // DAY MONTH YEAR (probably)
                else if (Regex.Match(currentValueTrimmed, @"^[0-9]{2}[\s\-\.]*[0-9]{2}[\s\-\.]*[0-9]{4}$", RegexOptions.IgnoreCase).Success)
                    currentComplexity = 3;

                // YEAR MONTH DAY
                else if (Regex.Match(currentValueTrimmed, @"^[0-9]{4}[\s\-\.]*[0-9]{2}[\s\-\.]*[0-9]{2}$", RegexOptions.IgnoreCase).Success)
                    currentComplexity = 3;
            }

            // We will skip guessing if we already have 3 data points (day, month, year) because we cannot guess anything more accurate anyway
            // Potentially, we can verify that our guess is the same as the value and alert the user if it isn't
            if (currentComplexity >= 3)
                return currentValue;

            Match infoboxMatch = regexInfoboxTemplate.Match(currentPageText);
            if (infoboxMatch.Success)
            {
                string infoboxText = infoboxMatch.Value;

                Match fieldMatch = guessDateType == GuessDateType.birth ? regexInfoboxDateOfBirth.Match(infoboxText) : regexInfoboxDateOfDeath.Match(infoboxText);
                if (fieldMatch.Success)
                {
                    // Get the value of the field
                    string value = fieldMatch.Groups[1].Value.Trim();
                    if (value == "") return currentValue;

                    // Trim any comments from the field
                    value = Regex.Replace(value, "<!--.*?-->", "").Trim();
                    if (value == "") return currentValue;

                    if (currentComplexity < 3) // if current value is less than day+month+year
                    {
                        // Try to find birth date template
                        // {{Birth date|1993|2|24|df=yes}} returns "24 February 1993"
                        // {{Birth date|1993|2|24|mf=yes}} returns "February 24, 1993"
                        Match match = Regex.Match(
                            value,
                            @"\{\{\s*" +
                            (guessDateType == GuessDateType.birth ? birthDateTemplates : deathDateTemplates) +
                            @"([0-9]+)\s*\|\s*([0-9]+)\s*\|\s*([0-9]+)\s*\|?(?:\s*([md])f\s*=\s*yes)?", // don't care about the ending
                            RegexOptions.IgnoreCase
                            );
                        if (match.Success)
                        {
                            if (match.Groups[4].Value == "m")
                                return match.Groups[3] + " " + MonthName(int.Parse(match.Groups[2].Value)) + " " + match.Groups[1];
                            else
                                return MonthName(int.Parse(match.Groups[2].Value)) + " " + match.Groups[3] + ", " + match.Groups[1];
                        }
                    }

                    // Birth-date death-date -- plain text
                    // {{start-date|7 December 1941}}
                    // {{start-date|5:43PM HST, December 7th, 1941|tz=y}}
                    // {{start-date| December 8, 1941 12:50PM Australia/Adelaide|tz=y}}

                    if (currentComplexity == 0) // if there was no value before
                        return value; // try the field without any parsing, user can fix it up
                }
            }

            return currentValue;
        }

        private string MonthName(int monthNumber)
        {
            switch (monthNumber)
            {
                case 01: return "January";
                case 02: return "February";
                case 03: return "March";
                case 04: return "April";
                case 05: return "May";
                case 06: return "June";
                case 07: return "July";
                case 08: return "August";
                case 09: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return monthNumber < 10 ? "0" + monthNumber : monthNumber.ToString();
            }
        }

        private void UpdateFocus()
        {
            if (pageListSource == PageListSource.category)
            {
                if (pageListSourceValue == "Category:Persondata templates without name parameter")
                {
                    textBoxName.Focus();
                }
                else if (pageListSourceValue == "Category:Persondata templates without short description parameter")
                {
                    textBoxShortDescription.Focus();
                }
                else
                {
                    textBoxName.Focus();
                }
            }
            else
            {
                textBoxName.Focus();
            }
        }

        void LoadAhead()
        {
            bool pageListDone = false;
            while (true)
            {
                bool shouldLoadNext;
                lock (lastLoadedPageIndexLock)
                {
                    shouldLoadNext = (lastLoadedPageIndex - (lastSavedPageIndex) - 1 < numLoadAheadPages);
                }
                if (shouldLoadNext)
                {
                    int nextPage = lastLoadedPageIndex + 1;
                    if (!pageListDone && nextPage >= pageList.Count() - numLoadAheadPages && loadPageListOnDemand)
                    {
                        int prevCount = pageList.Count();
                        pageList.FillSomeFromCategoryEx(pageListSourceValue, ref pageListNext);
                        pageListDone = (pageList.Count() == prevCount);
                    }
                    if (nextPage >= pageList.Count())
                    {
                        break;
                    }
                    try
                    {
                        pageList[nextPage].Load();
                    }
                    catch (Exception e)
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            MessageBox.Show("Could not load page " + pageList[nextPage].title + " due to error:\n" + e.Message);
                        }));
                        break;
                    }
                    lock (lastLoadedPageIndexLock)
                    {
                        lastLoadedPageIndex++;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            textBoxWikitext.Width = this.ClientSize.Width - panelPersondata.Width;
            textBoxWikitext.Height = this.ClientSize.Height - (textBoxTitle.Top + textBoxTitle.Height + 8);
            textBoxWikitext.Top = this.ClientSize.Height - textBoxWikitext.Height;
            textBoxTitle.Width = this.ClientSize.Width - panelPersondata.Width - textBoxTitle.Left - buttonOpenInBrowser.Width - 16;
            buttonOpenInBrowser.Left = textBoxTitle.Left + textBoxTitle.Width + 8;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (loadingDialog != null)
            {
                loadingDialog.Close();
            }
            if (loadAheadThread != null)
            {
                loadAheadThread.Abort();
            }
            if (savePendingThread != null)
            {
                for (int i=0; i < 300; i++)
                {
                    lock (saveQueue)
                    {
                        if (saveQueue.Count == 0)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(100);
                }
                savePendingThread.Abort();
            }
            Application.Exit();
        }

        private delegate string UpdateEditSummaryHelper(string name, string origValue, string value);

        private string UpdateEditSummaryHelperExtraLong(string name, string origValue, string value)
        {
            if (origValue.Trim() == "" && value.Trim() != "")
            {
                return "added " + name + " \"" + value.Trim() + "\"";
            }
            else if (origValue.Trim() != "" && value.Trim() == "")
            {
                return "removed " + name + " (was \"" + origValue.Trim() + "\")";
            }
            else if (origValue.Trim() != value.Trim() && origValue.Trim() != "" && value.Trim() != "")
            {
                return "updated " + name + " from \"" + origValue.Trim() + "\" to \"" + value.Trim() + "\"";
            }
            return "";
        }

        private string UpdateEditSummaryHelperLong(string name, string origValue, string value)
        {
            if (origValue.Trim() == "" && value.Trim() != "")
            {
                return "added " + name + " \"" + value.Trim() + "\"";
            }
            else if (origValue.Trim() != "" && value.Trim() == "")
            {
                return "removed " + name;
            }
            else if (origValue.Trim() != value.Trim() && origValue.Trim() != "" && value.Trim() != "")
            {
                return "updated " + name + " to \"" + value.Trim() + "\"";
            }
            return "";
        }

        private string UpdateEditSummaryHelperShort(string name, string origValue, string value)
        {
            if (origValue.Trim() == "" && value.Trim() != "")
            {
                return "added " + name;
            }
            else if (origValue.Trim() != "" && value.Trim() == "")
            {
                return "removed " + name;
            }
            else if (origValue.Trim() != value.Trim() && origValue.Trim() != "" && value.Trim() != "")
            {
                return "updated " + name;
            }
            return "";
        }

        private void CreateSummaryList(UpdateEditSummaryHelper helper, List<string> list)
        {
            list.Add(helper("name", origName, textBoxName.Text));
            list.Add(helper("alternative names", origAlternativeNames, textBoxAlternativeNames.Text));
            list.Add(helper("short description", origShortDescription, textBoxShortDescription.Text));
            list.Add(helper("date of birth", origDateOfBirth, textBoxDateOfBirth.Text));
            list.Add(helper("place of birth", origPlaceOfBirth, textBoxPlaceOfBirth.Text));
            list.Add(helper("date of death", origDateOfDeath, textBoxDateOfDeath.Text));
            list.Add(helper("place of death", origPlaceOfDeath, textBoxPlaceOfDeath.Text));
            list.RemoveAll(delegate(string s) { return s.Length == 0; });
        }

        private void UpdateEditSummary()
        {
            if (manualEditSummary)
            {
                return;
            }

            List<string> listExtraLong = new List<string>();
            List<string> listLong = new List<string>();
            List<string> listShort = new List<string>();
            CreateSummaryList(UpdateEditSummaryHelperExtraLong, listExtraLong);
            CreateSummaryList(UpdateEditSummaryHelperLong, listLong);
            CreateSummaryList(UpdateEditSummaryHelperShort, listShort);

            if (listLong.Count == 0)
            {
                textBoxEditSummary.Text = "";
            }
            else
            {
                textBoxEditSummary.Text = "Persondata: " + string.Join(", ", listExtraLong.ToArray()) + " using [[WP:POM|Persondata-o-matic]]";

                if (textBoxEditSummary.Text.Length > 250)
                    textBoxEditSummary.Text = "Persondata: " + string.Join(", ", listLong.ToArray()) + " using [[WP:POM|Persondata-o-matic]]";

                if (textBoxEditSummary.Text.Length > 250)
                    textBoxEditSummary.Text = "Persondata: " + string.Join(", ", listShort.ToArray()) + " using [[WP:POM|Persondata-o-matic]]";

                if (textBoxEditSummary.Text.Length > 250)
                    textBoxEditSummary.Text = "Modifying Persondata using [[WP:POM|Persondata-o-matic]]";
            }
            manualEditSummary = false;
        }

        private void UpdateField(string value, Regex regex)
        {
            value = value.Trim();
            Match m = regex.Match(currentTemplate);
            int startIndexTemplate = m.Groups[1].Index;
            int startIndexPage = currentTemplateIndex + startIndexTemplate;
            int length = m.Groups[1].Length;
            if (m.Groups[2].Value.Trim() == value.Trim())
            {
                // No change except for whitespace, don't mess with it
                return;
            }
            // Keep enter at the end if there is one already
            string suffix = "";
            if (m.Groups[1].Value.EndsWith("\n"))
            {
                suffix = "\n";
            }
            currentPageText = currentPageText.Remove(startIndexPage, length);
            currentPageText = currentPageText.Insert(startIndexPage, " " + value + suffix);
            currentTemplate = currentTemplate.Remove(startIndexTemplate, length);
            currentTemplate = currentTemplate.Insert(startIndexTemplate, " " + value + suffix);


            int savePos = TextBoxHelpers.GetVScrollPos(textBoxWikitext);
            textBoxWikitext.Text = currentPageText.Replace("\n", "\r\n");
            TextBoxHelpers.SetVScrollPos(textBoxWikitext, savePos);

            UpdateEditSummary();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxName.Text, regexName);
            textBoxName.BackColor = ColorFromChange(origName, textBoxName.Text);
        }

        private void textBoxAlternativeNames_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxAlternativeNames.Text, regexAlternativeNames);
            textBoxAlternativeNames.BackColor = ColorFromChange(origAlternativeNames, textBoxAlternativeNames.Text);
        }

        private void textBoxShortDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxShortDescription.Text, regexShortDescription);
            textBoxShortDescription.BackColor = ColorFromChange(origShortDescription, textBoxShortDescription.Text);
        }

        private void textBoxDateOfBirth_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxDateOfBirth.Text, regexPersondataDateOfBirth);
            textBoxDateOfBirth.BackColor = ColorFromChange(origDateOfBirth, textBoxDateOfBirth.Text);
        }

        private void textBoxPlaceOfBirth_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxPlaceOfBirth.Text, regexPlaceOfBirth);
            textBoxPlaceOfBirth.BackColor = ColorFromChange(origPlaceOfBirth, textBoxPlaceOfBirth.Text);
        }

        private void textBoxDateOfDeath_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxDateOfDeath.Text, regexDateOfDeath);
            textBoxDateOfDeath.BackColor = ColorFromChange(origDateOfDeath, textBoxDateOfDeath.Text);
        }

        private void textBoxPlaceOfDeath_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxPlaceOfDeath.Text, regexPlaceOfDeath);
            textBoxPlaceOfDeath.BackColor = ColorFromChange(origPlaceOfDeath, textBoxPlaceOfDeath.Text);
        }

        private void textBoxEditSummary_TextChanged(object sender, EventArgs e)
        {
            manualEditSummary = true;
        }

        /// <summary>
        /// This will return a background color for a textbox based on what the original value was and what the new value is
        /// </summary>
        private Color ColorFromChange(string original, string current)
        {
            // No change
            if (original == current) return Color.White;

            // We removed the field
            if (original != "" && current == "") return Color.Tomato;

            // We added the field
            if (original == "" && current != "") return Color.Cyan;

            // We modified the field
            if (original != "" && current != "") return Color.Gold;

            return Color.HotPink; // becuse we are awesome and this should never happen
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            UpdateFocus();
            tabControlPage.SelectTab(tabPageMarkup);
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            if (lastSavedPageIndex < pageList.Count())
            {
                lock (lastLoadedPageIndexLock)
                {
                    lastSavedPageIndex++;
                }
                ShowNextPage();
            }
        }

        private void buttonSaveAndNext_Click(object sender, EventArgs e)
        {
            if (lastSavedPageIndex < pageList.Count())
            {
                lock (lastLoadedPageIndexLock)
                {
                    lastSavedPageIndex++;
                }
                if (currentPageText != origPageText)
                {
                    pageList[lastSavedPageIndex].text = currentPageText;
                    SaveInfo info = new SaveInfo();
                    info.Page = pageList[lastSavedPageIndex];
                    info.EditSummary = textBoxEditSummary.Text;
                    lock (saveQueue)
                    {
                        saveQueue.Enqueue(info);
                    }
                }
                ShowNextPage();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (lastSavedPageIndex < pageList.Count())
            {
                lock (lastLoadedPageIndexLock)
                {
                    lastSavedPageIndex++;
                }
                currentPageText = regexPersondataTemplate.Replace(currentPageText, "");
                if (currentPageText != origPageText)
                {
                    pageList[lastSavedPageIndex].text = currentPageText;
                    SaveInfo info = new SaveInfo();
                    info.Page = pageList[lastSavedPageIndex];
                    info.EditSummary = "Removing persondata (not a biographical article) using [[WP:POM|Persondata-o-matic]]";
                    lock (saveQueue)
                    {
                        saveQueue.Enqueue(info);
                    }
                }
                ShowNextPage();
            }
        }

        private void buttonOpenInBrowser_Click(object sender, EventArgs e)
        {
            // Based on http://dotnetpulse.blogspot.com/2006/04/opening-url-from-within-c-program.html
            string key = @"http\shell\open\command";
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            string defaultBrowserPath = ((string)registryKey.GetValue(null, null)).Split('"')[1];
            try
            {
                Process.Start(defaultBrowserPath, "http://en.wikipedia.org/wiki/" + pageList[lastSavedPageIndex + 1].title.Replace(' ', '_'));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred starting default browser: " + ex.Message);
            }
        }

        private void tabControlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlPage.SelectedTab == tabPageBrowser)
            {
                NavigateWebBrowserToCurrentPage();
            }
        }

        private void restoreValueButton_Click(object sender, EventArgs e)
        {
            textBoxName.Text = origName;
            textBoxAlternativeNames.Text = origAlternativeNames;
            textBoxShortDescription.Text = origShortDescription;
            textBoxDateOfBirth.Text = origDateOfBirth;
            textBoxPlaceOfBirth.Text = origPlaceOfBirth;
            textBoxDateOfDeath.Text = origDateOfDeath;
            textBoxPlaceOfDeath.Text = origPlaceOfDeath;
        }
    }

    public class SaveInfo
    {
        public Page Page;
        public string EditSummary;
    }

    public static class TextBoxHelpers
    {
        // From http://www.pinvoke.net/default.aspx/user32.getscrollpos , http://pinvoke.net/default.aspx/user32/SetScrollPos.html
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar);
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        // Adapted from http://www.vbforums.com/showthread.php?p=3801101
        private const uint EM_LINESCROLL = 0xb6;
        private const uint WM_VSCROLL = 0x115;
        private const int SB_TOP = 6;

        // Adapted from http://www.pinvoke.net/default.aspx/user32.getscrollpos
        public static int GetVScrollPos(TextBox textBox)
        {
            return GetScrollPos((IntPtr)textBox.Handle, System.Windows.Forms.Orientation.Vertical);
        }

        public static void SetVScrollPos(TextBox textBox, int position)
        {
            SetScrollPos((IntPtr)textBox.Handle, System.Windows.Forms.Orientation.Vertical, position, true);
            // Adapted from http://stackoverflow.com/questions/1069497/how-to-scroll-down-in-a-textbox-by-code-in-c-sharp
            SendMessage(textBox.Handle, WM_VSCROLL, new IntPtr(SB_TOP), IntPtr.Zero);
            SendMessage(textBox.Handle, EM_LINESCROLL, IntPtr.Zero, new IntPtr(position));
        }
    }
}
