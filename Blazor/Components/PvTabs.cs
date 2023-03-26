namespace Du.Blazor.Components;

/// <summary>
/// 피벗탭
/// </summary>
public class PvTabs : ComponentContainer<Subset>
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Border { get; set; }

	[Parameter] public EventCallback<Subset> OnChange { get; set; }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		// 먼저 등록
		builder.OpenComponent<CascadingValue<PvTabs>>(0);
		builder.AddAttribute(1, "Value", this);
		builder.AddAttribute(2, "IsFixed", true);
		builder.AddAttribute(3, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(4, ChildContent)));
		builder.CloseComponent(); // CascadingValue<PvTabs>

		//
		var count = Math.Clamp(Items.Count, 0, 6);
		if (count == 0)
		{
			// 헐?
			builder.OpenElement(10, "div");
			builder.AddContent(11, "Loading...");
			builder.CloseElement();
			return;
		}

		// 탭 컨테이너
		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", Cssc.Class("vtab cpvt", $"ld{count}", Border.IfTrue("bdr")));

		// 탭 눌러
		for (var i = 0; i < count; i++)
		{
			var item = Items[i];
			var active = item == SelectedItem;
			var variant = (item.Variant ?? Variant ?? Settings.Variant).ToCss();

			builder.OpenElement(20, "input");
			builder.AddAttribute(21, "class", "cpvtir");
			builder.AddAttribute(22, "id", item.Id);
			builder.AddAttribute(23, "type", "radio");
			builder.AddAttribute(24, "chcked", active);
			builder.AddAttribute(25, "onclick", async () => await HandleTabClick(item));
			builder.AddEventStopPropagationAttribute(26, "onclick", true);
			builder.CloseElement();

			builder.OpenElement(30, "label");
			builder.AddAttribute(31, "class", Cssc.Class(variant, "cpvtlb", active.IfTrue("active")));
			builder.AddAttribute(32, "for", item.Id);
			builder.AddContent(33, item.Text ?? $"[{item.Id}]");
			builder.CloseElement();
		}

		// 패널
		var select = SelectedItem is null ? 0 : Items.IndexOf(SelectedItem);
		var index = count - select - 1;

		builder.OpenElement(30, "div");
		builder.AddAttribute(31, "class", Cssc.Class("cpvtr", index == 0 ? null : $"i{index}"));

		for (var i = 0; i < count; i++)
		{
			var item = Items[i];

			builder.OpenElement(40, "div");
			builder.AddAttribute(41, "class", "cpvtp");
			builder.AddContent(42, item.ChildContent);
			builder.CloseElement();
		}

		builder.CloseElement();

		builder.CloseElement();
	}

	/// <inheritdoc />
	protected override async Task<bool> OnItemSelectedAsync(Subset? item, Subset? previous)
	{
		if (item is not null)
			await InvokeOnChange(item);

		return true;
	}

	//
	private Task HandleTabClick(Subset item) =>
		SelectItemAsync(item, true);

	//
	private Task InvokeOnChange(Subset item) =>
		OnChange.HasDelegate is false
			? Task.CompletedTask
			: OnChange.InvokeAsync(item);
}
