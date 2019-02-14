﻿using Dzaba.HomeAccounting.Windows.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dzaba.HomeAccounting.Windows.View
{
    /// <summary>
    /// Interaction logic for IncomeView.xaml
    /// </summary>
    public partial class IncomeView : UserControl
    {
        public IncomeView()
        {
            InitializeComponent();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            var vm = DataContext as IncomeViewModel;

            if (vm != null && row != null)
            {
                var item = row.Item;
                if (vm.ShowMonthCommand.CanExecute(item))
                {
                    vm.ShowMonthCommand.Execute(item);
                }
            }
        }
    }
}
