using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

        string category;
        string origPageText;
        string origName = "", origAlternativeNames = "", origShortDescription = "", origDateOfBirth = "", origPlaceOfBirth = "", origDateOfDeath = "", origPlaceOfDeath = "";
        bool manualEditSummary;

        PageList pageList;
        // Lock covers both lastLoadedPageIndex and lastSavedPageIndex
        object lastLoadedPageIndexLock = new object();
        int lastLoadedPageIndex = -1;
        int lastSavedPageIndex = -1;
        int numLoadAheadPages;
        Queue<SaveInfo> saveQueue = new Queue<SaveInfo>();


        private static string GetRegexForField(string field)
        {
            return @"\{\{\s*persondata[^\}]*\|\s*" + field + @"\s*=([ \t]*([^\}\|]*))";
        }
        Regex regexTemplate = new Regex(@"\{\{\s*persondata[^\}]*\}\}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexName             = new Regex(GetRegexForField("NAME"),              RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexAlternativeNames = new Regex(GetRegexForField("ALTERNATIVE NAMES"), RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexShortDescription = new Regex(GetRegexForField("SHORT DESCRIPTION"), RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexDateOfBirth      = new Regex(GetRegexForField("DATE OF BIRTH"),     RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexPlaceOfBirth     = new Regex(GetRegexForField("PLACE OF BIRTH"),    RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexDateOfDeath      = new Regex(GetRegexForField("DATE OF DEATH"),     RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexPlaceOfDeath     = new Regex(GetRegexForField("PLACE OF DEATH"),    RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex regexSortKey = new Regex(@"\[\[\s*Category\s*:\s*[^\|\]]*\|\s*([^\]]*)", RegexOptions.IgnoreCase);

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
            pageList = new PageList(site);
            category = loginForm.Category;

            loadingDialog.Message = "Loading list of pages (up to 10000)...";
            Thread fillFromCategoryThread = new Thread(new ThreadStart(delegate()
            {
                pageList.FillAllFromCategoryEx(category, 10000);
                this.Invoke(new MethodInvoker(CompleteCategoryLoad));
            }));
            fillFromCategoryThread.Start();
        }

        void CompleteCategoryLoad()
        {
            loadingDialog.Hide();

            // Shuffle PageList in random order
            Random r = new Random();
            for (int i = pageList.Count() - 1; i >= 1; i--)
            {
                int j = r.Next(0, i + 1);
                Page temp = pageList[j];
                pageList[j] = pageList[i];
                pageList[i] = temp;
            }

            loadAheadThread = new Thread(new ThreadStart(LoadAhead));
            loadAheadThread.Start();
            savePendingThread = new Thread(new ThreadStart(SavePending));
            savePendingThread.Start();
            ShowNextPage();
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
            // Removing comment per consensus at [[Wikipedia_talk:Persondata#Can_we_stop_adding_the_annoying.2C_useless_comment_now.3F]]
            currentPageText = page.text.Replace(" <!-- Metadata: see [[Wikipedia:Persondata]]. -->", "");
            origPageText = page.text;
            textBoxTitle.Text = page.title;
            textBoxWikitext.Text = currentPageText.Replace("\n", "\r\n");

            Match templateMatch = regexTemplate.Match(currentPageText);
            currentTemplate = regexTemplate.Match(currentPageText).Value;
            currentTemplateIndex = templateMatch.Index;
            textBoxName.Text = regexName.Match(currentTemplate).Groups[2].Value.Trim();
            origName = textBoxName.Text;
            textBoxAlternativeNames.Text = regexAlternativeNames.Match(currentTemplate).Groups[2].Value.Trim();
            origAlternativeNames = textBoxAlternativeNames.Text;
            textBoxShortDescription.Text = regexShortDescription.Match(currentTemplate).Groups[2].Value.Trim();
            origShortDescription = textBoxShortDescription.Text;
            textBoxDateOfBirth.Text = regexDateOfBirth.Match(currentTemplate).Groups[2].Value.Trim();
            origDateOfBirth = textBoxDateOfBirth.Text;
            textBoxPlaceOfBirth.Text = regexPlaceOfBirth.Match(currentTemplate).Groups[2].Value.Trim();
            origPlaceOfBirth = textBoxPlaceOfBirth.Text;
            textBoxDateOfDeath.Text = regexDateOfDeath.Match(currentTemplate).Groups[2].Value.Trim();
            origDateOfDeath = textBoxDateOfDeath.Text;
            textBoxPlaceOfDeath.Text = regexPlaceOfDeath.Match(currentTemplate).Groups[2].Value.Trim();
            origPlaceOfDeath = textBoxPlaceOfDeath.Text;

            if (textBoxName.Text == "")
            {
                textBoxName.Text = GuessName(page);
            }

            manualEditSummary = false;

            UpdateEditSummary();

            UpdateFocus();
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

        private void UpdateFocus()
        {
            if (category == "Persondata templates without name parameter")
            {
                textBoxName.Focus();
            }
            else if (category == "Persondata templates without short description parameter")
            {
                textBoxShortDescription.Focus();
            }
        }

        void LoadAhead()
        {
            while(true)
            {
                bool shouldLoadNext;
                lock (lastLoadedPageIndexLock)
                {
                    shouldLoadNext = (lastLoadedPageIndex - (lastSavedPageIndex) - 1 < numLoadAheadPages);
                }
                if (shouldLoadNext)
                {
                    int nextPage = lastLoadedPageIndex + 1;
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

        private string UpdateEditSummaryHelper(string name, string origValue, string value)
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

        private void UpdateEditSummary()
        {
            if (manualEditSummary)
            {
                return;
            }

            List<string> list = new List<string>();
            list.Add(UpdateEditSummaryHelper("name", origName, textBoxName.Text));
            list.Add(UpdateEditSummaryHelper("alternative names", origAlternativeNames, textBoxAlternativeNames.Text));
            list.Add(UpdateEditSummaryHelper("short description", origShortDescription, textBoxShortDescription.Text));
            list.Add(UpdateEditSummaryHelper("date of birth", origDateOfBirth, textBoxDateOfBirth.Text));
            list.Add(UpdateEditSummaryHelper("place of birth", origPlaceOfBirth, textBoxPlaceOfBirth.Text));
            list.Add(UpdateEditSummaryHelper("date of death", origDateOfDeath, textBoxDateOfDeath.Text));
            list.Add(UpdateEditSummaryHelper("place of death", origPlaceOfDeath, textBoxPlaceOfDeath.Text));
            list.RemoveAll(delegate(string s) { return s.Length == 0; });

            if (list.Count == 0)
            {
                textBoxEditSummary.Text = "";
            }
            else
            {
                textBoxEditSummary.Text = "persondata: " + string.Join(", ", list.ToArray()) + " using [[Wikipedia:Persondata-o-matic|Persondata-o-matic]]";
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
        }

        private void textBoxAlternativeNames_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxAlternativeNames.Text, regexAlternativeNames);
        }

        private void textBoxShortDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxShortDescription.Text, regexShortDescription);
        }

        private void textBoxDateOfBirth_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxDateOfBirth.Text, regexDateOfBirth);
        }

        private void textBoxPlaceOfBirth_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxPlaceOfBirth.Text, regexPlaceOfBirth);
        }

        private void textBoxDateOfDeath_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxDateOfDeath.Text, regexDateOfDeath);
        }

        private void textBoxPlaceOfDeath_TextChanged(object sender, EventArgs e)
        {
            UpdateField(textBoxPlaceOfDeath.Text, regexPlaceOfDeath);
        }

        private void textBoxEditSummary_TextChanged(object sender, EventArgs e)
        {
            manualEditSummary = true;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            UpdateFocus();
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
                currentPageText = regexTemplate.Replace(currentPageText, "");
                if (currentPageText != origPageText)
                {
                    pageList[lastSavedPageIndex].text = currentPageText;
                    SaveInfo info = new SaveInfo();
                    info.Page = pageList[lastSavedPageIndex];
                    info.EditSummary = "Removing persondata (not a biographical article)";
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
