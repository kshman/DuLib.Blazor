﻿using System.Diagnostics.CodeAnalysis;
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

	internal static string ToCss(this Variant v, VarLead lead = VarLead.Set)
	{
		// s = 전체 세트
		// d = 낮은색 (default)
		// u = 밝은색 (active)
		var l = lead switch
		{
			VarLead.Set => 's',
			VarLead.Down => 'd',
			VarLead.Up => 'u',
			_ => LogIf.ArgumentOutOfRange<char>(lead, nameof(lead))
		};
		return $"v{l}{v.ToCssDesc()}";
	}

	internal static string ToCss(this Responsive e) => e switch
	{
		Responsive.Default => "lcn",
		Responsive.W6 => "lcn6",
		Responsive.W9 => "lcn9",
		Responsive.W12 => "lcn12",
		Responsive.Full => "lcnf",
		_ => LogIf.ArgumentOutOfRange<string>(e, nameof(e))
	};

    internal static string ToCssNavBar(this Responsive e) => e switch
    {
        Responsive.W6 => "cnvb6",
        Responsive.W9 => "cnvb9",
        Responsive.W12 => "cnvb12",
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
