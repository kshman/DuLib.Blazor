namespace Du.Blazor.Components;

public enum TagRole
{
	Block,
	Image,
	Header,
	Footer,
	Content,
}


/// <summary>
/// 태그 아이템 기본. 텍스트 속성만 갖고 있음<br/>
/// 이 클래스에서는 컨테이너/부모/채용자/연결자 등을 정의하지 않음<br/>
/// </summary>
/// <remarks>
/// 태그를 정의할때 텍스트가 필요하면 이 클래스를 상속할것.
/// </remarks>
public abstract class TagProp : ComponentContent
{
	/// <summary>태그 핸들러</summary>
	[CascadingParameter]
	public ITagHandler? TagHandler { get; set; }

	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>참일 경우 감싸는태그의 모드로 출력한다</summary>
	/// <remarks>예컨데 드랍일경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</remarks>
	[Parameter] public bool Represent { get; set; }

	/// <summary>클릭</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	public string TagName { get; }
	public TagRole TagRole { get; }

	//
	protected TagProp(string tag, TagRole role)
	{
		TagName = tag;
		TagRole = role;
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (TagHandler is not null)
			TagHandler.OnRender(this, builder);
		else
		{
			// 핸들러 없이도 그림
			RenderTagProp(builder);
		}
	}

	//
	internal void RenderTagProp(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@ActualClass" id="@Id" @attributes="@UserAttrs">
		 *     @Text
		 *     @ChildContent
		 * </div>
		 */
		builder.OpenElement(0, TagName);
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
	internal void RenderTagPropWithWrapTag(RenderTreeBuilder builder, string wrapTag)
	{
		/*
		 * 	<wrap-tag>
		 * 		<tag class="@ActualClass" id="@Id" @attributes="@UserAttrs">
		 * 			@Text
		 * 			@ChildContent
		 * 		</tag>
		 * 	</wrap-tag>
		 */

		builder.OpenElement(0, wrapTag);

		builder.OpenElement(2, TagName);
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

	//
	protected virtual Task InvokeOnClick(MouseEventArgs e) => OnClick.InvokeAsync(e);
}


/// <summary>태그 블록</summary>
public class TagBlock : TagProp
{
	//
	public TagBlock()
		: base("div", TagRole.Block)
	{ }

	//
	protected TagBlock(string tag)
		: base(tag, TagRole.Block)
	{ }
}


/// <summary>태그 P</summary>
public class TagP : TagBlock
{
	//
	public TagP()
		: base("p")
	{ }
}


/// <summary>태그 DIV</summary>
public class TagDiv : TagBlock
{
	public TagDiv()
		: base("div")
	{ }
}


/// <summary>태그 SPAN</summary>
public class TagSpan : TagBlock
{
	public TagSpan()
		: base("span")
	{ }
}


/// <summary>
/// 이미지 태그
/// </summary>
public class TagImage : TagProp
{
	/// <summary>이미지 URL</summary>
	[Parameter] public string? Image { get; set; }
	/// <summary>가로 너비</summary>
	[Parameter] public int? Width { get; set; }
	/// <summary>세로 높이</summary>
	[Parameter] public int? Height { get; set; }

	//
	public TagImage()
		: base("img", TagRole.Image)
	{ }

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		Text ??= "Image";
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (TagHandler is not null)
			TagHandler.OnRender(this, builder);
		else
		{
			// 핸들러 없이도 그림
			RenderTagImage(builder);
		}
	}

	//
	private void RenderTagImage(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, TagName);
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "src", Image);
		builder.AddAttribute(3, "alt", Text);
		builder.AddAttribute(4, "width", Width);
		builder.AddAttribute(5, "height", Height);
		//builder.AddAttribute(6, "role", "img");

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(7, "onclick", InvokeOnClick);
			builder.AddEventStopPropagationAttribute(8, "onclick", true);
		}

		builder.CloseElement();
	}
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : ComponentContent
{
	[CascadingParameter] public IContentHandler? ContentHandler { get; set; }

	//
	[Inject] protected ILogger<TagContent> Logger { get; set; } = default!;

	//
	public TagRole TagRole { get; }

	//
	public TagContent() =>
		TagRole = TagRole.Content;

	//
	protected TagContent(TagRole role) =>
		TagRole = role;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, this, ContentHandler);

		base.OnInitialized();
	}

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ContentHandler?.OnRender(this, builder);
}


/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContent
{
	public TagHeader()
		: base(TagRole.Header)
	{ }
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContent
{
	public TagFooter()
		: base(TagRole.Footer)
	{ }
}
