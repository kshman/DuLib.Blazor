using Du.Blazor.Components;

namespace Du.Blazor;

/// <summary>
/// 컴포넌트 스토리지 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IComponentStrage<TItem>
{
	/// <summary>아이템 추가</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task AddItemAsync(TItem item);
	/// <summary>아이템 삭제</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task RemoveItemAsync(TItem item);
	/// <summary>아이템 얻기</summary>
	/// <param name="id">찾을 아이디</param>
	/// <returns>찾은 아이템</returns>
	TItem? GetItem(string id);
}


/// <summary>
/// 컴포넌트 컨테이너 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IComponentContainer<TItem> : IComponentStrage<TItem>
{
	/// <summary>현재 아이템 ID</summary>
	string? CurrentId { get; set; }
	/// <summary>골라둔 아이템</summary>
	TItem? SelectedItem { get; set; }
	/// <summary>아이템 선택</summary>
	/// <param name="item"></param>
	/// <param name="stateHasChanged"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task SelectItemAsync(TItem? item, bool stateHasChanged = false);
	/// <summary>아이디로 아이템 선택</summary>
	/// <param name="id"></param>
	/// <param name="stateHasChanged"></param>
	/// <returns>비동기 처리한 태스크</returns>
	Task SelectItemAsync(string id, bool stateHasChanged = false);
}


/// <summary>태그 아이템 핸들러</summary>
/// <remarks>컨테이너가 아닌것은 개체를 소유하지 않고 처리만 도와주기 때문</remarks>
public interface ITagHandler
{
	/// <summary>
	/// 태그 아이템의 렌더 트리를 만듦
	/// </summary>
	/// <param name="tag"></param>
	/// <param name="builder"></param>
	void OnRender(TagProp tag, RenderTreeBuilder builder);
}


/// <summary>
/// 태그 콘텐트 에이전시
/// </summary>
public interface IContentHandler
{
	/// <summary>
	/// 태그 콘텐트의 렌더 트리를 만듦
	/// </summary>
	/// <param name="content">콘텐트</param>
	/// <param name="builder">빌드 개체</param>
	void OnRender(TagContent content, RenderTreeBuilder builder);
}


/// <summary>
/// 리스트 에이전트
/// </summary>
public interface IListAgent
{
	/// <summary>
	/// 감싸야할 태그 이름. 널이면 안감싸도록
	/// </summary>
	string? Tag { get; }
	/// <summary>
	/// 원래 태그(감싸는 태그가 아닌)의 CSS 클래스
	/// </summary>
	string? Class { get; }
	/// <summary>
	/// 반응에 대한 처리
	/// </summary>
	/// <param name="component"></param>
	/// <returns></returns>
	Task OnResponseAsync(ComponentProp component);
}
