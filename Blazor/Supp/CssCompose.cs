namespace Du.Blazor.Supp;

/// <summary>
/// CSS 클래스 만들어 주기
/// </summary>
public class CssCompose
{
	private const char class_separator = ' ';
	private const char style_separator = ';';

	private readonly List<string> _sts = new();
	private readonly List<Func<string?>> _fns = new();

	public CssCompose Set(string className)
	{
		_sts.Add(className);
		return this;
	}

	public CssCompose Add(string? className)
	{
		if (className.IsHave())
			_sts.Add(className);
		return this;
	}

	public CssCompose Add(bool condition, string? trueName)
	{
		if (condition && trueName.IsHave())
			_sts.Add(trueName);
		return this;
	}

	public CssCompose Add(bool condition, string trueName, string falseName)
	{
		_sts.Add(condition ? trueName : falseName);
		return this;
	}

	public CssCompose Register(Func<string?> classFunc)
	{
		_fns.Add(classFunc);
		return this;
	}

	public bool Test(string className) 
		=> _sts.Contains(className);

	private string? InternalJoin(char separator)
	{
		var s = string.Join(separator, _sts);
		var f = string.Join(separator, _fns.Select(x => x()).Where(g => g.IsHave()));
        return s.Length == 0 ? f.Length == 0 ? null : f : f.Length == 0 ? s : $"{s}{separator}{f}";
	}

	/// <summary>만들어진 CSS 클래스</summary>
	public string? Class =>
		InternalJoin(class_separator);

	/// <summary>만들어진 CSS 스타일</summary>
	public string? Style =>
		InternalJoin(style_separator);

	public override string ToString() => $"({_sts.Count}/{_fns.Count}) {Class}";

	public static string? Join(char separator, params string?[] args)
	{
		var j = string.Join(separator, args.Where(x => x.IsHave()));
		return j.Length == 0 ? null : j;
	}

	public static string? Join(params string?[] args) =>
		Join(class_separator, args);
}
