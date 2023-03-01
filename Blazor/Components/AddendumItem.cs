namespace Du.Blazor.Components;

/// <summary>부속 아이템 컨테이너</summary>
/// <remarks>
/// 이 컴포넌트를 상속 받는 애들은 <see cref="AddendumItem"/> 컴포넌트로
/// 콘텐트를 구성해야 함
/// </remarks>
public abstract class AddendumItemContainer : ComponentContainer<AddendumItem>
{
}

/// <summary>스토리지 또는 컨테이너용 아이템</summary>
public class AddendumItem : TagTextBase, IAsyncDisposable
{
	/// <summary>이 컴포넌트를 포함하는 컨테이너</summary>
	[CascadingParameter] public ComponentStorage<AddendumItem>? Container { get; set; }

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