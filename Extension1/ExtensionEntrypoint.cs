using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using System.Windows.Media.Imaging;

namespace Extension1
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "Extension1.44040ec7-edbb-4663-a60b-27c3ad97147f",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Publisher name",
                    displayName: "Extension1",
                    description: "Extension description"),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
            serviceCollection.AddSingleton<IBitmapSourceVisualizer, BitmapSourceVisualizer>();
        }
    }

    public interface IBitmapSourceVisualizer
    {
        void Visualize(BitmapSource bitmapSource);
    }

    public class BitmapSourceVisualizer : IBitmapSourceVisualizer
    {
        public void Visualize(BitmapSource bitmapSource)
        {
            // Implement the visualization logic here
            // For example, you can display the BitmapSource in a new window
            var window = new System.Windows.Window
            {
                Title = "BitmapSource Visualizer",
                Content = new System.Windows.Controls.Image { Source = bitmapSource },
                Width = 800,
                Height = 600
            };
            window.ShowDialog();
        }
    }
}
