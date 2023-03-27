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
	Menu,
	Lead,
	Tail,
	Toggle,
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
	Dark,
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
/// 저스티파이
/// </summary>
public enum Justify
{
	Start,
	End,
	Center,
	SpaceBetween,
	SpaceAround,
	SpaceEvenly,
}

/// <summary>
/// 놓는 위치
/// </summary>
public enum Placement
{
	None,
	Top,
	Bottom,
	Start, // left
	End, // right
	Overlay,
}

/// <summary>
/// 그리드 갯수
/// </summary>
public enum GridCount
{
	C1 = 1,
	C2 = 2,
	C3 = 3,
	C4 = 4,
	C5 = 5,
	C6 = 6,
	C7 = 7,
	C8 = 8,
	C9 = 9,
	Cx = 10,
}

/// <summary>
/// 그리드 너비
/// </summary>
public enum GridWidth
{
	W1 = 1,
	W2 = 2,
	W3 = 3,
	W4 = 4,
	W5 = 5,
	W6 = 6,
	W7 = 7,
	W8 = 8,
	W9 = 9,
	Wx = 10,

	P33 = 33,
	P66 = 66,
	P25 = 25,
	P75 = 75,
	P16 = 16,

	Full = 10,
	Half = 5,
	Quarter = 25,

	OneThree = 33,
	TwoThree = 66,

	OneFour = 25,
	TwoFour = 50,
	ThreeFour = 75,

	OneSix = 16,
}
