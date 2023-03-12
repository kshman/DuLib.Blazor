namespace Du.Blazor.Components;

/// <summary>
/// 드랍 다운
/// </summary>
public class DropBtn : Nulo, IListAgent
{
	[Parameter] public TagDirection? Direction { get; set; }
	[Parameter] public string? PanelClass { get; set; }
	[Parameter] public bool TextOnSelect { get; set; } = true;

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
		builder.AddAttribute(1, "class", "dpd");

		builder.OpenElement(10, "button");
		builder.AddAttribute(11, "type", "button");
		builder.AddAttribute(12, "class", ActualClass);
		builder.AddAttribute(14, "onclick", HandleOnClickAsync);
		builder.AddEventStopPropagationAttribute(15, "onclick", true);
		builder.AddMultipleAttributes(16, UserAttrs);
		builder.AddContent(17, _actual_text);
		builder.CloseElement(); // button

		builder.OpenElement(20, "div");
		builder.AddContent(21, ChildContent);
		builder.CloseElement(); // div

		builder.CloseElement(); // div
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
			_actual_text = nulo.Text;

		return Task.CompletedTask;
	}
	#endregion
}
