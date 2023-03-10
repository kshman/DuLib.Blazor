using System.Diagnostics.CodeAnalysis;

namespace Du.Blazor.Supp;

internal static class TypeSupp
{
	#region 기본 타입
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

	internal static bool ShouldAwaitTask(this Task task) =>
		task.Status is not TaskStatus.RanToCompletion and not TaskStatus.Canceled;
	#endregion 기본 타입
}
