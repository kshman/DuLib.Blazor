﻿using Microsoft.AspNetCore.Components.Routing;

namespace Du.Blazor.Components;

/// <summary>
/// 나브 링크
/// </summary>
public class NavBtn : Nulo, IDisposable
{
	[Parameter] public NavLinkMatch Match { get; set; }
	[Parameter] public string? Link { get; set; }

	//
	[Inject] private NavigationManager NavMan { get; set; } = default!;

	//
	private bool _isActive;
	private string? _href;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		NavMan.LocationChanged += OnLocationChanged;
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_href = Link.Empty() ? null : NavMan.ToAbsoluteUri(Link).AbsoluteUri;
		_isActive = ShouldMatch(NavMan.Uri);

		ComponentClass = GetNuloClassName("cnvbn", _isActive.IfTrue("active"), false);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "a");
		builder.AddAttribute(1, "class", ActualClass);
		builder.AddAttribute(2, "href", Link);
		builder.AddMultipleAttributes(3, UserAttrs);
		builder.AddContent(4, Text);
		builder.AddContent(5, ChildContent);
		builder.CloseElement(); // a
	}

	//
	private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		if (ResponseHandler is not null)
			InvokeAsync(() => ResponseHandler.OnResponseAsync(this));

		var active = ShouldMatch(e.Location);

		if (active == _isActive)
			return;

		_isActive = active;
		StateHasChanged();
	}

	//
	private bool ShouldMatch(string uri)
	{
		if (_href is null)
			return false;
		if (EqualsHrefExactlyOrIfTrailingSlashAdded(uri))
			return true;
		if (Match == NavLinkMatch.Prefix && IsStrictlyPrefixWithSeparator(uri, _href))
			return true;
		return false;
	}

	//
	private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string uri)
	{
		if (string.Equals(uri, _href, StringComparison.OrdinalIgnoreCase))
			return true;
		if (uri.Length != _href!.Length - 1)
			return false;
		if (_href[^1] != '/' || !_href.StartsWith(uri, StringComparison.OrdinalIgnoreCase))
			return false;
		return true;
	}

	//
	private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
	{
		var l = prefix.Length;
		if (value.Length > l)
		{
			return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
							&& (l == 0
								|| !char.IsLetterOrDigit(prefix[l - 1])
								|| !char.IsLetterOrDigit(value[l]));
		}
		return false;
	}

	//
	public void Dispose() =>
		NavMan.LocationChanged -= OnLocationChanged;
}