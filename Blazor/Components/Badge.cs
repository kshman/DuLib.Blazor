namespace Du.Blazor.Components;

public class Badge : ComponentContent
{
	[Parameter] public TagVariant? Color { get; set; }
	[Parameter] public TagVariant? Background { get; set; }
	[Parameter] public TagRound? Round { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class("bdg",
			(Color ?? TagVariant.Light).ToCss("cc"),
			(Background ?? TagVariant.Primary).ToCss("bg"),
			Round?.ToCss("rnd"));
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)=>
		RenderTag(builder, "span");
}
