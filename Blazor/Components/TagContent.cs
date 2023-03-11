namespace Du.Blazor.Components;

/// <summary>태그 콘텐트 부위</summary>
public enum TagContentRole
{
	Header,
	Footer,
	Content,
}


/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContent
{
	public TagHeader()
		: base(TagContentRole.Header)
	{ }
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContent
{
	public TagFooter()
		: base(TagContentRole.Footer)
	{ }
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : ComponentContent
{
	[CascadingParameter] public ITagContentHandler? ContentHandler { get; set; }

	//
	[Inject] protected ILogger<TagContent> Logger { get; set; } = default!;

	//
	private readonly TagContentRole _role;

	//
	public TagContent() =>
		_role = TagContentRole.Content;

	//
	protected TagContent(TagContentRole role) =>
		_role = role;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, this, ContentHandler);

		base.OnInitialized();
	}

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ContentHandler?.OnRender(_role, this, builder);
}
