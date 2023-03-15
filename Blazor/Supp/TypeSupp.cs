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

	private static string ToCssName(this Variant v) =>
		v.ToString("F").ToLower();

	internal static string ToCss(this Variant v, string lead = "variant") =>
		$"{lead}-{v.ToCssName()}";

	internal static string ToCss(this TagVariant v) => v switch
	{
		TagVariant.Primary => "pri",
		TagVariant.Success => "suc",
		TagVariant.Danger => "dng",
		TagVariant.Warning => "wrn",
		TagVariant.Info => "inf",
		TagVariant.Light => "lgt",
		TagVariant.Dark => "drk",
		_ => LogIf.ArgumentOutOfRange<string>(v, nameof(v))
	};

	internal static string ToCss(this TagVariant v, string lead) =>
		$"{lead}-{v.ToCss()}";

	private static string ToCss(this TagRound r) => r switch
	{
		TagRound.Circle => "c",
		TagRound.Pill => "p",
		_ => LogIf.ArgumentOutOfRange<string>(r, nameof(r))
	};

	internal static string ToCss(this TagRound r, string lead) =>
		$"{lead}-{r.ToCss()}";
}
