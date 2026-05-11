using Calculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.ViewModels
{
    // [ObservableObject] → 이 클래스가 View와 데이터를 주고받을 수 있게 해줌
    // partial → CommunityToolkit이 코드를 자동생성하려면 반드시 필요!
    [ObservableObject]
    public partial class CalculatorViewModel
    {
        private readonly CalculatorModel _calculatorModel;

        public CalculatorViewModel(CalculatorModel calculatorModel)
        {
            _calculatorModel = calculatorModel;
        }


        // [ObservableProperty] → 값이 바뀌면 자동으로 View에 알려줌
        // private string _display → 자동으로 public string Display 프로퍼티 생성
        // View의 TextBlock Text 와 바인딩될 현재 입력값
        [ObservableProperty]
        private string _display = "0";


        // View의 TextBlock Text 와 바인딩될 이전 입력값
        // ex) "5 +" 처럼 이전 숫자 + 연산자 표시
        [ObservableProperty]
        private string _previousDisplay = string.Empty;


        [RelayCommand]
        private void NumberInput(string number)
        {
            // = 눌러서 계산 완료된 상태에서 숫자 누르면
            // 새로운 계산 시작
            if (_calculatorModel.IsCalculated == true)
            {
                _calculatorModel.CurrentInput = string.Empty;
                _calculatorModel.IsCalculated = false;
            }

            //소수점 중복 방지
            // ex) 3.1.4 이렇게 입력 못하게 막으려고
            if (number == "." && _calculatorModel.CurrentInput.Contains("."))
            {
                return;
            }

            // 현재 입력값 뒤에 숫자 붙이기
            // ex) "12" + "3" = "123"
            _calculatorModel.CurrentInput = _calculatorModel.CurrentInput + number;

            //화면 업데이트
            Display = _calculatorModel.CurrentInput;
        }

        // =============================================
        // 연산자 버튼 눌렀을때 (+, -, *, /)
        // =============================================
        [RelayCommand]
        private void OperatorInput(string operatorSymbol)
        {
            //현재 입력값을 이전 입력값으로 저장
            _calculatorModel.PreviousInput = _calculatorModel.CurrentInput;

            //연산자 저장
            _calculatorModel.Operator = operatorSymbol;

            // 현재 입력값 초기화 (다음 숫자 입력 받으려고)
            _calculatorModel.CurrentInput = string.Empty;

            //이전 입력 표시 업데이트
            // ex) "5 +" 이렇게 보여줌
            PreviousDisplay = _calculatorModel.PreviousInput + " " + _calculatorModel.Operator;

            //계산 완료 상태 초기화
            //방어 차원에서 혹시모를 상황에 대비해서 false로 초기화
            _calculatorModel.IsCalculated = false;
        }

        // =============================================
        // = 버튼 눌렀을때
        // =============================================
        [RelayCommand]
        private void Calculate()
        {
            // 연산자나 이전값이 없으면 계산 안함
            if (_calculatorModel.Operator == string.Empty || _calculatorModel.PreviousInput == string.Empty)
            {
                return;
            }

            //Model의 Calculate 메서드 호출해서 결과 받기
            string result = _calculatorModel.Calculate();


            //이전 표시 업데이트
            // ex) "5 + 3 = " 
            PreviousDisplay = _calculatorModel.PreviousInput + " " + _calculatorModel.Operator + " " + _calculatorModel.CurrentInput + " =";

            //화면에 결과 표시
            Display = result;

            //결과값을 현재 입력값으로 저장
            //다음 계산에서 이 결과값을 사용하려고
            _calculatorModel.CurrentInput = result;
            _calculatorModel.IsCalculated = true;
        }

        // =============================================
        // C 버튼 눌렀을때 (전체 초기화)
        // =============================================
        [RelayCommand]
        private void Reset()
        {
            // Model 초기화
            _calculatorModel.Reset();

            // 화면 초기화
            Display = "0";
            PreviousDisplay = string.Empty;
        }

        // =============================================
        // ← 버튼 눌렀을때 (마지막 글자 지우기)
        // =============================================
        [RelayCommand]
        private void Backspace()
        {
            // 계산 완료된 상태면 backspace 무시
            if (_calculatorModel.IsCalculated == true)
            {
                return;
            }

            // 현재 입력값이 없으면 무시
            if(_calculatorModel.CurrentInput.Length == 0)
            {
                return;
            }

            // 마지막 글지 지우기
            // ex) "123" → "12"
            // Substring(0, Length-1) → 처음부터 마지막 글자 전까지만 가져옴
            _calculatorModel.CurrentInput= _calculatorModel.CurrentInput.Substring(0, _calculatorModel.CurrentInput.Length - 1);

            // 다 지워지면 "0"으로 표시
            if ( _calculatorModel.CurrentInput.Length ==0)
            {
                Display = "0";
            }else
            {
                Display = _calculatorModel.CurrentInput;
            }
        }

        // =============================================
        // % 버튼 눌렀을때 (퍼센트 계산)
        // =============================================
        [RelayCommand]
        private void Percent()
        {
            //현재 입력값이 없으면 무시
            if(_calculatorModel.CurrentInput ==string.Empty)
            {
                return;
            }

            //문자열을 숫자로 변환
            bool success = double.TryParse(_calculatorModel.CurrentInput, out double current);

            if (success == false)
            {
                return;
            }

            // 100으로 나눠서 퍼센트 계산
            // ex) 50 → 0.5
            double result = current / 100;

            // 결과를 현재 입력값으로 저장
            _calculatorModel.CurrentInput = result.ToString("G15");

            // 화면 업데이트
            Display = _calculatorModel.CurrentInput;
        }
    }
}


// =============================================
// C# MVVM Command 동작 순서
//
// 1. View 에서 버튼 클릭
//    <Button Command="{Binding NumberInputCommand}"/>
//
// 2. NumberInputCommand (public) 실행
//    → CommunityToolkit 이 [RelayCommand] 로 자동생성한 것
//    → public ICommand NumberInputCommand { get; }
//
// 3. NumberInput (private) 메서드 실행
//    → Command 가 내부적으로 메서드를 호출
//    → 메서드를 직접 호출하는게 아니라
//      Command 를 통해서 실행되기 때문에 private 가능!
//
// Java 와 비교
// Java   → 버튼 클릭 → 메서드 직접 호출 → public 필요
// C# MVVM→ 버튼 클릭 → Command → 메서드 → private 가능
// =============================================