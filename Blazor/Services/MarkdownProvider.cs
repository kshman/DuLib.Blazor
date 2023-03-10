using Markdig;

namespace Du.Blazor.Services;

public class MarkdownProvider : IMarkdownProvider
{
	public MarkdownProvider() =>
		Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

	public MarkdownPipeline Pipeline { get; }

	public string GetHtml(string markdown) =>
		Markdown.ToHtml(markdown, Pipeline);

	public MarkupString GetMarkup(string markdown) =>
		(MarkupString)Markdown.ToHtml(markdown, Pipeline);
}

public interface IMarkdownProvider
{
	public MarkdownPipeline Pipeline { get; }
	public string GetHtml(string markdown);
	public MarkupString GetMarkup(string markdown);
}

public static class MarkdownProviderExtension
{
	public static IServiceCollection AddMarkdownProvider(this IServiceCollection services)
	{
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		services.AddScoped<IMarkdownProvider, MarkdownProvider>();

		return services;
	}
}
