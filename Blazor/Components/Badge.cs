namespace Du.Blazor.Components;

public class Badge : ComponentContent
{
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Active { get; set; } = true;
	[Parameter] public bool Round { get; set; }

	[Parameter] public bool Outer { get; set; }
	[Parameter] public bool Bottom { get; set; }

	[Parameter] public string? Color { get; set; }
	[Parameter] public string? Background { get; set; }
	[Parameter] public string? Style { get; set; }

	// Color랑 Background를 위한 스타일
	private string? _style;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		if (Outer is false)
		{
			// 안쪽 뱃지
			ComponentClass = Cssc.Class(
				"bdg bdg-i",
				(Variant ?? Settings.Variant).ToCss(Active ? "a" : "d"),
				Round.IfTrue("dgrm"));
		}
		else
		{
			// 바깥쪽 뱃지
			ComponentClass = Cssc.Class(
				"bdg",
				(Variant ?? Settings.Variant).ToCss(Active ? "a" : "d"),
				Round.IfTrue("dgrm"),
				Bottom ? "brb" : "brt");
		}

		// 스타일
		var sc = Color.TestHave() ? $"color:var(--bdg-cc-{Color})" : null;
		var sb = Background.TestHave() ? $"background-color:var(--bdg-bg-{Background})" : null;
		_style = Cssc.Style(sc, sb, Style);
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (Outer is false)
			RenderInner(builder);
		else
			RenderOuter(builder);
	}

	// 안쪽
	private void RenderInner(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" @attributes="UserAttrs">
		 *    @ChildContent
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "style", _style);
		builder.AddMultipleAttributes(3, UserAttrs);
		builder.AddContent(4, ChildContent);
		builder.CloseElement(); // div
	}

	// 바깥쪽
	private void RenderOuter(RenderTreeBuilder builder)
	{
		// 어떻게 할까
		ThrowIf.NotImplementedWithCondition<Badge>(false);
		_ = builder;
	}
}
