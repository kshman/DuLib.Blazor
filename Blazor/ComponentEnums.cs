namespace Du.Blazor;

/// <summary>
/// 컴포넌트 롤
/// </summary>
public enum ComponentRole
{
	// 기본
	Block,
	Text,
	Image,
	Link,

	// 특수
	Divide,
	Badge,

	// 콘텐트 
	Lead,
	Tail,
	Menu,
}


/// <summary>
/// 바리언트
/// </summary>
public enum Variant
{
	Normal,
	Splendid,
	Simple,
	Outline,
	Primary,
	Digital,
}


/// <summary>
/// 바리언트 리드
/// </summary>
public enum VarLead
{
	Set,
	Up,
	Down,
}


/// <summary>
/// 반응형 크기
/// </summary>
public enum Responsive
{
	Default,
	W6,
	W9,
	W12,
	W15,
	Full,
}

/// <summary>
/// 리스폰시브가 너무 길어서 축약판. <br/>
/// Red Mage가 아님 Responsive Dimension임
/// </summary>
public enum Rdm
{
	None,
	W6,
	W9,
	W12,
	W15,
}

// 그리드 열
public enum Rdc
{
	None, // 0
	Full, // 100
	Half, // 50
	OneThird, // 3분의 1 -> 33
	TwoThird, // 3분의 2 -> 66
	// 3분의 3은 Full
	OneFourth, // 4분의 1 -> 25
	// 4분의 2는 Half
	ThreeFourth, // 4분의 3 -> 75
	// 4분의 4는 Full
	OneFifth, // 5분의 1 -> 20
	TwoFifth, // 5분의 2 -> 40
	ThreeFifth, // 5분의 3 -> 60
	FourFifth, // 5분의 4 -> 80
	// 5분의 5는 Full
	OneSixth, // 6분의 1 -> 16
}
