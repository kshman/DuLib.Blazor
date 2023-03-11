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

	/// <summary>아래 컴포넌트가 미리 지정할 수 있는 CSS 클래스</summary>
	protected virtual string? ComponentClass => null;

	// 실제 CSS 클래스
	internal string? ActualClass => Cssc.Class(Class, ComponentClass);

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
/// 콘텐트를 가지는 컴포넌트
/// </summary>
public abstract class ComponentContent : ComponentProp
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] public RenderFragment? ChildContent { get; set; }

	// 태그로 자식 콘텐트를 감써서 그린다
	internal void RenderWithTag(RenderTreeBuilder builder, string tag = "div")
	{
		/*
		 * <tag class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *    @ChildContent
		 * </tag>
		 */

		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);
		builder.AddContent(4, ChildContent);
		builder.CloseElement(); // tag
	}

	// 태그로 자식 콘텐츠를 캐스캐이딩해서 그린다
	internal void RenderCascadingWithTag<TComponent>(RenderTreeBuilder builder, string tag = "div")
	{
		/*
		 * <tag class="@CssClass" id="@Id" @attributes="@UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true>
		 *         @ChildContent
		 *     </CascadingValue>
		 * </tag>
		 */
		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);

		builder.OpenComponent<CascadingValue<TComponent>>(4);
		builder.AddAttribute(5, "Value", this);
		builder.AddAttribute(6, "IsFixed", true);
		builder.AddAttribute(7, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(8, ChildContent)));
		builder.CloseComponent(); // CascadingValue<TType>

		builder.CloseElement(); // tag
	}
}
