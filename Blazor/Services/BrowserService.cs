namespace Du.Blazor.Services;

public class BrowserService : IBrowserService, IAsyncDisposable
{
	private readonly IJSRuntime _js;
	private IJSObjectReference? _md;

	private IDictionary<string, string>? _cookies;

	public BrowserService(IJSRuntime js)
	{
		_js = js;
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		if (_md is not null)
		{
			await _md.DisposeAsync();
			_md = null;
		}
	}

	// 
	private async Task PrepareModuleAsync()
	{
		_md ??= await _js.InvokeAsync<IJSObjectReference>("import",
			"./_content/DuLib.Blazor/browserservice.js");
	}

	//
	private async Task<IDictionary<string, string>> PrepareCookieAsync()
	{
		await PrepareModuleAsync();

		if (_md is null)
			return new Dictionary<string, string>();

		return (await _md.InvokeAsync<string>("ckrd"))
			.Split(';')
			.Select(x => x.Split('='))
			.ToDictionary(k => k.First().Trim(), v => v.Last().Trim());
	}

	/// <inheritdoc />
	public async Task<string?> GetCookieAsync(string name, bool refresh = false)
	{
		if (refresh)
			_cookies = await PrepareCookieAsync();
		else
			_cookies ??= await PrepareCookieAsync();

		return _cookies.TryGetValue(name, out var value) ? value : null;
	}

	/// <inheritdoc />
	public async Task SetCookieAsync(string name, string value, int? days = null)
	{
		await PrepareModuleAsync();

		if (_md is null)
			return;

		await _md.InvokeVoidAsync("ckwr", name, value, days);
	}

	/// <inheritdoc />
	public async Task<BrowserDimension> GetDimensionAsync()
	{
		await PrepareModuleAsync();

		if (_md is null)
			return new BrowserDimension { Width = 0, Height = 0 };

		var dim = await _md.InvokeAsync<BrowserDimension>("getdim");
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
	/// <summary>
	/// 쿠키를 얻자
	/// </summary>
	/// <param name="name">쿠키 이름</param>
	/// <param name="refresh">내부 데이터를 갱신하려면 true</param>
	/// <returns>얻은 값. 없으면 당근 null</returns>
	Task<string?> GetCookieAsync(string name, bool refresh = false);
	/// <summary>
	/// 쿠키를 넣자
	/// </summary>
	/// <param name="name">쿠키 이름</param>
	/// <param name="value">넣을 값</param>
	/// <param name="days">보존 기간. 기본값이 null로 무제한일껄</param>
	/// <returns></returns>
	Task SetCookieAsync(string name, string value, int? days = null);
	/// <summary>
	/// 브라우저의 표시 크기를 얻는다
	/// </summary>
	/// <returns></returns>
	public Task<BrowserDimension> GetDimensionAsync();
}

public static class BrowserServiceExtension
{
	public static IServiceCollection AdDuBrowserServiceScoped(this IServiceCollection services)
	{
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		services.AddScoped<IBrowserService, BrowserService>();

		return services;
	}

	public static IServiceCollection AdDuBrowserServiceTransient(this IServiceCollection services,
		bool true_for_scope_or_false_for_transient = true)
	{
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		services.AddTransient<IBrowserService, BrowserService>();

		return services;
	}
}
