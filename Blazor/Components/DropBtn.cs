namespace Du.Blazor.Components;

/// <summary>
/// 드랍 다운
/// </summary>
public class DropBtn : Nulo, IComponentResponse
{
	/// <summary>오른쪽으로 정렬</summary>
	[Parameter] public bool Right { get; set; }
	/// <summary></summary>
	[Parameter] public string? PanelClass { get; set; }
	/// <summary>선택한 아이템의 TEXT를 버튼 TEXT로 표시한다</summary>
	[Parameter] public bool? SelectText { get; set; }
	/// <summary>선택하면 닫힌다</summary>
	[Parameter] public bool? SelfClose { get; set; }
	/// <summary>모서리 표시</summary>
	[Parameter] public bool Border { get; set; }

	/// <summary></summary>
	[Parameter] public EventCallback<ComponentProp> OnSelect { get; set; }

	//
	private string? _actual_text;
	private bool _short_bye;

	/// <inheritdoc />
	protected override void OnInitialized() =>
		_actual_text = Text;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		SelectText ??= NavAgent is null;
		SelfClose ??= NavAgent is null;

		ComponentClass = GetNuloClassName();
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<div class="dpd">
				<button type="button" class="nulo">버튼 제목</button>
				<div>
					<CascadingValue Value="this" IsFixed="true">
						<a>아이템</a>
						<a>아이템</a>
						<div>아이템</div>
					</CascadingValue>
				</div>
			</div>
		 */
		if (ListAgent?.Tag is not null)
			builder.OpenElement(0, ListAgent.Tag); // wrap

		builder.OpenElement(1, "div");
		builder.AddAttribute(2, "class", Cssc.Class(
			"dpd",
			Border.IfTrue("dpd-border"),
			Right.IfTrue("dpd-right")));

		builder.OpenElement(10, "button");
		builder.AddAttribute(11, "type", "button");
		builder.AddAttribute(12, "class", ActualClass);
		builder.AddAttribute(14, "onclick", HandleOnClickAsync);
		builder.AddEventStopPropagationAttribute(15, "onclick", true);
		builder.AddMultipleAttributes(16, UserAttrs);
		builder.AddContent(17, _actual_text);
		builder.CloseElement(); // button

		if (!_short_bye)
		{
			builder.OpenElement(20, "div");
			builder.AddAttribute(21, "class", PanelClass);

			builder.OpenComponent<CascadingValue<DropBtn>>(22);
			builder.AddAttribute(23, "Value", this);
			builder.AddAttribute(24, "IsFixed", true);
			builder.AddAttribute(25, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(7, ChildContent)));
			builder.CloseComponent(); // CascadingValue<Accds>, 콘텐트

			builder.CloseElement(); // div, 패널
		}

		builder.CloseElement(); // div, 메인

		if (ListAgent?.Tag is not null)
			builder.CloseElement(); // wrap
	}

	#region IComponentResponse
	/// <inheritdoc />
	async Task IComponentResponse.OnResponseAsync(ComponentProp component)
	{
		var self_close = SelfClose ?? true;

		// 일단 닫는다
		if (self_close)
			_short_bye = true;

		// 텍스트 처리
		if (SelectText ?? true)
		{
			if (component is Nulo nulo)
			{
				// 눌러면 눌러 텍스트, 없으면 원래 텍스트
				_actual_text = nulo.Text ?? Text;
			}
		}

		// 암튼 눌렸으니 알림
		if (OnSelect.HasDelegate)
			await OnSelect.InvokeAsync(component);
		else
		{
			if (component is Nulo { InternalType: NuloType.Link })
			{
				// 링크를 누르셧꾼요. 무시
				// 왜냐하면 페이지 이동하면서 리프레시하겠지
				// 링크가 없으면 안하겠지?
			}
			else if (self_close)
			{
				// 아니.. 이벤트 처리가 없으면 리프레시가 안된다. 강제로 처리
				StateHasChanged();
			}
		}

		// 리스폰스
		if (ResponseHandler is not null)
			await ResponseHandler.OnResponseAsync(this);

		// 잠시 대기했다.... 원래대로
		if (self_close)
		{
			try
			{
				await Task.Delay(1);

				_short_bye = false;
				StateHasChanged();
			}
			catch
			{
				// 무슨 예외가 날지도 몰라서 그냥 해 둠
			}
		}
	}
	#endregion
}
