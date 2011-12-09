using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Net.NetworkInformation;
using System.Net.NetworkInformation;




namespace PopcornRemoteTest1
{



    public partial class MainPanoramaPage : PhoneApplicationPage
    {
        private Settings AppSettings;
        
        public MainPanoramaPage()
        {
            InitializeComponent();
            AppSettings = (App.Current as App).AppSettings;

            LayoutRoot.DataContext = AppSettings;
            
            if (string.IsNullOrEmpty(AppSettings.ServerIP))
                pivotControl.SelectedItem = settingsPivot;
            
           
        }

        private Queue<string> commandqueue = new Queue<string>();
        private int QUEUESIZE = 100;
        private void AddCommand(string command)
        {
            if (commandqueue.Count < QUEUESIZE)
            {
                commandqueue.Enqueue(command);

                //commandList.Items.Add(new CommandInQueue());
                
                if (!waitingForCommand)
                    CallCommand();
            }
            else MessageBox.Show("Only " + QUEUESIZE + " commands can be buffered at a time. Please wait");
        }

        private bool waitingForCommand = false;

        private WebClient client;


        private void CallCommand()
        {
            
          //  if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) //Uitzetten?
            {
                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || AppSettings.Allow3G==true )
                {

                    if (commandqueue.Count > 0)
                    {
                        if (!waitingForCommand)
                        {
                            progressbar.IsIndeterminate = false;
                            client = new WebClient();
                            Uri uri;
                            if (!String.IsNullOrEmpty(AppSettings.ServerIP))
                            {

                                string uristring = @"http://" + AppSettings.ServerIP + ":" + AppSettings.Port + @"/c200remote_web/webrc200.php?"; 
                                string command = commandqueue.Dequeue();
                                if (command.Length == 1)
                                {
                                    uristring += "cmd=" + command;
                                }
                                else
                                {
                                    uristring += "fcmd=" + command;
                                }

                                uristring += "&r=" + Guid.NewGuid().ToString();
                                uri = new Uri(uristring);


                                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(sendCommandCompleted);
                                waitingForCommand = true;

                                progressbar.IsIndeterminate = true;
                                progressbar.Visibility = Visibility.Visible;


                                client.DownloadStringAsync(uri);

                            }
                            else
                            {
                                MessageBox.Show("No Popcorn URL was set. Please do this first through the settings page");
                                EmptyQueue();
                            }
                            //Beter: http://msdn.microsoft.com/en-us/library/0aa3d588.aspx 



                        }
                        else
                        {
                            MessageBox.Show("Still waiting for result of previous call");
                            EmptyQueue();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Wifi connection available. See settings page if you wish to use your 3G-connection");
                    EmptyQueue();
                }
            }
            //else
            //{
            //    MessageBox.Show("No network connection available.");
            //    EmptyQueue();
            //}

            
        }

        private void EmptyQueue()
        {
            commandqueue.Clear();
            //commandList.Children.Clear();
        }


        void sendCommandCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            // progressbar.IsIndeterminate = false;
            // pivotControl.IsEnabled = true;

            waitingForCommand = false;

            //commandList.Children.RemoveAt(0);
            try
            {
                string result = (string)e.Result;
                if (e.Error == null)
                {
                    if(!result.Contains("Success, redirecting back to remote control!"))
                    {
                        if (result.Contains("something is wrong"))
                            MessageBox.Show("The remote server responded and the c200remote service is installed. However, the Popcorn Hour Device did not respond to the request.");
                        else
                            MessageBox.Show("The remote server responded but no c200remote service is installed there");

                    }
                       
                }
                else
                {
                    MessageBox.Show(e.Error.Message);
                }

                CallCommand();
  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                commandqueue.Clear();
                //commandList.Children.Clear();
            }

            if (commandqueue.Count == 0)
            {
                progressbar.IsIndeterminate = false;
                progressbar.Visibility = Visibility.Collapsed;

            }



        }

        private void btnCommand_Click(object sender, RoutedEventArgs e)
        {
            AddCommand((string)(((Button)(sender)).Tag));
        }

        private void sendTextBtn_Click(object sender, RoutedEventArgs e)
        {
            char[] command = txbToSend.Text.ToLower().ToCharArray();
            //Controleren op legale input
            foreach (var chartosend in command)
            {

                bool fail = false;
                #region switch
                switch (chartosend)
                {

                    #region Button 2
                    case 'a':
                        AddCommand("2");
                        break;
                    case 'b':
                        AddCommand("2");
                        goto case 'a';
                    case 'c':
                        AddCommand("2");
                        goto case 'b';
                    #endregion
                    #region Button 3
                    case 'd':
                        AddCommand("3");
                        break;
                    case 'e':
                        AddCommand("3");
                        goto case 'd';
                    case 'f':
                        AddCommand("3");
                        goto case 'e';
                    #endregion
                    #region Button 4
                    case 'g':
                        AddCommand("4");
                        break;
                    case 'h':
                        AddCommand("4");
                        goto case 'g';
                    case 'i':
                        AddCommand("4");
                        goto case 'h';
                    #endregion
                    #region Button 5
                    case 'j':
                        AddCommand("5");
                        break;
                    case 'k':
                        AddCommand("5");
                        goto case 'j';
                    case 'l':
                        AddCommand("5");
                        goto case 'k';
                    #endregion
                    #region Button 6
                    case 'm':
                        AddCommand("6");
                        break;
                    case 'n':
                        AddCommand("6");
                        goto case 'm';
                    case 'o':
                        AddCommand("6");
                        goto case 'n';
                    #endregion
                    #region Button 7
                    case 'p':
                        AddCommand("7");
                        break;
                    case 'q':
                        AddCommand("7");
                        goto case 'p';
                    case 'r':
                        AddCommand("7");
                        goto case 'q';
                    case 's':
                        AddCommand("7");
                        goto case 'r';
                    #endregion
                    #region Button 8
                    case 't':
                        AddCommand("8");
                        break;
                    case 'u':
                        AddCommand("8");
                        goto case 't';
                    case 'v':
                        AddCommand("8");
                        goto case 'u';

                    #endregion
                    #region Button 9
                    case 'w':
                        AddCommand("9");
                        break;
                    case 'x':
                        AddCommand("9");
                        goto case 'w';
                    case 'y':
                        AddCommand("9");
                        goto case 'x';
                    case 'z':
                        AddCommand("9");
                        goto case 'y';
                    #endregion

                    default:
                        if (Char.IsNumber(chartosend))
                        {
                            if (chartosend == '1') AddCommand("1");
                            else if (chartosend == '2') CallMultipleCommand('2', 4);
                            else if (chartosend == '3') CallMultipleCommand('3', 4);
                            else if (chartosend == '4') CallMultipleCommand('4', 4);
                            else if (chartosend == '5') CallMultipleCommand('5', 4);
                            else if (chartosend == '6') CallMultipleCommand('6', 4);
                            else if (chartosend == '7') CallMultipleCommand('7', 5);
                            else if (chartosend == '8') CallMultipleCommand('8', 4);
                            else if (chartosend == '9') CallMultipleCommand('9', 5);
                            else if (chartosend == '0') AddCommand("0");
                        }
                        else if (chartosend == '.') CallMultipleCommand('1', 2);
                        else if (chartosend == '/') CallMultipleCommand('1', 3);
                        else if (chartosend == ',') CallMultipleCommand('1', 4);
                        else if (chartosend == ':') CallMultipleCommand('1', 5);
                        else if (chartosend == '-') CallMultipleCommand('1', 6);
                        else if (chartosend == '_') CallMultipleCommand('1', 7);
                        else if (chartosend == '?') CallMultipleCommand('1', 8);
                        else if (chartosend == '!') CallMultipleCommand('1', 9);
                        else if (chartosend == '\'') CallMultipleCommand('1', 10);
                        else if (chartosend == '@') CallMultipleCommand('1', 11);
                        else if (chartosend == ';') CallMultipleCommand('1', 12);
                        else if (chartosend == '(') CallMultipleCommand('1', 13);
                        else if (chartosend == ')') CallMultipleCommand('1', 14);
                        else if (chartosend == '[') CallMultipleCommand('1', 15);
                        else if (chartosend == ']') CallMultipleCommand('1', 16);
                        else if (chartosend == '$') CallMultipleCommand('1', 17);
                        else if (chartosend == '=') CallMultipleCommand('1', 18);
                        else if (chartosend == '%') CallMultipleCommand('1', 19);
                        else if (chartosend == '#') CallMultipleCommand('1', 20);
                        else
                        {
                            fail = true;
                            MessageBox.Show("Charact " + chartosend.ToString() + " not supported.");
                        }


                        break;
                }
                if (fail == false)
                {
                    AddCommand("right");
                }
                #endregion

            }

        }

        private void CallMultipleCommand(char p, int p_2)
        {

            for (int i = 0; i < p_2; i++)
            {

                AddCommand(new string(p, 1));

            }
            AddCommand("right");
        }


        private void btnPowerDown_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you certain?", "Shutdown/powerup?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                AddCommand("x");
        }

        //P311 e.v. van handboek ivm dataconnectivuty



    }
}