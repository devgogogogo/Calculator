namespace Calculator.Models
{
    public class CalculatorModel
    {

        //현재 입력중인 숫자 저장
        // ex) 1,2,3 순서로 누르면 → "123"
        public string CurrentInput { get; set; } = string.Empty;

        // 이전에 입력한 숫자 저장
        // ex) "5 + 3" 에서 + 누르는 순간 → PreviousInput = "5"
        public string PreviousInput { get; set; } = string.Empty;

        // 선택한 연산자 저장 (+, -, *, /)
        public string Operator { get; set; } = string.Empty;


        // 계산이 끝난 상태인지 체크
        // = 버튼 눌러서 결과 나온 상태 → true
        // 숫자 입력중인 상태 → false
        public bool IsCalculated { get; set; } = false;


        public string Calculate()
        {
            // 문자열을 숫자로 변환
            // TryParse → 변환 실패해도 앱이 죽지 않음
            // Parse    → 변환 실패하면 앱이 죽음 (사용 X)
            bool previousSuccess = double.TryParse(PreviousInput, out double previous);
            bool currentSuccess = double.TryParse(CurrentInput, out double current);

            if (previousSuccess == false || currentSuccess == false)
            {
                return "Error";
            }

            //계산 결과를 저장할 변수
            double result = 0;

            // 연산자에 따라 계산
            if (Operator == "+")
            {
                result = previous + current;
            }
            else if (Operator == "-")
            {
                result = previous - current;
            }
            else if (Operator == "*")
            {
                result = previous * current;
            }
            else if (Operator == "/")
            {
                // 0으로 나누면 오류나니까 예외처리 필수!
                if (current == 0)
                {
                    return "Error";
                }
                result = previous / current;
            }
            else
            {
                result = 0;
            }
            // G15 유효숫자 15자리까지 표시 (double의 최대 유효숫자)
            // 3.14159265358979323846...
            //→ G15 적용
            //→ 3.14159265358979   ← 유효숫자 15개!

            // 12345.0
            //→ G15 적용
            //→ 12345              ← 뒤에 불필요한 .0 제거

            // 0.30000000000000004
            //→ G15 적용
            //→ 0.3                ← 오차 부분 잘려나감
            return result.ToString("G15");

        }
        // C 버튼 눌렀을때 전체 초기화
        public void Reset()
        {
            CurrentInput = string.Empty;
            PreviousInput = string.Empty;
            Operator= string.Empty;
            IsCalculated = false;
        }
    }
}
