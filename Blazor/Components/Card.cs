namespace Du.Blazor.Components;

using ImagePlace = Placement;

public class Card : ComponentProp, IComponentRenderer/*, IComponentAgent*/
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public string? Image { get; set; }
	[Parameter] public string? Text { get; set; }
	[Parameter] public Placement? Placement { get; set; }
	[Parameter] public bool Border { get; set; }
	[Parameter] public string? Title { get; set; }
	[Parameter] public string? SubTitle { get; set; }

	//
	private ImagePlace _place;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		Text ??= "Card image";

		_place = Image.WhiteSpace() ? ImagePlace.None : Placement ?? ImagePlace.Top;
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		// 대신 그려주는거 쓰자
		Renderer.CascadingTag(builder, this, "div",
			Cssc.Class("vcg ccd", Border.IfTrue("bdr")),
			ChildContent, UserAttrs);
	}

	/*#region IComponentAgent
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
	#endregion*/

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
		if (_place is ImagePlace.Top or ImagePlace.Start or ImagePlace.Overlay)
		{
			builder.OpenElement(0, "img");
			builder.AddAttribute(1, "class", "ccdi");
			builder.AddAttribute(2, "src", Image);
			builder.AddAttribute(3, "alt", Text);
			builder.CloseElement();
		}

		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", _place is ImagePlace.Overlay ? "ccdo" : "ccdc");
		builder.AddMultipleAttributes(12, component.UserAttrs);

		if (Title.TestHave(true))
		{
			builder.OpenElement(13, "h5");
			builder.AddAttribute(14, "class", "ccdtm");
			builder.AddContent(15, Title);
			builder.CloseElement();

			if (SubTitle.TestHave(true))
			{
				builder.OpenElement(16, "h6");
				builder.AddAttribute(17, "class", "ccdts");
				builder.AddContent(18, SubTitle);
				builder.CloseElement();
			}
		}

		builder.AddContent(19, component.ChildContent);
		builder.CloseElement(); // div, 몸통

		if (_place is ImagePlace.Bottom or ImagePlace.End)
		{
			builder.OpenElement(20, "img");
			builder.AddAttribute(21, "class", "ccdi");
			builder.AddAttribute(22, "src", Image);
			builder.AddAttribute(23, "alt", Text);
			builder.CloseElement();
		}

		return true;
	}
	#endregion
}
