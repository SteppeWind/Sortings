using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RGRSortings
{
    //сессия тестов
    class Session
    {
        //список тестов
        public ObservableCollection<Test> ListTests { get; private set; }

        //кол-во тестов
        public int CountTests { get; private set; }

        //шаг теста (например 50, кол-во тестов - 5, то результатом будет 0,50,100,150,200)
        public int StepTest { get; private set; }

        //событие, которое уведомляет о завершении сессии тестов
        public event Action SessionEndedEvent;
        
        //конструктор, в него передаем кол-во тестов и шаг
        public Session(int countTests, int step)
        {
            ListTests = new ObservableCollection<Test>();
            CountTests = countTests;
            StepTest = step;
        }

        public async void StartSession()//запуск сессии
        {
            for (int i = 1; i <= CountTests; i++)
            {
                var test = new Test((i - 1) * StepTest, i);//создаем экземпляр классTest
                await test.StartTest();//запускаем и ожидаем выполнения теста
                ListTests.Add(test);//добавляем завершенный тест в список
            }
            SessionEndedEvent?.Invoke();//уведомляем о завершении
        }
    }
}