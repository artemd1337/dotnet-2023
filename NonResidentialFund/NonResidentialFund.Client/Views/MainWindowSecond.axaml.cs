using Avalonia.Controls;
using Avalonia.ReactiveUI;
using NonResidentialFund.Client.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace NonResidentialFund.Client.Views;

public partial class MainWindowSecond : ReactiveWindow<MainWindowSecondViewModel>
{

    public MainWindowSecond()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Content.ShowDialog.RegisterHandler(ShowOrganizationDialogAsync)));
    }

    private async Task ShowOrganizationDialogAsync(InteractionContext<OrganizationViewModel, OrganizationViewModel?> interaction)
    {
        var dialog = new OrganizationWindow
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<OrganizationViewModel?>(this);
        interaction.SetOutput(result);
    }
}

