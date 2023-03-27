namespace Du.Blazor.Components;

/// <summary>
/// 스택 리스트
/// </summary>
public class Stacks : ComponentProp/*, IComponentAgent*/
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public bool Border { get; set; }
	[Parameter] public bool Numbered { get; set; }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		// 대신 그려주는거 쓰자
		Renderer.CascadingTag(builder, this, "div",
			Cssc.Class("cstk", Border.IfTrue("bdr"), Numbered.IfTrue("nmd")),
			ChildContent, UserAttrs);
	}

	/*#region IComponentAgent
	/// <inheritdoc />
	bool IComponentAgent.SelfClose => false;

	/// <inheritdoc />
	string? IComponentAgent.GetRoleClass(ComponentRole role, string? baseClass) => role switch
	{
		ComponentRole.Block or
		ComponentRole.Text or
		ComponentRole.Image => "cstkm",
		ComponentRole.Link => Cssc.Class(baseClass, "cstkm"),
		_ => null,
	};
	#endregion*/
}
