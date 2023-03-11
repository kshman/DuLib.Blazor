namespace Du.Blazor.Components;

public class Accd : ComponentContent, IAsyncDisposable
{
	[CascadingParameter] public Accds? Accds { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public bool Open { get; set; }

	// 
	internal bool InternalOpened { get; set; }

	/// <inheritdoc />
	protected override Task OnInitializedAsync()
	{
		ThrowIf.ContainerIsNull(this, Accds);

		FillAutoId();

		InternalOpened = Open;

		return Accds.AddItemAsync(this);
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		<div class="active">
			<button type="button" id="" @onClick="" @onClick:StopPropagation="true">
				제목
			</button>
			<div>
				내용
			</div>
		</div>
		 */
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", InternalOpened.IfTrue("active"));

		builder.OpenElement(10, "button");
		builder.AddAttribute(11, "type", "button");
		builder.AddAttribute(12, "class", ActualClass);
		builder.AddAttribute(13, "id", Id);
		builder.AddAttribute(14, "onclick", HandleOnClickAsync);
		builder.AddEventStopPropagationAttribute(15, "onclick", true);
		builder.AddMultipleAttributes(16, UserAttrs);
		builder.AddContent(17, Text);
		builder.CloseElement(); // button

		builder.OpenElement(20, "div");
		builder.AddContent(21, ChildContent);
		builder.CloseElement(); // div

		builder.CloseElement(); // div
	}

	//
	public async ValueTask DisposeAsync()
	{
		if (Accds is not null)
		{
			await Accds.RemoveItemAsync(this);
			Accds = null;
		}

		GC.SuppressFinalize(this);
	}

	//
	private async Task HandleOnClickAsync(MouseEventArgs e)
	{
		InternalOpened = !InternalOpened;

		if (Accds is not null)
			await Accds.HandleAccdAsync(this);
	}
}

public class Accds : ComponentContainer<Accd>
{
	[Parameter] public bool Separate { get; set; }
	[Parameter] public bool Border { get; set; }

	[Parameter] public EventCallback<ExpandEventArgs> OnExpand { get; set; }

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
				item.InternalOpened = false;
			else if (item.InternalOpened)
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
		<div class="accd accd-border">
			<CascadingValue Value="this" IsFixed="true">
				ACCD 아이템
			</CascadingValue>
		</div>
		 */
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", Cssc.Class("accd", Border.IfTrue("accd-border")));
		builder.AddMultipleAttributes(2, UserAttrs);

		builder.OpenComponent<CascadingValue<Accds>>(3);
		builder.AddAttribute(4, "Value", this);
		builder.AddAttribute(5, "IsFixed", true);
		builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(7, ChildContent)));
		builder.CloseComponent(); // CascadingValue<Accds>

		builder.CloseElement(); // div
	}

	//
	internal async Task HandleAccdAsync(Accd accd)
	{
		if (Separate)
			await InvokeOpenClose(accd.Id!, accd.InternalOpened);
		else
		{
			var prev = SelectedItem;

			if (prev != accd && prev is not null)
			{
				prev.InternalOpened = false;
				await InvokeOpenClose(prev.Id!, false);
			}

			await InvokeOpenClose(accd.Id!, accd.InternalOpened);
		}

		await SelectItemAsync(accd);
	}

	//
	private Task InvokeOpenClose(string id, bool open) =>
		OnExpand.HasDelegate is false
			? Task.CompletedTask
			: OnExpand.InvokeAsync(new ExpandEventArgs
			{
				Id = id,
				Expand = open,
			});
}
