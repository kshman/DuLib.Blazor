namespace Du.Blazor.Components;

/// <summary>
/// 드랍 다운
/// </summary>
public class DropBtn : Nulo, IListAgent
{
	[Parameter] public TagAlignment? Alignment { get; set; }
	[Parameter] public string? PanelClass { get; set; }
	[Parameter] public bool TextOnSelect { get; set; } = true;
	[Parameter] public bool Border { get; set; }

	//
	private string? _actual_text;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		_actual_text = Text;
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		ComponentClass = Cssc.Class("nulo", VariantString);
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
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", Cssc.Class(
			"dpd",
			Border.IfTrue("dpd-border"),
			(Alignment == TagAlignment.Right).IfTrue("dpd-align-right")));

		builder.OpenElement(10, "button");
		builder.AddAttribute(11, "type", "button");
		builder.AddAttribute(12, "class", ActualClass);
		builder.AddAttribute(14, "onclick", HandleOnClickAsync);
		builder.AddEventStopPropagationAttribute(15, "onclick", true);
		builder.AddMultipleAttributes(16, UserAttrs);
		builder.AddContent(17, _actual_text);
		builder.CloseElement(); // button

		builder.OpenElement(20, "div");
		builder.AddAttribute(21, "class", PanelClass);

		builder.OpenComponent<CascadingValue<DropBtn>>(22);
		builder.AddAttribute(23, "Value", this);
		builder.AddAttribute(24, "IsFixed", true);
		builder.AddAttribute(25, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(7, ChildContent)));
		builder.CloseComponent(); // CascadingValue<Accds>, 콘텐트

		builder.CloseElement(); // div, 패널

		builder.CloseElement(); // div, 메인
	}

	#region IListAgent
	/// <inheritdoc />
	string? IListAgent.Tag => null;

	/// <inheritdoc />
	string? IListAgent.Class => null;

	/// <inheritdoc />
	Task IListAgent.OnResponseAsync(ComponentProp component)
	{
		if (!TextOnSelect)
			return Task.CompletedTask;

		if (component is Nulo nulo)
		{
			_actual_text = nulo.Text;

			if (_actual_text.WhiteSpace())
				_actual_text = Text;

			StateHasChanged();
		}

		return Task.CompletedTask;
	}
	#endregion
}
