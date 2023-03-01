using Du.Blazor.Supp;
using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor;

/// <summary>
/// 컴포넌트 스토리지 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IComponentStrage<TItem>
{
	Task AddItemAsync(TItem item);
	Task RemoveItemAsync(TItem item);
	TItem? GetItem(string id);
}


/// <summary>
/// 컴포넌트 컨테이너 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IComponentContainer<TItem> : IComponentStrage<TItem>
{
	string? CurrentId { get; set; }
	Task SelectItemAsync(TItem? item, bool stateChange = false);
	Task SelectItemAsync(string id, bool stateChange = false);
}


/// <summary>태그 아이템 핸들러</summary>
/// <remarks>컨테이너가 아닌것은 개체를 소유하지 않고 처리만 도와주기 때문</remarks>
public interface ITagItemHandler
{
	/// <summary>
	/// 태그 아이템의 CSS클래스를 설정
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cssc"></param>
	void OnClass(TagItem item, CssCompose cssc);
	/// <summary>
	/// 태그 아이템의 렌더 트리를 만듦
	/// </summary>
	/// <param name="item"></param>
	/// <param name="builder"></param>
	void OnRender(TagItem item, RenderTreeBuilder builder);
}


/// <summary>태그 콘텐트 부위</summary>
public enum TagContentRole
{
	Header,
	Footer,
	Content,
}


/// <summary>
/// 태그 콘텐트 에이전시
/// </summary>
public interface ITagContentHandler
{
	/// <summary>
	/// 태그 콘텐트의 CSS클래스를 설정
	/// </summary>
	/// <param name="role"></param>
	/// <param name="content">콘텐트</param>
	/// <param name="cssc">CssCompose</param>
	void OnClass(TagContentRole role, TagContent content, CssCompose cssc);

	/// <summary>
	/// 태그 콘텐트의 렌더 트리를 만듦
	/// </summary>
	/// <param name="role">
	/// </param>
	/// <param name="content">콘텐트</param>
	/// <param name="builder">빌드 개체</param>
	void OnRender(TagContentRole role, TagContent content, RenderTreeBuilder builder);
}


/// <summary>
/// 리스트 에이전트
/// </summary>
public interface ITagListAgent
{
	string Tag { get; }
	string Class { get; }
}
