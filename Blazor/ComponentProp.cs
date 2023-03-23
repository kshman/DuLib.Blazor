namespace Du.Blazor;

/// <summary>
/// 컴포넌트 맨 밑단
/// </summary>
public abstract class ComponentProp : ComponentBase
{
	/// <summary>클래스 지정</summary>
	[Parameter] public string? Class { get; set; }
	/// <summary>컴포넌트 아이디</summary>
	[Parameter] public string? Id { get; set; }
	/// <summary>사용자가 설정한 속성 지정</summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? UserAttrs { get; set; }

	/// <summary>컴포넌트 롤</summary>
	protected ComponentRole TagRole { get; init; }

	// 아토믹 인덱스
	private static uint _id_atomic_index = 1;

	/// <summary>내부적으로 아이디를 만들어줌</summary>
	protected void FillInternalId() =>
		Id ??= $"DZ_{Interlocked.Increment(ref _id_atomic_index):X}";

	/// <inheritdoc />
	public override string ToString() => Id is null
		? $"<{GetType().Name}>"
		: $"<{GetType().Name}#{Id}>";
}


/// <summary>
/// 블럭 컴포넌트
/// </summary>
public abstract class ComponentBlock : ComponentProp
{
	/// <summary>컴포넌트 에이전트</summary>
	[CascadingParameter] public IComponentAgent? AgentHandler { get; set; }
	/// <summary>컴포넌트 렌더러</summary>
	[CascadingParameter] public IComponentRenderer? Renderer { get; set; }

	/// <summary></summary>
	[Parameter] public Variant? Variant { get; set; }
	/// <summary></summary>
	[Parameter] public VarLead? Lead { get; set; }
	/// <summary>툴팁</summary>
	[Parameter] public string? Tooltip { get; set; }

	/// <summary>클릭</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	protected string TagName { get; }

	//
	protected ComponentBlock(ComponentRole role, string? tag = null)
	{
		TagRole = role;
		TagName = tag ?? "div";
	}

	/// <summary>
	/// 기본 css 클래스를 만들어 줌
	/// </summary>
	/// <param name="tagClass"></param>
	/// <param name="param1"></param>
	/// <param name="param2"></param>
	/// <returns></returns>
	protected string? GetCssClass(string? tagClass = null, string? param1 = null, string? param2 = null)
	{
		return Cssc.Class(
			Variant?.ToCss(Lead ?? VarLead.Down),
			AgentHandler?.GetRoleClass(TagRole) ?? tagClass,
			param1, param2, Class);
	}

	/// <summary>
	/// 렌더러가 그렸으면 true
	/// </summary>
	/// <param name="builder"></param>
	/// <returns></returns>
	protected bool RendererCheck(RenderTreeBuilder builder)
	{
		return Renderer is not null && Renderer.OnRender(TagRole, this, builder);
	}

	//
	internal void RenderBlock(RenderTreeBuilder builder, string? css, RenderFragment? content)
	{
		/*
		 * <tag class="@ActualClass" id="@Id" @attributes="@UserAttrs">
		 *     @ChildContent
		 * </tag>
		 */

		builder.OpenElement(0, TagName);
		builder.AddAttribute(1, "class", css);
		builder.AddAttribute(2, "id", Id);
		if (Tooltip.TestHave(true))
			builder.AddAttribute(3, "tooltip", Tooltip);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(4, "role", "button");
			builder.AddAttribute(5, "onclick", HandleOnClick);
			builder.AddEventStopPropagationAttribute(6, "onclick", true);
		}

		builder.AddMultipleAttributes(7, UserAttrs);
		if (content is not null)
			builder.AddContent(8, content);
		builder.CloseElement(); // tag
	}

	//
	protected virtual Task HandleOnClick(MouseEventArgs e) => OnClick.InvokeAsync(e);
}
