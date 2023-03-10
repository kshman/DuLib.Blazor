namespace Du.Blazor.Components;

public class Accd : ComponentObject, IAsyncDisposable
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
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", Cssc.Class("item", InternalOpened.IfTrue("active")));

		builder.OpenElement(10, "button");
		builder.AddAttribute(11, "type", "button");
		builder.AddAttribute(12, "class", Cssc.Class("nulo", Class));
		builder.AddAttribute(13, "id", Id);
		builder.AddAttribute(14, "onclick", HandleOnClickAsync);
		builder.AddEventStopPropagationAttribute(15, "onclick", true);
		builder.AddMultipleAttributes(16, UserAttrs);
		builder.AddContent(17, Text);
		builder.CloseElement(); // button

		builder.OpenElement(20, "div");
		builder.AddAttribute(21, "class", "pnl");
		builder.AddContent(22, ChildContent);
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

	[Parameter] public EventCallback<string> OnOpen { get; set; }
	[Parameter] public EventCallback<string> OnClose { get; set; }

	/// <inheritdoc />
	protected override bool SelectFirst => false;

	/// <inheritdoc />
	protected override async Task OnAfterFirstRenderAsync()
	{
		if (Separate)
			return;

		var needStateChange = false;

		foreach (var item in Items)
		{
			if (needStateChange)
				item.InternalOpened = false;
			else if (item.InternalOpened)
			{
				needStateChange = true;
				await SelectItemAsync(item);
			}
		}

		if (needStateChange)
			StateHasChanged();
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", Cssc.Class("accd", Border.IfTrue("bdr")));

		builder.OpenComponent<CascadingValue<Accds>>(2);
		builder.AddAttribute(3, "Value", this);
		builder.AddAttribute(4, "IsFixed", true);
		builder.AddAttribute(5, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(6, ChildContent)));
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
		open ? OnOpen.InvokeAsync(id) : OnClose.InvokeAsync(id);
}
