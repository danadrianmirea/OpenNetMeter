﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using OpenNetMeter.Models;

namespace OpenNetMeter.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private readonly DataUsageSummaryVM dusvm;
        private readonly DataUsageDetailedVM dudvm;
        private readonly DataUsageHistoryVM duhvm;
        private readonly SettingsVM svm;
        private readonly NetworkProcess netProc;
        public ICommand SwitchTabCommand { get; set; }

        private int tabBtnToggle;
        public int TabBtnToggle
        {
            get { return tabBtnToggle; }
            set { tabBtnToggle = value; OnPropertyChanged("TabBtnToggle"); }
        }

        private object selectedViewModel;
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }
        public ulong downloadSpeed;
        public ulong DownloadSpeed
        {
            get { return downloadSpeed; }
            set { downloadSpeed = value; OnPropertyChanged("DownloadSpeed"); }
        }
        public ulong uploadSpeed;
        public ulong UploadSpeed
        {
            get { return uploadSpeed; }
            set { uploadSpeed = value; OnPropertyChanged("UploadSpeed"); }
        }

        private string networkStatus;
        public string NetworkStatus
        {
            get { return networkStatus; }
            set { networkStatus = value; OnPropertyChanged("NetworkStatus"); }
        }

        private enum TabPage
        {
            Summary,
            Detailed,
            History,
            Settings
        }

        public MainWindowVM(MiniWidgetVM mwvm_DataContext, ConfirmationDialogVM cD_DataContext) //runs once during app init
        {
            DownloadSpeed = 0;
            UploadSpeed = 0;

            //initialize pages, dusvm == 0, dudvm === 1, svm == 2
            dusvm = new DataUsageSummaryVM();
            duhvm = new DataUsageHistoryVM();
            dudvm = new DataUsageDetailedVM(cD_DataContext);
            svm = new SettingsVM();

            netProc = new NetworkProcess(dusvm, dudvm, this, mwvm_DataContext);
            //dudvm.SetNetProc(netProc);

            Database.Class1 class1 = new Database.Class1();

            //intial startup page

            TabBtnToggle = Properties.Settings.Default.LaunchPage;
            switch (TabBtnToggle)
            {
                case ((int)TabPage.Summary):
                    SelectedViewModel = dusvm;
                    break;
                case ((int)TabPage.Detailed):
                    SelectedViewModel = dudvm;
                    break;
                case ((int)TabPage.History):
                    SelectedViewModel = duhvm;
                    break;
                case ((int)TabPage.Settings):
                    SelectedViewModel = svm;
                    break;
                default:
                    SelectedViewModel = dusvm;
                    TabBtnToggle = ((int)TabPage.Summary);
                    break;
            }

            netProc.InitConnection();

            //assign basecommand
            SwitchTabCommand = new BaseCommand(SwitchTab);

        }

        private void SwitchTab(object obj)
        {
            string tab = obj as string;
            switch (tab)
            {
                case "summary":
                    if (TabBtnToggle != ((int)TabPage.Summary))
                    {
                        SelectedViewModel = dusvm;
                        TabBtnToggle = ((int)TabPage.Summary);
                        Properties.Settings.Default.LaunchPage = TabBtnToggle;
                        Properties.Settings.Default.Save();
                    }
                    break;
                case "detailed":
                    if (TabBtnToggle != ((int)TabPage.Detailed))
                    {
                        SelectedViewModel = dudvm;
                        TabBtnToggle = ((int)TabPage.Detailed);
                        Properties.Settings.Default.LaunchPage = TabBtnToggle;
                        Properties.Settings.Default.Save();
                    }
                    break;
                case "history":
                    if (TabBtnToggle != ((int)TabPage.History))
                    {
                        SelectedViewModel = duhvm;
                        TabBtnToggle = ((int)TabPage.History);
                        Properties.Settings.Default.LaunchPage = TabBtnToggle;
                        Properties.Settings.Default.Save();
                    }
                    break;
                case "settings":
                    if (TabBtnToggle != ((int)TabPage.Settings))
                    {
                        SelectedViewModel = svm;
                        TabBtnToggle = ((int)TabPage.Settings);
                        Properties.Settings.Default.LaunchPage = TabBtnToggle;
                        Properties.Settings.Default.Save();
                    }
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
