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
		Renderer.Tag(builder, "div", "ccdh", component.ChildContent, component.UserAttrs);
		return true;
	}
	
	// 꼬리
	private bool RenderTail(Content component, RenderTreeBuilder builder)
	{
		// 대신 해줘
		Renderer.Tag(builder, "div", "ccdf", component.ChildContent, component.UserAttrs);
		return true;
	}
	
	// 몸통
	private bool RenderMenu(Content component, RenderTreeBuilder builder)
	{
		return true;
	}
	#endregion
}
