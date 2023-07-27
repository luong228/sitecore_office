namespace sitecorepro3.Foundation.SitecoreExtensions.Repositories
{
    using Sitecore.Mvc.Presentation;

    public interface IRenderingPropertiesRepository
  {
    T Get<T>(Rendering rendering);
  }
}