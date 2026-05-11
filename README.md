# 🧮 Calculator

## 📌 프로젝트 소개

> WPF + MVVM 패턴으로 구현한 계산기 애플리케이션입니다.
> MES/스마트팩토리 개발자 취업을 목표로 실무에서 사용하는 기술 스택을 적용했습니다.
> 숫자 입력 → 연산자 선택 → 계산 결과까지의 흐름을 MVVM 패턴으로 구현했습니다.

## 📌 목차

### 프로젝트 개요
- [① 프로젝트 소개](#-프로젝트-소개)
- [② 개발 기간](#-개발-기간)
- [③ 기술 스택](#-기술-스택)
- [④ 프로젝트 구조](#-프로젝트-구조)

### UI & 기능 소개
- [⑤ 주요 기능](#-주요-기능)
- [⑥ 화면 구성](#-화면-구성)

### 기타
- [⑦ 트러블슈팅](#-트러블슈팅)
- [⑧ 실행 방법](#-실행-방법)

## 📅 개발 기간

2026.05 ~ 2026.05

## 🛠 기술 스택

| 분류 | 기술 |
|------|------|
| UI | WPF (.NET 8) |
| 아키텍처 | MVVM 패턴 |
| MVVM 라이브러리 | CommunityToolkit.Mvvm |
| 버전 관리 | Git / GitHub |

## 📁 프로젝트 구조
```
Calculator/
├── Models/
│   └── CalculatorModel.cs       # 계산 로직 및 데이터
├── ViewModels/
│   └── CalculatorViewModel.cs   # View와 Model 연결
├── Views/
│   └── (추후 확장 예정)
├── MainWindow.xaml              # 메인 화면
└── App.xaml.cs                  # 앱 시작점 & 의존성 주입
```
## ✅ 주요 기능

- WPF + MVVM 패턴으로 UI 와 비즈니스 로직 분리
- CommunityToolkit.Mvvm 으로 ObservableProperty, RelayCommand 적용
- 의존성 주입(DI) 으로 Model 과 ViewModel 느슨하게 연결

### 1. 숫자 입력
- 0~9 숫자 버튼 클릭 시 화면에 순서대로 표시
- 소수점 중복 입력 방지 (3.1.4 입력 불가)
- 계산 완료 후 숫자 입력 시 새로운 계산 시작

### 2. 사칙연산
- +, -, ×, ÷ 연산자 버튼 지원
- 연산자 클릭 시 이전 입력값 + 연산자 상단 표시
- 0으로 나누기 시 Error 표시

### 3. 기능 버튼
- C 버튼 → 전체 초기화
- ← 버튼 → 마지막 글자 지우기
- % 버튼 → 퍼센트 계산 (ex. 50 → 0.5)
- = 버튼 → 계산 결과 표시

## 📷 화면 구성

> 스크린샷 추가 예정

| 화면 | 설명 |
|------|------|
| MainWindow | 계산기 메인 화면 |

## 💡 트러블슈팅

### 1. string 초기값 미설정으로 인한 NullReferenceException

- **문제** : Calculate() 메서드에서 TryParse 실행 시 NullReferenceException 발생
- **원인** : string 타입의 기본값은 null 이라서
  초기값 없이 선언하면 null 인 상태로 TryParse 에 전달됨
- **해결** : string 프로퍼티에 string.Empty 로 초기값 지정

```csharp
// 변경 전 — 초기값 없음 (null 위험)
public string CurrentInput { get; set; }

// 변경 후 — 초기값 지정 (안전)
public string CurrentInput { get; set; } = string.Empty;
```

### 2. [RelayCommand] 없이 메서드 선언해서 바인딩 안되는 문제

- **문제** : 버튼 클릭해도 아무 반응이 없음
- **원인** : OperatorInput, Backspace 메서드에
  [RelayCommand] 어트리뷰트를 빠트려서
  Command 가 자동생성되지 않아 바인딩 연결 안됨
- **해결** : 모든 Command 로 사용할 메서드에 [RelayCommand] 추가

```csharp
// 변경 전 — [RelayCommand] 없음 (바인딩 안됨)
private void OperatorInput(string operatorSymbol) { }

// 변경 후 — [RelayCommand] 추가 (정상)
[RelayCommand]
private void OperatorInput(string operatorSymbol) { }
```

### 3. partial 키워드 없이 [ObservableObject] 사용해서 오류

- **문제** : [ObservableProperty] 사용 시 컴파일 오류 발생
- **원인** : CommunityToolkit 소스제너레이터가
  코드를 자동생성하려면 partial 클래스여야 함
- **해결** : 클래스 선언에 partial 키워드 추가

```csharp
// 변경 전 — partial 없음 (오류)
public class CalculatorViewModel { }

// 변경 후 — partial 추가 (정상)
public partial class CalculatorViewModel { }
```

## 🚀 실행 방법

1. Visual Studio 2022 이상 설치 (.NET 8 포함)
2. 프로젝트 클론
```bash
git clone https://github.com/깃허브아이디/Calculator.git
```
3. Visual Studio 에서 `Calculator.sln` 열기
4. NuGet 패키지 복원
   - 솔루션 탐색기에서 프로젝트 우클릭 → `NuGet 패키지 복원`
   - 또는 Visual Studio 가 자동으로 복원 제안
5. 빌드 후 실행 (`F5`)
