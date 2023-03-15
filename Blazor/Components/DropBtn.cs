using System.Runtime.Intrinsics.Arm;

namespace Du.Blazor.Components;

/// <summary>
/// 드랍 다운
/// </summary>
public class DropBtn : Nulo, IListAgent
{
	[Parameter] public TagAlignment? Alignment { get; set; }
	[Parameter] public string? PanelClass { get; set; }
	[Parameter] public bool TextOnSelect { get; set; } = true;
	[Parameter] public bool CloseOnSelect { get; set; } = true;
	[Parameter] public bool Border { get; set; }

	[Parameter] public EventCallback<string?> OnSelect { get; set; }

	//
	private string? _actual_text;
	private bool _short_bye;

	/// <inheritdoc />
	protected override void OnInitialized() =>
		_actual_text = Text;

	/// <inheritdoc />
	//protected override void OnParametersSet() =>
	//	ComponentClass = BuildClassName();

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
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", Cssc.Class(
			"dpd",
			Border.IfTrue("dpd-border"),
			(Alignment == TagAlignment.Right).IfTrue("dpd-right")));

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
	}

	#region IListAgent
	/// <inheritdoc />
	string? IListAgent.Tag => null;

	/// <inheritdoc />
	string? IListAgent.Class => null;

	/// <inheritdoc />
	async Task IListAgent.OnResponseAsync(ComponentProp component)
	{
		// 일단 닫는다
		if (CloseOnSelect)
		{
			_short_bye = true;

			// 주석 처리 했으니 안닫힘
			//StateHasChanged(); 
		}

		// 눌러 처리 -> 이 안에서 뭔가 하면 StateHasChange가 발동할 것이고, 그러면 자동으로 닫힐 것이다
		if (component is Nulo nulo)
		{
			if (nulo.Text is not null)
			{
				if (TextOnSelect)
					_actual_text = nulo.Text;

				await OnSelect.InvokeAsync(_actual_text);
			}
			else
			{
				if (TextOnSelect)
					_actual_text = Text;

				if (nulo.Id is not null)
					await OnSelect.InvokeAsync(nulo.Id);
			}
		}

		// 잠시 대기했다.... 원래대로
		try
		{
			await Task.Delay(1);

			if (CloseOnSelect)
				_short_bye = false;

			StateHasChanged();
		}
		catch
		{
			// 무슨 예외가 날지도 몰라서 그냥 해 둠
		}
	}
	#endregion
}
