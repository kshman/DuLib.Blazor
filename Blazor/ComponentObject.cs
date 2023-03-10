namespace Du.Blazor;

/// <summary>
/// 자식 콘텐트를 가지는 컴포넌트
/// </summary>
public abstract class ComponentObject : BaseComponent
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


/// <summary>
/// 컴포넌트 맨 밑단
/// </summary>
public abstract class BaseComponent : ComponentBase
{
	/// <summary>클래스 지정</summary>
	[Parameter]
	public string? Class { get; set; }

	/// <summary>컴포넌트 아이디</summary>
	[Parameter]
	public string? Id { get; set; }

	/// <summary>사용자가 설정한 속성 지정</summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? UserAttrs { get; set; }

	//
	internal string? ActualClass => ComponentClass is null
		? Class
		: Class is null
			? ComponentClass
			: $"{Class} {ComponentClass}";

	protected virtual string? ComponentClass => null;

	//
	private static uint _id_atomic_index = 1;

	//
	protected void FillAutoId() =>
		Id ??= $"DZ_{Interlocked.Increment(ref _id_atomic_index):X}";

	//
	public override string ToString() => Id is null
		? $"<{GetType().Name}>"
		: $"<{GetType().Name}#{Id}>";
}



// 검토
// 팝업/다이얼로그
//		https://stackoverflow.com/questions/72004471/creating-a-popup-in-blazor-using-c-sharp-method
//		https://stackoverflow.com/questions/72005345/bootstrap-modal-popup-using-blazor-asp-net-core
//		https://stackoverflow.com/questions/73617831/pass-parameters-to-modal-popup
//		아니 찾다보니 많네..
// 트리
//		https://stackoverflow.com/questions/70311596/how-to-create-a-generic-treeview-component-in-blazor
