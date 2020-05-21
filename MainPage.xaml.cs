using System;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace App1
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>

    public sealed partial class MainPage : Page
    {
        private SpeechRecognizer constSpeechRecognizer;
        private CoreDispatcher dispatcher;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            constSpeechRecognizer = new SpeechRecognizer();
            await constSpeechRecognizer.CompileConstraintsAsync();

            constSpeechRecognizer.HypothesisGenerated += ContinuousRecognitionSession_HypothesisGenerated;
            constSpeechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;

            constSpeechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Conpleated;

            await constSpeechRecognizer.ContinuousRecognitionSession.StartAsync();
        }

        private async void ContinuousRecognitionSession_Conpleated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                textBlock1.Text = "Timeout";
                });

            await constSpeechRecognizer.ContinuousRecognitionSession.StartAsync();
        }

        private async void ContinuousRecognitionSession_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    textBlock1.Text = args.Hypothesis.Text;
                });
        }

        private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                textBlock1.Text = "Waiting...";
                output.Text += args.Result.Text + "。\n";
            });
        }
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
