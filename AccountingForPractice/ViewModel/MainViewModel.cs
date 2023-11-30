using AccountingForPractice.View;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AccountingForPractice.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private Page WoW = new WoWPage();
        private Page HS = new HSPage();
        private Page OW = new OwerwatchPage();
        private Page W3 = new W3Page();
        private Page D3 = new D3Page();
        private Page HOTS = new HOTSPage();
        private Page SC2 = new SC2Page();
        private Page SC = new SCPage();
        private Page _CurPage = new TestDBPage();
        

        private Page BD = new TestDBPage();
        private Page AddSpecialties = new AddSpecialtiesPage();
        private Page AddGroup = new AddGroupPage();
        private Page AddModules = new AddModulesPage();
        private Page AddCompany = new AddBankingDetailsPage();

        public Page curPage
        {
            get => _CurPage;
            set => Set(ref _CurPage, value);
        }

        public ICommand OpenAddCompany
        {
            get
            {
                return new RelayCommand(() => curPage = AddCompany);
            }
        }

        public ICommand OpenAddModules
        {
            get
            {
                return new RelayCommand(() => curPage = AddModules);
            }
        }

        public ICommand OpenAddGroup
        {
            get
            {
                return new RelayCommand(() => curPage = AddGroup);
            }
        }

        public ICommand OpenAddSpecialties
        {
            get
            {
                return new RelayCommand(() => curPage = AddSpecialties);
            }
        }

        public ICommand OpenBDPage
        {
            get
            {
                return new RelayCommand(() => curPage = BD);
            }
        }

        public ICommand OpenWoWPage
        {
            get
            {
                return new RelayCommand(() => curPage = WoW);
            }
        }

        public ICommand OpenHSPage
        {
            get
            {
                return new RelayCommand(() => curPage = HS);
            }
        }

        public ICommand OpenOWPage
        {
            get
            {
                return new RelayCommand(() => curPage = OW);
            }
        }

        public ICommand OpenW3Page
        {
            get
            {
                return new RelayCommand(() => curPage = W3);
            }
        }

        public ICommand OpenD3Page
        {
            get
            {
                return new RelayCommand(() => curPage = D3);
            }
        }

        public ICommand OpenHOTSPage
        {
            get
            {
                return new RelayCommand(() => curPage = HOTS);
            }
        }

        public ICommand OpenSC2Page
        {
            get
            {
                return new RelayCommand(() => curPage = SC2);
            }
        }

        public ICommand OpenSCPage
        {
            get
            {
                return new RelayCommand(() => curPage = SC);
            }
        }
    }
}
