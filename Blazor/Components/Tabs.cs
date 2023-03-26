namespace Du.Blazor.Components;

/// <summary>
/// 탭
/// </summary>
public class Tabs : ComponentContainer<Subset>
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Border { get; set; }
	[Parameter] public bool TabOnly { get; set; }
	[Parameter] public bool TabStart { get; set; } // 첨에 탭만 보이고, 내용이 안보임. 즉 선택 ㄴㄴ

	[Parameter] public EventCallback<Subset> OnChange { get; set; }

	/// <inheritdoc />
	protected override bool SelectFirst => !TabStart;

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<CascadingValue Value="this" IsFixed="true">
				탭 처리
			</CascadingValue>
			<div class="ctab bdr">
				<div class="ctabl">
					<button type="button" class="ctabb" aria-select="true" aria-controls @onClick>
						 탭 제목
					</button>
				</div>
				<div class="ctabp" id="id">
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
		builder.AddAttribute(11, "class", Cssc.Class("vtab ctab", Border.IfTrue("bdr")));

		// 탭 리스트
		builder.OpenElement(20, "div");
		builder.AddAttribute(21, "class", Cssc.Class("ctabl", Class));
		builder.AddAttribute(22, "role", "tablist");
		builder.AddMultipleAttributes(23, UserAttrs);

		// 탭 눌러
		foreach (var item in Items)
		{
			var active = item == SelectedItem;
			var variant = (item.Variant ?? Variant ?? Settings.Variant).ToCss();

			builder.OpenElement(30, "button");
			builder.AddAttribute(31, "type", "button");
			builder.AddAttribute(32, "class", Cssc.Class(variant, "ctabb", active.IfTrue("active"), item.Class));
			builder.AddAttribute(33, "role", "tab");
			builder.AddAttribute(34, "aria-selected", active.ToHtml());
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
				var active = item == SelectedItem;

				builder.OpenElement(40, "div");
				builder.AddAttribute(41, "class", Cssc.Class("ctabp", active.IfTrue("active")));
				builder.AddAttribute(42, "role", "tabpanel");
				builder.AddAttribute(43, "id", item.Id);
				builder.AddContent(44, item.ChildContent);
				builder.CloseElement();
			}
		}

		builder.CloseElement(); // div, 탭 컨테이너
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
