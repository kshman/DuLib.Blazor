using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

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


/// <summary>스토리지 또는 컨테이너용 아이템</summary>
public class TagSubset : TagTextBase, IAsyncDisposable
{
	/// <summary>이 컴포넌트를 포함하는 컨테이너</summary>
	[CascadingParameter] public ComponentStorage<TagSubset>? Container { get; set; }

	/// <summary>디스플레이 CSS클래스. 제목에 쓰임</summary>
	[Parameter] public string? DisplayClass { get; set; }
	/// <summary>디스플레이 태그. 제목에 쓰임</summary>
	[Parameter] public RenderFragment? Display { get; set; }
	/// <summary>콘텐트 태그. 내용에 쓰임</summary>
	[Parameter] public RenderFragment? Content { get; set; }

	//
	public object? ExtendObject { get; set; }

	//
	protected override Task OnInitializedAsync()
	{
		ThrowIf.ContainerIsNull(this, Container);

		return Container is null ? Task.CompletedTask : Container.AddItemAsync(this);
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	protected virtual Task DisposeAsyncCore() =>
		Container is not null ? Container.RemoveItemAsync(this) : Task.CompletedTask;
}
