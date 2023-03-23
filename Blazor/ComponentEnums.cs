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
