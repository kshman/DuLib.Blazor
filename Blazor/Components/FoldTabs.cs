namespace Du.Blazor.Components;

/// <summary>
/// 접는탭
/// </summary>
public class FoldTabs : ComponentContainer<Subset>
{
	[Parameter] public RenderFragment? ChildContent { get; set; }
	[Parameter] public Variant? Variant { get; set; }
	[Parameter] public bool Separate { get; set; }
	[Parameter] public bool Border { get; set; }

	[Parameter] public EventCallback<ActiveEventArgs> OnActive { get; set; }

	/// <inheritdoc />
	protected override bool SelectFirst => false;

	/// <inheritdoc />
	protected override async Task OnAfterFirstRenderAsync()
	{
		if (Separate)
			return;

		var opened = false;
		foreach (var item in Items)
		{
			if (opened)
				item.InternalActive = false;
			else if (item.InternalActive)
			{
				opened = true;
				await SelectItemAsync(item, true);
			}
		}
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
			<CascadingValue Value="this" IsFixed="true">
				탭 처리
			</CascadingValue>
			<div class="cacd bdr">
				<div class="cacdi">
					<button type="button" class="cacdb active" id @onClick>
						제목
					</button>
					<div class="cacdp active">
						내용
					</div>
				</div>
			</div>
		 */

		// 먼저 등록
		builder.OpenComponent<CascadingValue<FoldTabs>>(0);
		builder.AddAttribute(1, "Value", this);
		builder.AddAttribute(2, "IsFixed", true);
		builder.AddAttribute(3, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(4, ChildContent)));
		builder.CloseComponent(); // CascadingValue<FoldTabs>

		// 컨테이너
		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", Cssc.Class("cacd", Border.IfTrue("bdr")));
		builder.AddMultipleAttributes(12, UserAttrs);

		// 아이템
		foreach (var item in Items)
		{
			var active = item.InternalActive.IfTrue("active");
			var variant = (item.Variant ?? Variant ?? Settings.Variant).ToCss();

			builder.OpenElement(20, "div");
			builder.AddAttribute(21, "class", "cacdi");

			builder.OpenElement(30, "button");
			builder.AddAttribute(31, "type", "button");
			builder.AddAttribute(32, "class", Cssc.Class(variant, "cacdb", active, item.Class));
			builder.AddAttribute(33, "id", Id);
			builder.AddAttribute(34, "onclick", async () => await HandleItemOnClickAsync(item));
			builder.AddEventStopPropagationAttribute(35, "onclick", true);
			builder.AddMultipleAttributes(36, item.UserAttrs);
			builder.AddContent(37, item.Text);
			builder.CloseElement(); // button

			builder.OpenElement(40, "div");
			builder.AddAttribute(41, "class", Cssc.Class("cacdp", active));
			builder.AddContent(42, item.ChildContent);
			builder.CloseElement(); // div

			builder.CloseElement(); // div 
		}

		builder.CloseElement(); // div, 컨테이너
	}

	//
	private async Task HandleItemOnClickAsync(Subset item)
	{
		item.InternalActive = !item.InternalActive;

		if (Separate)
			await InvokeOpenClose(item);
		else
		{
			var prev = SelectedItem;

			if (prev != item && prev is not null)
			{
				prev.InternalActive = false;
				await InvokeOpenClose(prev);
			}

			await InvokeOpenClose(item);
		}

		await SelectItemAsync(item);
	}

	//
	private Task InvokeOpenClose(Subset item) =>
		OnActive.HasDelegate is false
			? Task.CompletedTask
			: OnActive.InvokeAsync(new ActiveEventArgs(item, item.InternalActive));
}
