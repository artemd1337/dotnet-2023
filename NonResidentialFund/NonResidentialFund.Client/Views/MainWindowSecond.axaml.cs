using Avalonia.ReactiveUI;
using NonResidentialFund.Client.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace NonResidentialFund.Client.Views;

public partial class MainWindowSecond : ReactiveWindow<MainWindowSecondViewModel>
{
    private IDisposable _contentHandlersSubscription = Disposable.Empty;

    public MainWindowSecond()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is not MainWindowSecondViewModel viewModel)
                return;

            // При замене Content:
            // - отписываем обработчики для старой вьюмодели
            // - регистрируем обработчики для новой вьюмодели
            viewModel
                .WhenAnyValue(viewModel => viewModel.Content)
                .Subscribe(values =>
                {
                    _contentHandlersSubscription.Dispose();
                    _contentHandlersSubscription = RegisterInteractionHandlers(viewModel.Content);
                });

            // При деактивации окна отписываем обработчики у последней вьюмодели
            d(Disposable.Create(() => _contentHandlersSubscription.Dispose()));
        });
    }

    private IDisposable RegisterInteractionHandlers(TableViewModelBase tableViewModel)
    {
        return new CompositeDisposable
        {
            tableViewModel.ShowDialog.RegisterHandler(ShowOrganizationDialogAsync),
            // Зарегистрировать другие обработчики при необходимости
        };
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
