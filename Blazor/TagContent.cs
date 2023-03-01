using Du.Blazor.Supp;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor;

/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContent
{
	/// <inheritdoc />
	protected override TagContentRole Role => TagContentRole.Header;
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContent
{
	/// <inheritdoc />
	protected override TagContentRole Role => TagContentRole.Footer;
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : ComponentFragment
{
	[CascadingParameter] public ITagContentHandler? ContentHandler { get; set; }

	protected virtual TagContentRole Role => TagContentRole.Content;

	//
	[Inject] protected ILogger<TagContent> Logger { get; set; } = default!;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, ContentHandler);

		base.OnInitialized();
	}

	//
	protected override void OnComponentClass(CssCompose cssc) =>
		ContentHandler?.OnClass(Role, this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ContentHandler?.OnRender(Role, this, builder);
}
