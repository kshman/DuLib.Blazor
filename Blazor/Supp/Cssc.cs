namespace Du.Blazor.Supp;

/// <summary>
/// CSS 클래스 만들어 주기 (Css composer)
/// </summary>
internal static class Cssc
{
	private const char class_separator = ' ';
	private const char style_separator = ';';

	/// <summary>
	/// 분리자로 합진다
	/// </summary>
	/// <param name="separator"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	private static string? Join(char separator, params string?[] args)
	{
		var j = string.Join(separator, args.Where(x => x.TestHave(true)));
		return j.Length == 0 ? null : j;
	}

	/// <summary>
	/// CSS 클래스로 합친다
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public static string? Class(params string?[] args) =>
		Join(class_separator, args);

	/// <summary>
	/// CSS 스타일로 합친다
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public static string? Style(params string?[] args) =>
		Join(style_separator, args);
}
