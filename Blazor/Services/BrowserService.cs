namespace Du.Blazor.Services;

public class BrowserService : IBrowserService
{
	private readonly IJSRuntime _js;

	public BrowserService(IJSRuntime js)
	{
		_js = js;
	}

	/// <inheritdoc />
	public async Task<BrowserDimension> GetDimensionAsync()
	{
		var dim = await _js.InvokeAsync<BrowserDimension>("DUBZ.getdim");
		return dim;
	}
}

public class BrowserDimension
{
	public int Width { get; set; }
	public int Height { get; set; }
}

public interface IBrowserService
{
	public Task<BrowserDimension> GetDimensionAsync();
}

public static class BrowserServiceExtension
{
	public static IServiceCollection AdDuBrowserService(this IServiceCollection services)
	{
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		services.AddScoped<IBrowserService, BrowserService>();

		return services;
	}
}
