namespace Du.Blazor.Components;

public class Badge : ComponentBlock
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public bool Round { get; set; }

	[Parameter] public int? Color { get; set; }
	[Parameter] public int? Background { get; set; }
	[Parameter] public string? Style { get; set; }

	// Color랑 Background를 위한 스타일
	private string? _style;

	//
	public Badge()
		:base(ComponentRole.Badge)
	{}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		Variant ??= Settings.Variant;

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
		builder.AddAttribute(1, "class", GetCssClass("cbdg", Round.IfTrue("dgrm")));
		builder.AddAttribute(2, "style", _style);
		builder.AddMultipleAttributes(3, UserAttrs);
		builder.AddContent(4, ChildContent);
		builder.CloseElement(); // div
	}
}
