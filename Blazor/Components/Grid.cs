namespace Du.Blazor.Components;

/// <summary>
/// 그리드
/// </summary>
public abstract class Grid : ComponentProp
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	/// <summary>바리언트</summary>
	[Parameter] public Variant? Variant { get; set; }
	/// <summary>바리언트 지정값</summary>
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
/// /// <remarks>
/// <see cref="Base"/>, <see cref="W6"/>, <see cref="W9"/>, <see cref="W12"/>, <see cref="W15"/>의
/// 값은 1~10으로,<br/>
/// 1칸부터 10칸을 의미함
/// </remarks>
public class GLine : Grid
{
	/// <summary>기본 크기 (1~10)</summary>
	[Parameter] public int Base { get; set; } = 1;
	/// <summary>600해상도 크기 (1~10)</summary>
	[Parameter] public int? W6 { get; set; }
	/// <summary>900해상도 크기 (1~10)</summary>
	[Parameter] public int? W9 { get; set; }
	/// <summary>1200해상도 크기 (1~10)</summary>
	[Parameter] public int? W12 { get; set; }
	/// <summary>1500해상도 크기 (1~10)</summary>
	[Parameter] public int? W15 { get; set; }

	/// <summary>정렬 방식</summary>
	[Parameter] public Justify? Justify { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_grid_css = Cssc.Class(
			VariantString,
			Justify?.ToCss(),
			"lr",
			ConvertPerLineCount(Base, "lr"),
			ConvertPerLineCount(W6, "l6r"),
			ConvertPerLineCount(W9, "l9r"),
			ConvertPerLineCount(W12, "l12r"),
			ConvertPerLineCount(W15, "l15r"));
	}

	//
	private static string? ConvertPerLineCount(int? count, string header)
	{
		if (count is null)
			return null;

		var correction = Math.Clamp(count.Value, 1, 10);

		return $"{header}{correction}";
	}
}

/// <summary>
/// 그리드 열(블럭)
/// </summary>
/// <remarks>
/// <see cref="Base"/>, <see cref="W6"/>, <see cref="W9"/>, <see cref="W12"/>, <see cref="W15"/>의
/// 값은 다음을 참고:
/// <list type="table">
/// <listheader><term>값</term><description>설명</description></listheader>
/// <item><term>1~10</term><description>10%부터 100%</description></item>
/// <item><term>33/66</term><description>삼분의일(33%)와 삼분의이(66%)</description></item>
/// <item><term>25/75</term><description>사분의일(25%)와 사분의삼(75%) 사분의이는 5를 쓰면됨</description></item>
/// <item><term>16</term><description>육분의일(16%)</description></item>
/// </list>
/// </remarks>
public class GBlock : Grid
{
	/// <summary>기본 크기 (1~10, 33/66, 25/75, 16)</summary>
	[Parameter] public int? Base { get; set; }
	/// <summary>자동으로 늘어나나</summary>
	[Parameter] public bool Grow { get; set; }

	/// <summary>600해상도 크기 (1~10, 33/66, 25/75, 16)</summary>
	[Parameter] public int? W6 { get; set; }
	/// <summary>900해상도 크기 (1~10, 33/66, 25/75, 16)</summary>
	[Parameter] public int? W9 { get; set; }
	/// <summary>1200해상도 크기 (1~10, 33/66, 25/75, 16)</summary>
	[Parameter] public int? W12 { get; set; }
	/// <summary>1500해상도 크기 (1~10, 33/66, 25/75, 16)</summary>
	[Parameter] public int? W15 { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_grid_css = Cssc.Class(
			VariantString,
			Grow ? "lcg" : "lc",
			ConvertGridWidth(Base, "lc"),
			ConvertGridWidth(W6, "l6c"),
			ConvertGridWidth(W9, "l9c"),
			ConvertGridWidth(W12, "l12c"),
			ConvertGridWidth(W15, "l15c"));
	}

	//
	private static string? ConvertGridWidth(int? count, string header)
	{
		return count is null ? null : $"{header}{count}";
	}
}
