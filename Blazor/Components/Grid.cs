namespace Du.Blazor.Components;

/// <summary>
/// 그리드
/// </summary>
public abstract class Grid : ComponentProp
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	/// <summary></summary>
	[Parameter] public Variant? Variant { get; set; }
	/// <summary></summary>
	[Parameter] public VarLead? Lead { get; set; }
	/// <summary>툴팁</summary>
	[Parameter] public string? Tooltip { get; set; }

	//
	protected string? _grid_css;

	// 
	protected Grid()
	{
		TagRole = ComponentRole.Block;
	}

	//
	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", Cssc.Class(_grid_css, Class));
		builder.AddMultipleAttributes(2, UserAttrs);
		builder.AddContent(3, ChildContent);
		builder.CloseElement();
	}

	// 바리언트 변환
	protected string? VariantString => Variant?.ToCss(Lead ?? VarLead.Down);
}

/// <summary>
/// 그리드 줄
/// </summary>
public class GLine : Grid
{
	[Parameter] public int Base { get; set; }
	[Parameter] public int? W6 { get; set; }
	[Parameter] public int? W9 { get; set; }
	[Parameter] public int? W12 { get; set; }
	[Parameter] public int? W15 { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_grid_css = Cssc.Class(
			VariantString,
			"lr",
			$"lr{Base}",
			W6 is null ? null : $"l6r{W6}",
			W9 is null ? null : $"l9r{W9}",
			W12 is null ? null : $"l12r{W12}",
			W15 is null ? null : $"l15r{W15}");
	}
}

/// <summary>
/// 그리드 열(블럭)
/// </summary>
public class GBlock : Grid
{
	[Parameter] public string? Base { get; set; }
	[Parameter] public string? W6 { get; set; }
	[Parameter] public string? W9 { get; set; }
	[Parameter] public string? W12 { get; set; }
	[Parameter] public string? W15 { get; set; }
	[Parameter] public bool Grow { get; set; }
	[Parameter] public bool Center { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_grid_css = Cssc.Class(
			VariantString,
			Grow? "lcg": "lc",
			Center.IfTrue("lcj"),
			ConvertResponseClass(Rdm.None, Base),
			ConvertResponseClass(Rdm.W6, W6),
			ConvertResponseClass(Rdm.W9, W9),
			ConvertResponseClass(Rdm.W12, W12),
			ConvertResponseClass(Rdm.W15, W15));
	}

	// 숫자를 열 크기로
	private static string ConvertResponseClass(Rdm rdm, int n)
	{
		var s = n switch
		{
			0 => "n",
			100 => "f",
			_ => n.ToString()
		};
		var r = rdm switch
		{
			Rdm.None => "lc",
			Rdm.W6 => "l6c",
			Rdm.W9 => "l9c",
			Rdm.W12 => "l12c",
			Rdm.W15 => "l15c",
			_ => LogIf.ArgumentOutOfRange<string>(rdm, nameof(rdm))
		};
		return $"{r}{s}";
	}

	// 문자열을 열 크기로
	private static string? ConvertResponseClass(Rdm rdm, string? rsp)
	{
		if (rsp is null)
			return null;

		if (int.TryParse(rsp, out var n))
		{
			// ㅇㅋ 숫자에서 변환
			return ConvertResponseClass(rdm, n);
		}

		if (Enum.TryParse<Rdc>(rsp, true, out var rdc))
		{
			// ㅇㅋ 문자열에서 변환
			n = rdc switch
			{
				Rdc.None => 0,
				Rdc.Full => 100,
				Rdc.Half => 50,
				Rdc.OneThird => 33,
				Rdc.TwoThird => 66,
				Rdc.OneFourth => 25,
				Rdc.ThreeFourth => 75,
				Rdc.OneFifth => 20,
				Rdc.TwoFifth => 40,
				Rdc.ThreeFifth => 60,
				Rdc.FourFifth => 80,
				Rdc.OneSixth => 16,
				_ => 0
			};

			return ConvertResponseClass(rdm, n);
		}

		// 이건 변환 못한다 
		ThrowIf.InvalidArgument(nameof(rsp));

		return null;
	}
}
