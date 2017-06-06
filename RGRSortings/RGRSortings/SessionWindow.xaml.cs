using RGRSortings.Grapgics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace RGRSortings
{
    /// <summary>
    /// Логика взаимодействия для SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        Session CurrentSession;

        public SessionWindow()
        {
            InitializeComponent();
            CurrentSession = new Session(10, 50);//создаем сессию с 10ю тестами, с шагом 50
            CurrentSession.SessionEndedEvent += CurrentSession_SessionEndedEvent;//подписываемся на событие о завершении теста
            CurrentSession.StartSession();//запускаем сессию
        }

        private void CurrentSession_SessionEndedEvent()
        {
            AnalysisResultDataGrid.ItemsSource = CurrentSession.ListTests;//результат теста записывем в AnalysisResultDataGrid свойству ItemsSource
             WaitAnalysis.Visibility = Visibility.Collapsed;//зеленую полосу делаем невидимой
        }

    }
}