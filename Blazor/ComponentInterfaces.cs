namespace Du.Blazor;

/// <summary>
/// 컴포넌트 스토리지 인터페이스
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IComponentStorage<TItem>
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
public interface IComponentContainer<TItem> : IComponentStorage<TItem>
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


/// <summary>
/// 반응을 처리한다
/// </summary>
public interface IComponentResponse
{
	/// <summary>
	/// 반응에 대한 처리
	/// </summary>
	/// <param name="component"></param>
	/// <returns></returns>
	Task OnResponseAsync(ComponentProp component);
}


/// <summary>
/// 컴포넌트 에이전트
/// </summary>
public interface IComponentAgent
{
	/// <summary>
	/// 닫아야하는 액션을 직접 하면 참
	/// </summary>
	bool SelfClose { get; }
	/// <summary>
	/// 역할에 따른 클래스를 얻음
	/// </summary>
	/// <param name="role"></param>
	/// <returns></returns>
    string? GetRoleClass(ComponentRole role);
}


/// <summary>
/// 컴포넌트 렌더 트리 빌더
/// </summary>
public interface IComponentRenderer
{
	/// <summary>
	/// 그리기
	/// </summary>
	/// <param name="role"></param>
	/// <param name="component"></param>
	/// <param name="builder"></param>
	/// <returns>그렸으면 true, 안그렸으면 false</returns>
	bool OnRender(ComponentRole role, ComponentBlock component, RenderTreeBuilder builder);
}

