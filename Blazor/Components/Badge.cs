namespace Du.Blazor.Components;

public class Badge : ComponentContent
{
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Active { get; set; } = true;
	[Parameter] public bool Round { get; set; }

	[Parameter] public int? Color { get; set; }
	[Parameter] public int? Background { get; set; }
	[Parameter] public string? Style { get; set; }

	// Color랑 Background를 위한 스타일
	private string? _style;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class(
			"cbdg",
			(Variant ?? Settings.Variant).ToCss(Active ? VrtLead.Up : VrtLead.Down),
			Round.IfTrue("dgrm"));

		// 스타일
		var sc = Color is not null ? $"color:#{Color:X}" : null;
		var sb = Background is not null ? $"background-color:#{Background:X}" : null;
		_style = Cssc.Style(sc, sb, Style);
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
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
}
