using System;
using System.Collections;
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
using BookstoreApp.ViewModel;
using Labb2_DbFirst_Template.Data;
using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.View
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        //private ShopRepository _storeRepository = new ShopRepository();

        //private BookRepository _bookRepository = new BookRepository();
        //StockViewModel StockViewModel = new StockViewModel();

        
        public StockView()
        {
            InitializeComponent();
            
        }

    }
}
