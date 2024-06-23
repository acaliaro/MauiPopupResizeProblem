using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;

namespace MauiApp1
{
    public partial class LoaderProblemViewModel : ObservableObject
    {
        [ObservableProperty] bool isBusy;

        public async Task SetBusyAsync()
        {
            //await Task.Delay(250);
            IsBusy = true;
            //await Task.Delay(250);
        }
        public async Task SetNotBusyAsync()
        {
            //await Task.Delay(250);
            IsBusy = false;
            await Task.Delay(550);
        }

        [RelayCommand]
        async Task ShowLoaderAsync()
        {

            try
            {
                // await SetBusyAsync();
                IsBusy = true;
                await Task.Delay(50);

                Task.Run(async () =>
                {

                    Console.WriteLine("Another thread...");
                    await Task.Delay(1000);
                    IsBusy = false;
                });

                await Shell.Current.DisplayAlert("Warning", "Time is passed!!", "Ok");
                await Shell.Current.GoToAsync("..", animate: true);

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                // await SetNotBusyAsync();
                IsBusy = false;

                //await Shell.Current.GoToAsync("..", animate: true);
            }
        }


        partial void OnIsBusyChanged(bool value)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (value)
                {
                    try
                    {
                        if (App.LoadingView == null)
                        {
                            App.LoadingView = new LoadingView();

                            Shell.Current.ShowPopup(App.LoadingView);
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        if (App.LoadingView != null)
                        {
                            App.LoadingView.Close();
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        App.LoadingView = null;
                    }
                }
            });
        }
    }
}
