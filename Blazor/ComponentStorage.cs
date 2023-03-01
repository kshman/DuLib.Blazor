using Du.Blazor.Supp;

namespace Du.Blazor;

/// <summary>아이템 컴포넌트 스토리지</summary>
/// <remarks>컨테이너와 다른 점은, 스토리지는 그냥 아이템만 보관하고 관리</remarks>
/// <typeparam name="TItem"><see cref="ComponentObject"/>를 상속한 아이템 컴포넌트</typeparam>
public class ComponentStorage<TItem> : ComponentFragment, IComponentStrage<TItem>, IAsyncDisposable
	where TItem : ComponentObject
{
	//
	protected List<TItem> Items { get; } = new();

	// 
	protected override Task OnAfterRenderAsync(bool firstRender) =>
		!firstRender ? Task.CompletedTask : InvokeAfterRenderFirstAsync();

	/// <summary>아이템 추가</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	public virtual async Task AddItemAsync(TItem item)
	{
		Items.Add(item);

		var task = OnItemAddedAsync(item);
		await StateHasChangedOnAsyncCompletion(task);
	}

	/// <summary>아이템 삭제</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	public virtual async Task RemoveItemAsync(TItem item)
	{
		Items.Remove(item);

		try
		{
			var task = OnItemRemovedAsync(item);
			await StateHasChangedOnAsyncCompletion(task);
		}
		catch (ObjectDisposedException)
		{
			// 헐
			// 이건 디스포즈하다가 개체가 사라진거겟지
		}
	}

	/// <summary>아이템 얻기</summary>
	/// <param name="id">찾을 아이디</param>
	/// <returns>찾은 아이템</returns>
	public TItem? GetItem(string id) =>
		Items.FirstOrDefault(x => x.Id == id);

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	/// <summary><see cref="DisposeAsync"/>의 처리 메소드</summary>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;

	//
	protected async Task InvokeAfterRenderFirstAsync()
	{
		var task = OnAfterFirstRenderAsync();
		if (task.ShouldAwaitTask())
		{
			try
			{
				await task;
			}
			catch
			{
				if (!task.IsCanceled)
					throw;
			}
		}
	}

	/// <summary>
	/// <see cref="OnAfterRenderAsync(bool)"/> 메소드가 처리될 때
	/// 특별하게 firstRender의 경우에만 호출
	/// </summary>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnAfterFirstRenderAsync() => Task.CompletedTask;

	/// <summary>아이템 추가될 때</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnItemAddedAsync(TItem item) => Task.CompletedTask;

	/// <summary>아이템 삭제할 때 </summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnItemRemovedAsync(TItem item) => Task.CompletedTask;
}


/// <summary>컴포넌트 컨테이너</summary>
/// <remarks>
/// 스토리지에서 추가된 것은,
/// <see cref="CurrentId"/>로 현재 아이템을 쓰고 가져올 수 있고,<br/>
/// <see cref="SelectedItem"/>로 선택한 아이템을 처리할 수 있음
/// </remarks>
public class ComponentContainer<TItem> : ComponentStorage<TItem>, IComponentContainer<TItem>
	where TItem : ComponentObject
{
	/// <summary>현재 아이템 ID</summary>
	[Parameter]
	public string? CurrentId { get; set; }

	/// <summary>현재 아이템 ID 바뀜 이벤트</summary>
	[Parameter]
	public EventCallback<string?> CurrentIdChanged { get; set; }

	/// <summary>골라둔 아이템</summary>
	protected TItem? SelectedItem { get; set; }

	/// <summary>CurrentId가 없으면 맨 첫 항목을 골라둠</summary>
	protected virtual bool SelectFirst => true;

	// 
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		if (SelectedItem is null)
		{
			if (CurrentId.IsHave())
				await SelectItemAsync(CurrentId);
			else if (SelectFirst && Items.Count > 0)
				await SelectItemAsync(Items[0]);
		}

		await InvokeAfterRenderFirstAsync();
	}

	//
	public override async Task AddItemAsync(TItem item)
	{
		Items.Add(item);

		if (CurrentId is not null)
		{
			if (item.Id == CurrentId)
				await SelectItemAsync(item);
		}
		else if (SelectedItem is null && SelectFirst && item.Enabled)
		{
			await SelectItemAsync(item);
		}

		var task = OnItemAddedAsync(item);
		await StateHasChangedOnAsyncCompletion(task);
	}

	//
	public override async Task RemoveItemAsync(TItem item)
	{
		if (SelectedItem == item)
			SelectedItem = null;

		Items.Remove(item);

		try
		{
			var task = OnItemRemovedAsync(item);
			await StateHasChangedOnAsyncCompletion(task);
		}
		catch (ObjectDisposedException)
		{
			// 헐
			// 이건 디스포즈하다가 개체가 사라진거겟지
		}
	}

	/// <summary>아이템 선택</summary>
	/// <param name="item"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	public async Task SelectItemAsync(TItem? item, bool stateChange = false)
	{
		if (item == SelectedItem)
			return;

		var previous = SelectedItem;
		SelectedItem = item;

		var task = OnItemSelectedAsync(item, previous);
		if (task.ShouldAwaitTask())
		{
			try
			{
				await task;
			}
			catch
			{
				if (task.IsCanceled)
					return;
				throw;
			}
		}

		if (item is not null && task.Result)
			await InvokeCurrentIdChangedAsync(item.Id);

		if (stateChange)
			StateHasChanged();
	}

	/// <summary>아이디로 아이템 선택</summary>
	/// <param name="id"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	public Task SelectItemAsync(string id, bool stateChange = false)
	{
		var item = Items.FirstOrDefault(i => i.Id == id);
		return SelectItemAsync(item, stateChange);
	}

	/// <summary>아이템을 선택했을 때</summary>
	/// <param name="item"></param>
	/// <param name="previous"></param>
	/// <returns>
	/// 비동기 처리한 태스크<br/>
	/// false를 반환하면 그뒤에 기능(InvokeCurrentIdChangedAsync 호출)을 하지 않음
	/// </returns>
	protected virtual Task<bool> OnItemSelectedAsync(TItem? item, TItem? previous) => Task.FromResult(true);

	/// <summary><see cref="CurrentId"/> 항목이 변경됐을 때</summary>
	/// <param name="id"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task InvokeCurrentIdChangedAsync(string? id) => CurrentIdChanged.InvokeAsync(id);
}
