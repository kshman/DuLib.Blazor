namespace Du.Blazor.Components;

public class Card : ComponentProp, IComponentAgent, IComponentRenderer
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public string? Image { get; set; }
	[Parameter] public string? Text { get; set; }
	[Parameter] public int? Width { get; set; }
	[Parameter] public int? Height { get; set; }
	[Parameter] public Placement Placement { get; set; }
	[Parameter] public bool AutoSize { get; set; }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		// 대신 그려주는거 쓰자
		Renderer.CascadingTag(builder, this, "div", "ccd", ChildContent, UserAttrs);
	}

	#region IComponentAgent
	/// <inheritdoc />
	bool IComponentAgent.SelfClose => false;

	/// <inheritdoc />
	string? IComponentAgent.GetRoleClass(ComponentRole role) =>
		role switch
		{
			ComponentRole.Block or
				ComponentRole.Text or
				ComponentRole.Image or
				ComponentRole.Link => "ccdm",
			_ => null
		};
	#endregion

	#region IComponentRenderer
	/// <inheritdoc />
	bool IComponentRenderer.OnRender(ComponentRole role, ComponentBlock component, RenderTreeBuilder builder) =>
		role switch
		{
			ComponentRole.Lead => RenderLead((Content)component, builder),
			ComponentRole.Tail => RenderTail((Content)component, builder),
			ComponentRole.Menu => RenderMenu((Content)component, builder),
			_ => false
		};

	// 머리
	private bool RenderLead(Content component, RenderTreeBuilder builder)
	{
		// 대신 해줘
		Renderer.Tag(builder, "div",
			Cssc.Class("ccdh", component.Class),
			component.ChildContent,
			component.UserAttrs);
		return true;
	}

	// 꼬리
	private bool RenderTail(Content component, RenderTreeBuilder builder)
	{
		// 대신 해줘
		Renderer.Tag(builder, "div",
			Cssc.Class("ccdf", component.Class),
			component.ChildContent,
			component.UserAttrs);
		return true;
	}

	// 몸통
	private bool RenderMenu(Content component, RenderTreeBuilder builder)
	{
		var image = Image.TestHave(true);
		
		if (image && Placement is Placement.Top or Placement.Start or Placement.Overlay)
		{
			builder.OpenElement(0, "img");
			builder.AddAttribute(1, "class", Cssc.Class(
				"ccdi",
				Placement switch
				{
					Placement.Top => "ccdit",
					Placement.Start => "ccdil",
					_ => null
				}));
			builder.AddAttribute(2, "src", Image);
			builder.AddAttribute(3, "alt",  Text);
			builder.AddAttribute(4, "width", Width);
			builder.AddAttribute(5, "height", Height);
			builder.CloseElement();
		}
		
		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", image && Placement is Placement.Overlay ? "ccdo" : "ccdc");
		builder.AddMultipleAttributes(12, component.UserAttrs);
		builder.AddContent(13, component.ChildContent);
		builder.CloseElement(); // div, 몸통

		if (image && Placement is Placement.Bottom or Placement.End)
		{
			builder.OpenElement(20, "img");
			builder.AddAttribute(21, "class", Cssc.Class(
				"ccdi",
				Placement is Placement.Bottom ? "ccdib" : "ccdir"));
			builder.AddAttribute(22, "src", Image);
			builder.AddAttribute(23, "alt",  Text);
			builder.AddAttribute(24, "width", Width);
			builder.AddAttribute(25, "height", Height);
			builder.CloseElement();
		}

		return true;
	}
	#endregion
}
