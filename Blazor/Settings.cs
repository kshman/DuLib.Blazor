namespace Du.Blazor;

/// <summary>
/// 블레이저 설정
/// </summary>
public static class Settings
{
	/// <summary>로컬 메시지를 표시하능가</summary>
	public static bool UseLocaleMesg { get; set; } = true;
	/// <summary>로그 출력할 때 예외 처리하는가</summary>
	public static bool ThrowOnLog { get; set; } = false;
}
