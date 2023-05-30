using AutoMapper;
using Microsoft.VisualBasic;
using NonResidentialFund.Client.Views;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace NonResidentialFund.Client.ViewModels;
public class OrganizationsViewModel : TableViewModelBase
{
    public ObservableCollection<OrganizationViewModel> Organizations { get; } = new();
    private OrganizationViewModel? _selectedOrganization;
    public OrganizationViewModel? SelectedOrganization
    {
        get => _selectedOrganization;
        set => this.RaiseAndSetIfChanged(ref _selectedOrganization, value);
    }

    public ReactiveCommand<Unit, Unit> OnAddOrganizationCommand { get; set; }
    public ReactiveCommand<Unit, Unit> OnUpdateOrganizationCommand { get; set; }
    public ReactiveCommand<Unit, Unit> OnDeleteOrganizationCommand { get; set; }
    public ReactiveCommand<Unit, Unit> Cancel { get; }
    private readonly ApiWrapper _apiClient;
    private readonly IMapper _mapper;

    public OrganizationsViewModel()
    {
        _apiClient = Locator.Current.GetService<ApiWrapper>();
        _mapper = Locator.Current.GetService<IMapper>();

        ShowDialog = new Interaction<OrganizationViewModel, OrganizationViewModel?>();

        OnAddOrganizationCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var organizationViewModel = await ShowDialog.Handle(new OrganizationViewModel());
            if (organizationViewModel != null)
            {
                var newOrganization = await _apiClient.AddOrganizationAsync(_mapper.Map<OrganizationPostDto>(organizationViewModel));
                Organizations.Add(_mapper.Map<OrganizationViewModel>(newOrganization));
            }
        });
        OnUpdateOrganizationCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var organizationViewModel = await ShowDialog.Handle(SelectedOrganization!);
            if (organizationViewModel != null)
            {
                var newOrganization = await _apiClient.UpdateOrganizationAsync(SelectedOrganization!.OrganizationId,
                    _mapper.Map<OrganizationPostDto>(SelectedOrganization));
                _mapper.Map(organizationViewModel, SelectedOrganization);
            }
        }, this.WhenAnyValue(vm => vm.SelectedOrganization).Select(selectedOrganization => selectedOrganization != null));
        OnDeleteOrganizationCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await _apiClient.DeleteOrganizationAsync(SelectedOrganization!.OrganizationId);
            Organizations.Remove(SelectedOrganization);
        }, this.WhenAnyValue(vm => vm.SelectedOrganization).Select(selectedOrganization => selectedOrganization != null));
        Cancel = ReactiveCommand.Create(() => { });

        RxApp.MainThreadScheduler.Schedule(LoadOrganizationsAsync);
    }

    private async void LoadOrganizationsAsync()
    {
        var organizations = await _apiClient.GetOrganizationsAsync();
        foreach (var organization in organizations)
        {
            Organizations.Add(_mapper.Map<OrganizationViewModel>(organization));
        }
    }
}
