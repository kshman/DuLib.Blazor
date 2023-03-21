using System.Diagnostics.CodeAnalysis;
using Du.Blazor.Components;

namespace Du.Blazor.Supp;

internal static class TypeSupp
{
	internal static bool Empty([NotNullWhen(false)] this string? s) =>
		string.IsNullOrEmpty(s);

	internal static bool WhiteSpace([NotNullWhen(false)] this string? s) =>
		string.IsNullOrWhiteSpace(s);

	internal static bool TestHave([NotNullWhen(true)] this string? s, bool testSpace = false) =>
		testSpace ? !string.IsNullOrWhiteSpace(s) : !string.IsNullOrEmpty(s);

	internal static string? IfTrue(this bool condition, string? value) =>
		condition ? value : null;

	internal static string? IfFalse(this bool condition, string? value) =>
		condition ? null : value;

	internal static string ToHtml(this bool b) =>
		b ? "true" : "false";

	internal static bool ShouldAwaitTask(this Task task) =>
		task.Status is not TaskStatus.RanToCompletion and not TaskStatus.Canceled;

	private static string ToCssDesc(this Variant v) => v switch
	{
		Variant.Normal => "nrm",
		Variant.Splendid => "spn",
		Variant.Simple => "sme",
		Variant.Outline => "oue",
		Variant.Primary => "pri",
		Variant.Digital => "dgt",
		_ => LogIf.ArgumentOutOfRange<string>(v, nameof(v))
	};

	internal static string ToCss(this Variant v, VrtLead lead = VrtLead.Set)
	{
		// s = 전체 세트
		// d = 낮은색 (default)
		// u = 밝은색 (active)
		var l = lead switch
		{
			VrtLead.Set => 's',
			VrtLead.Down => 'd',
			VrtLead.Up => 'u',
			_ => LogIf.ArgumentOutOfRange<char>(lead, nameof(lead))
		};
		return $"v{l}{v.ToCssDesc()}";
	}

	internal static string ToCss(this LayoutExpand e) => e switch
	{
		LayoutExpand.Default => "lcn",
		LayoutExpand.W6 => "lcn6",
		LayoutExpand.W9 => "lcn9",
		LayoutExpand.W12 => "lcn12",
		LayoutExpand.Full => "lcnf",
		_ => LogIf.ArgumentOutOfRange<string>(e, nameof(e))
	};

    internal static string ToCssNavBar(this LayoutExpand e) => e switch
    {
        LayoutExpand.W6 => "cnvb6",
        LayoutExpand.W9 => "cnvb9",
        LayoutExpand.W12 => "cnvb12",
        _ => LogIf.ArgumentOutOfRange<string>(e, nameof(e))
    };
}


// 눌러 타입
internal enum NuloType
{
	Link,
	Action,
	Button,
	Submit,
}

// 바리언트 리드
internal enum VrtLead
{
	Set,
	Down,
	Up,
}
