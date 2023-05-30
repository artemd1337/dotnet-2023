using Avalonia.Controls;
using Avalonia.ReactiveUI;
using NonResidentialFund.Client.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace NonResidentialFund.Client.Views;
public partial class OrganizationsView : ReactiveUserControl<OrganizationsViewModel>
{
    public OrganizationsView()
    {
        InitializeComponent();
    }
}
