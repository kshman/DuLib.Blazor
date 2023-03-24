// 개별 구성 요소를 한데 묶은 파일
// (ComponentBlock)
//   Block - Item
//     TextBlock
//     Paragraph
//   Divider
//   Pix
//   Content - Menu
//     Lead
//     Tail
// (ContainerEntry)
//   Subset
namespace Du.Blazor.Components;

/// <summary>
/// 블럭 -> DIV
/// </summary>
public class Block : ComponentBlock
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] public RenderFragment? ChildContent { get; set; }

	//
	public Block()
		: base(ComponentRole.Block, "div")
	{ }

	//
	public Block(ComponentRole role, string tag)
		: base(role, tag)
	{ }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (RendererCheck(builder))
			return;

		RenderBlock(builder, GetCssClass(), ChildContent);
	}
}


/// <summary>
/// 블럭의 별칭...인데 아직 쓸지 안쓸지 안정함
/// </summary>
public class Item : Block
{
}


/// <summary>
/// 텍스트 블럭 -> SPAN
/// </summary>
public class TextBlock : Block
{
	public TextBlock()
		: base(ComponentRole.Text, "span")
	{ }
}


/// <summary>
/// 문단 -> P
/// </summary>
public class Paragraph : Block
{
	public Paragraph()
		: base(ComponentRole.Text, "p")
	{ }
}


/// <summary>
/// 분리 줄
/// </summary>
public class Divider : ComponentBlock
{
	public Divider()
		: base(ComponentRole.Divide, "hr")
	{
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (RendererCheck(builder))
			return;

		RenderBlock(builder, GetCssClass(), null);
	}
}


/// <summary>
/// 그림
/// </summary>
public class Pix : ComponentBlock
{
	/// <summary>이미지 URL</summary>
	[Parameter] public string? Image { get; set; }
	/// <summary>이미지 설명</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>가로 너비</summary>
	[Parameter] public int? Width { get; set; }
	/// <summary>세로 높이</summary>
	[Parameter] public int? Height { get; set; }

	/// <summary></summary>
	[Parameter] public bool AutoSize { get; set; }

	//
	public Pix()
		: base(ComponentRole.Image, "img")
	{ }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		Text ??= "Image";
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (RendererCheck(builder))
			return;

		var css = Cssc.Class(
			AgentHandler?.GetRoleClass(TagRole),
			AutoSize.IfTrue("cpxa"),
			Class);

		builder.OpenElement(0, TagName);
		builder.AddAttribute(1, "class", css);
		builder.AddAttribute(2, "src", Image);
		builder.AddAttribute(3, "alt", Text);
		builder.AddAttribute(4, "width", Width);
		builder.AddAttribute(5, "height", Height);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(6, "role", "button");
			builder.AddAttribute(7, "onclick", HandleOnClick);
			builder.AddEventStopPropagationAttribute(8, "onclick", true);
		}

		builder.CloseElement();
	}
}


/// <summary>
/// 내용
/// </summary>
public class Content : ComponentBlock
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] public RenderFragment? ChildContent { get; set; }

	//
	[Inject] protected ILogger<Content> Logger { get; set; } = default!;

	//
	public Content()
		: base(ComponentRole.Menu)
	{ }

	//
	public Content(ComponentRole role)
		: base(role)
	{ }

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, this, Renderer);

		base.OnInitialized();
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		Renderer?.OnRender(TagRole, this, builder);
}


/// <summary>
/// 머리
/// </summary>
public class Lead : Content
{
	protected Lead()
		: base(ComponentRole.Lead)
	{ }
}


/// <summary>
/// 꼬리
/// </summary>
public class Tail : Content
{
	public Tail()
		: base(ComponentRole.Tail)
	{ }
}


/// <summary>
/// 브랜드
/// </summary>
public class Brand : Lead
{
}


/// <summary>
/// 메뉴
/// </summary>
public class Menu : Content
{
}


/// <summary>
/// 서브셋
/// </summary>
public class Subset : ComponentSubset<Subset, ComponentContainer<Subset>>
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public string? Text { get; set; }
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Active { get; set; }

	// 
	internal bool InternalActive { get; set; }

	/// <inheritdoc />
	protected override Task OnInitializedAsync()
	{
		FillInternalId();

		InternalActive = Active;

		return base.OnInitializedAsync();
	}
}
