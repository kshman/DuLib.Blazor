namespace Du.Blazor.Components;

/// <summary>태그 DIV 아이템</summary>
public class TagDiv : TagItem
{
	public TagDiv() 
		: base("div")
	{ }
}


/// <summary>태그 SPAN 아이템</summary>
public class TagSpan : TagItem
{
	public TagSpan() 
		: base("span")
	{ }
}


/// <summary>
/// 태그 아이템. 
/// </summary>
public class TagItem : TagTextBase
{
	/// <summary>아이템 핸들러</summary>
	[CascadingParameter]
	public ITagItemHandler? ItemHandler { get; set; }

	/// <summary>참일 경우 감싸는태그의 모드로 출력한다</summary>
	/// <remarks>예컨데 드랍일경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</remarks>
	[Parameter]
	public bool WrapRepresent { get; set; }

	/// <summary>감싸는 태그안에 있을 때 css클래스, 주로 리스트이므로 "li"의 클래스</summary>
	[Parameter]
	public string? WrapClass { get; set; }

	//
	public TagItem() 
		: base("p")
	{ }

	//
	protected TagItem(string tag) 
		: base(tag)
	{ }

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ItemHandler is not null)
			ItemHandler.OnRender(this, builder);
		else
		{
			// 핸들러 없이도 그릴 수 있다!
			RenderTextWithTag(builder);
		}
	}

	//
	internal void RenderSrroundWithTag(RenderTreeBuilder builder, string surroundTag)
	{
		/*
		 * 	<surround-tag>
		 * 		<tag class="@ActualClass" id="@Id" @attributes="@UserAttrs">
		 * 			@Text
		 * 			@ChildContent
		 * 		</tag>
		 * 	</surround-tag>
		 */

		builder.OpenElement(0, surroundTag);
		
		if (WrapClass.TestHave())
			builder.AddAttribute(1, WrapClass);

		builder.OpenElement(2, _tag);
		builder.AddAttribute(3, "class", ActualClass);
		builder.AddAttribute(4, "id", Id);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(5, "role", "button");
			builder.AddAttribute(6, "onclick", InvokeOnClick);
			builder.AddEventStopPropagationAttribute(7, "onclick", true);
		}

		builder.AddMultipleAttributes(8, UserAttrs);
		builder.AddContent(9, Text);
		builder.AddContent(10, ChildContent);
		builder.CloseElement(); // tag

		builder.CloseElement(); // li
	}
}


/// <summary>
/// 태그 아이템 기본. 텍스트 속성만 갖고 있음<br/>
/// 이 클래스에서는 컨테이너/부모/채용자/연결자 등을 정의하지 않음<br/>
/// </summary>
/// <remarks>
/// 태그를 정의할때 텍스트가 필요하면 이 클래스를 상속할것.
/// </remarks>
public abstract class TagTextBase : ComponentContent
{
	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	protected readonly string _tag;

	//
	protected TagTextBase(string tag) => 
		_tag = tag;

	//
	internal void RenderTextWithTag(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@ActualClass" id="@Id" @attributes="@UserAttrs">
		 *     @Text
		 *     @ChildContent
		 * </div>
		 */
		builder.OpenElement(0, _tag);
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "id", Id);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(3, "role", "button");
			builder.AddAttribute(4, "onclick", InvokeOnClick);
			builder.AddEventStopPropagationAttribute(5, "onclick", true);
		}

		builder.AddMultipleAttributes(6, UserAttrs);
		builder.AddContent(7, Text);
		builder.AddContent(8, ChildContent);
		builder.CloseElement(); // tag
	}

	//
	protected virtual Task InvokeOnClick(MouseEventArgs e) => OnClick.InvokeAsync(e);
}
