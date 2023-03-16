namespace Du.Blazor.Components;

public class Tab : ComponentSubset<Tab, Tabs>
{
	[Parameter] public string? Text { get; set; }
	[Parameter] public bool Active { get; set; }
}


public class Tabs : ComponentContainer<Tab>
{
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Border { get; set; }
	[Parameter] public bool TabOnly { get; set; }
	[Parameter] public bool NoCurrentStart { get; set; } // 첨에 탭만 보이고, 선택이 없음

	[Parameter] public EventCallback<Tab> OnChange { get; set; }

	/// <inheritdoc />
	protected override bool SelectFirst => !NoCurrentStart;

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<CascadingValue Value="this" IsFixed="true">
				탭 처리
			</CascadingValue>
			<div class="tab">
				<div role="tablist">
					<button type="button" role="tab" aria-select="true" aria-controls="id" 
						 @onClick="" @onClick:StopPropagation="true">
						 탭 제목
					</button>
				</div>
				<div id="id" role="tablpanel">
					탭 내용
				</div>
			</div>
		 */
		// 먼저 등록
		builder.OpenComponent<CascadingValue<Tabs>>(0);
		builder.AddAttribute(1, "Value", this);
		builder.AddAttribute(2, "IsFixed", true);
		builder.AddAttribute(3, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(4, ChildContent)));
		builder.CloseComponent(); // CascadingValue<Tabs>

		// 탭 컨테이너
		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", Cssc.Class("tab", Border.IfTrue("tab-border")));
		builder.AddAttribute(12, "id", Id);

		// 탭 리스트
		builder.OpenElement(20, "div");
		builder.AddAttribute(21, "class", ActualClass);
		builder.AddAttribute(22, "role", "tablist");
		builder.AddMultipleAttributes(23, UserAttrs);

		// 탭 눌러
		foreach (var item in Items)
		{
			var cur = item == SelectedItem;

			builder.OpenElement(30, "button");
			builder.AddAttribute(31, "type", "button");
			builder.AddAttribute(32, "class", Cssc.Class(
				(Variant ?? Settings.Variant).ToCss(),
				cur.IfTrue("active"), 
				item.ActualClass));
			builder.AddAttribute(33, "role", "tab");
			builder.AddAttribute(34, "aria-selected", cur.ToHtml());
			builder.AddAttribute(35, "aria-controls", item.Id);
			builder.AddAttribute(36, "onclick", async () => await HandleTabClick(item));
			builder.AddEventStopPropagationAttribute(37, "onclick", true);
			builder.AddMultipleAttributes(38, item.UserAttrs);
			builder.AddContent(39, item.Text);
			builder.CloseElement(); // button, 탭 눌러
		}

		builder.CloseElement(); // div, 탭 리스트

		// 패널
		if (TabOnly is false)
		{
			foreach (var item in Items)
			{
				var cur = item == SelectedItem;

				builder.OpenElement(40, "div");
				builder.AddAttribute(41, "class", cur.IfTrue("active"));
				builder.AddAttribute(42, "role", "tabpanel");
				builder.AddAttribute(43, "id", item.Id);
				builder.AddContent(44, item.ChildContent);
				builder.CloseElement();
			}
		}

		builder.CloseElement(); // div, 탭 컨테이너
	}

	/// <inheritdoc />
	protected override async Task<bool> OnItemSelectedAsync(Tab? item, Tab? previous)
	{
		if (item is not null)
			await InvokeOnActive(item);

		return true;
	}

	//
	private Task HandleTabClick(Tab tab) =>
		SelectItemAsync(tab, true);

	//
	private Task InvokeOnActive(Tab tab) =>
		OnChange.HasDelegate is false
			? Task.CompletedTask
			: OnChange.InvokeAsync(tab);
}
