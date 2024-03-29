﻿namespace Du.Blazor;

/// <summary>아이템 컴포넌트 스토리지</summary>
/// <remarks>컨테이너와 다른 점은, 스토리지는 그냥 아이템만 보관하고 관리</remarks>
/// <typeparam name="TItem"><see cref="ComponentProp"/>를 상속한 아이템 컴포넌트</typeparam>
public abstract class ComponentStorage<TItem> : ComponentProp,
	IComponentStorage<TItem>, IAsyncDisposable
	where TItem : ComponentProp
{
	//
	protected List<TItem> Items { get; } = new();

	/// <summary>사용할 준비가 끝났다</summary>
	public bool Ready { get; protected set; }

	// 
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		await OnAfterFirstRenderAsync();

		Ready = true;
	}

	//
	public virtual Task AddItemAsync(TItem item)
	{
		Items.Add(item);

		return OnItemAddedAsync(item);
	}

	//
	public virtual Task RemoveItemAsync(TItem item)
	{
		Items.Remove(item);

		return Ready is false ? Task.CompletedTask : OnItemRemovedAsync(item);
	}

	//
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
	protected virtual ValueTask DisposeAsyncCore()
	{
		Ready = false;
		return ValueTask.CompletedTask;
	}

	/// <summary>태스크 상태를 봐서 기다렸다가 StateHasChanged 호출</summary>
	/// <param name="task"></param>
	/// <returns></returns>
	protected async Task StateHasChangedOnAsyncCompletion(Task task)
	{
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

		StateHasChanged();
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
public abstract class ComponentContainer<TItem> : ComponentStorage<TItem>, IComponentContainer<TItem>
	where TItem : ComponentProp
{
	//
	[Parameter]
	public string? CurrentId { get; set; }

	/// <summary>현재 아이템 ID 바뀜 이벤트</summary>
	[Parameter]
	public EventCallback<string?> CurrentIdChanged { get; set; }

	//
	public TItem? SelectedItem { get; set; }

	/// <summary>CurrentId가 없으면 맨 첫 항목을 골라둠</summary>
	protected virtual bool SelectFirst => true;

	// 
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		if (CurrentId.TestHave())
		{
			var item = GetItem(CurrentId);
			await SelectItemAsync(item, true);
		}
		else if (SelectFirst is false)
		{
			// 이거 안하면 화면에 안나온다... 이럴스가
			StateHasChanged();
		}
		else if (Items.Count > 0)
		{
			var item = Items[0];
			await SelectItemAsync(item, true);
		}

		await OnAfterFirstRenderAsync();

		Ready = true;
	}

	//
	public override Task RemoveItemAsync(TItem item)
	{
		if (SelectedItem == item)
			SelectedItem = null;

		return base.RemoveItemAsync(item);
	}

	//
	public async Task SelectItemAsync(TItem? item, bool stateHasChanged = false)
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

		if (stateHasChanged)
			StateHasChanged();
	}

	//
	public Task SelectItemAsync(string id, bool stateHasChanged = false)
	{
		var item = Items.FirstOrDefault(i => i.Id == id);
		return SelectItemAsync(item, stateHasChanged);
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


/// <summary>
/// 스토리지 또는 컨테이너에서 사용하는 항목
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <typeparam name="TStorage"></typeparam>
public abstract class ComponentSubset<TItem, TStorage> : ComponentProp,
	IAsyncDisposable
	where TItem : ComponentProp
	where TStorage : ComponentStorage<TItem>
{
	[CascadingParameter] public TStorage? Storage { get; set; }

	/// <inheritdoc />
	protected override Task OnInitializedAsync()
	{
		ThrowIf.ContainerIsNull<TItem, TStorage>(Storage);

		FillInternalId();

		return Storage.AddItemAsync(this.CastFail<TItem>());
	}

	//
	public async ValueTask DisposeAsync()
	{
		if (Storage is not null)
		{
			await Storage.RemoveItemAsync(this.CastFail<TItem>());
			Storage = null;
		}

		GC.SuppressFinalize(this);
	}
}
