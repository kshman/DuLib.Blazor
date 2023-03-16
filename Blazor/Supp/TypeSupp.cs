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

	private static string ToCssName(this Variant v) => v switch
	{
		Variant.Normal => "nrm",
		Variant.Splendid => "spn",
		Variant.Simple => "sme",
		Variant.Outline => "oue",
		Variant.Primary => "pri",
		_ => LogIf.ArgumentOutOfRange<string>(v, nameof(v))
	};

	internal static string ToCss(this Variant v) =>
		$"var-{v.ToCssName()}";

	internal static string ToCss(this Variant v, string tail) =>
		$"var-{v.ToCssName()}-{tail}";
}
