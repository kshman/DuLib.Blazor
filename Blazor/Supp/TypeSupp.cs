using System.Diagnostics.CodeAnalysis;

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

	internal static string ToCss(this Variant variant, VarLead lead = VarLead.Set)
	{
		// s = 전체 세트
		// d = 낮은색 (default)
		// w = 밝은색 (active)
		var l = lead switch
		{
			VarLead.Set => "vk",
			VarLead.Down => "vd",
			VarLead.Up => "vw",
			_ => LogIf.ArgumentOutOfRange<string>(lead, nameof(lead))
		};
		var v = variant switch
		{
			Variant.Normal => null,
			Variant.Splendid => "vssp",
			Variant.Simple => "vsse",
			Variant.Outline => "vsoe",
			Variant.Primary => "vspr",
			Variant.Digital => "vsdt",
			Variant.Dark => "vsdk",
			_ => LogIf.ArgumentOutOfRange<string>(variant, nameof(variant))
		};
		return v == null ? l : $"{l} {v}";
	}

	internal static string ToCss(this Responsive e) => e switch
	{
		Responsive.Default => "lsp",
		Responsive.W6 => "lsp6",
		Responsive.W9 => "lsp9",
		Responsive.W12 => "lsp12",
		Responsive.W15 => "lsp15",
		Responsive.Full => "lspf",
		_ => LogIf.ArgumentOutOfRange<string>(e, nameof(e))
	};

	internal static string ToCssNavBar(this Responsive e) => e switch
	{
		Responsive.W6 => "cnvb6",
		Responsive.W9 => "cnvb9",
		Responsive.W12 => "cnvb12",
		Responsive.W15 => "cnvb15",
		_ => LogIf.ArgumentOutOfRange<string>(e, nameof(e))
	};

	internal static string ToCss(this Justify j) => j switch
	{
		Justify.Start => "sjcs",
		Justify.End => "sjce",
		Justify.Center => "sjcc",
		Justify.SpaceBetween => "sjceb",
		Justify.SpaceAround => "sjcea",
		Justify.SpaceEvenly => "sjcee",
		_ => LogIf.ArgumentOutOfRange<string>(j, nameof(j))
	};

	internal static string? ToCssMarginAuto(this Placement? p) => p switch
	{
		null => null,
		Placement.None => null,
		Placement.Top => "smta",
		Placement.Bottom => "smba",
		Placement.Start => "smsa",
		Placement.End => "smea",
		Placement.Overlay => null,
		_ => LogIf.ArgumentOutOfRange<string>(p, nameof(p))
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
