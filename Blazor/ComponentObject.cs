using Du.Blazor.Supp;

namespace Du.Blazor;

/// <summary>
/// Du.Blazor 컴포넌트 맨 밑단
/// </summary>
public abstract class ComponentObject : ComponentBase
{
	/// <summary>사용 여부 (disabled)</summary>
	[Parameter] 
	public bool Enabled { get; set; } = true;

	/// <summary>클래스 지정</summary>
	[Parameter] 
	public string? Class { get; set; }

	/// <summary>컴포넌트 아이디</summary>
	[Parameter]
	public string Id { get; set; } = $"D_Z_{NextAtomicIndex:X}";

	/// <summary>사용자가 설정한 속성 지정</summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object> UserAttrs { get; set; } = new();

	/// <summary>만들어진 최종 CSS 클래스</summary>
	public string? CssClass => _css_compose.Class;

	// 
	private bool _init_css;
	private readonly CssCompose _css_compose = new();

	/// <inheritdoc />
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		UserAttrs.Clear();

		foreach (var p in parameters)
		{
			switch (p.Name)
			{
				case nameof(Enabled):
					Enabled = (bool)p.Value;
					break;
				case "class" or nameof(Class):
					Class = (string?)p.Value;
					break;
				case "id" or nameof(Id):
					Id = (string)p.Value;
					break;
				default:
					UserAttrs.Add(p.Name, p.Value);
					break;
			}
		}

		await base.SetParametersAsync(ParameterView.Empty);

		if (!_init_css)
		{
			_init_css = true;

			OnComponentClass(_css_compose);
			_css_compose.Add(Class).Register(() => Enabled.IfFalse("disabled"));
		}
	}

	/// <summary>
	/// 컴포넌트의 CSS 클래스 값을 지정<br/>
	/// <see cref="ComponentBase.OnInitialized"/>와 <see cref="ComponentBase.OnParametersSet"/>을
	/// 모두 거친 다음에 호출됨
	/// </summary>
	protected virtual void OnComponentClass(CssCompose cssc)
	{ }

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
				if (task.IsCanceled) return;
				throw;
			}
		}

		StateHasChanged();
	}

	//
	private static uint _atomic_index = 1;
	private static uint NextAtomicIndex => Interlocked.Increment(ref _atomic_index);

	//
	public override string ToString() => $"<{GetType().Name}#{Id}>";
}


/// <summary>
/// 프래그먼트 콘텐트를 가지는 컴포넌트
/// </summary>
public abstract class ComponentFragment : ComponentObject
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] public RenderFragment? ChildContent { get; set; }
}


// 검토
// 팝업/다이얼로그
//		https://stackoverflow.com/questions/72004471/creating-a-popup-in-blazor-using-c-sharp-method
//		https://stackoverflow.com/questions/72005345/bootstrap-modal-popup-using-blazor-asp-net-core
//		https://stackoverflow.com/questions/73617831/pass-parameters-to-modal-popup
//		아니 찾다보니 많네..
// 트리
//		https://stackoverflow.com/questions/70311596/how-to-create-a-generic-treeview-component-in-blazor
