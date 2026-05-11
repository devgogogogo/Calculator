using Calculator.Models;
using Calculator.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // OnStartup → 앱이 시작될때 자동으로 실행되는 메서드
        // Java 의 main() 메서드랑 비슷한 역할

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Spring Bean등록으로 생각하면됨
            //Model 객체 생성
            CalculatorModel calculatorModel = new CalculatorModel();

            // ViewModel 생성 → Model 을 생성자로 넣어줌 (의존성 주입!)
            CalculatorViewModel calculatorViewModel = new CalculatorViewModel(calculatorModel);

            // View(창) 생성
            MainWindow mainWindow = new MainWindow();

            // DataContext → View 가 바라볼 ViewModel 지정
            // 이게 연결되어야 바인딩이 동작
            mainWindow.DataContext = calculatorViewModel;

            //창 띄우기
            mainWindow.Show();
        }
    }
}
