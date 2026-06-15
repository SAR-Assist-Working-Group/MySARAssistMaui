using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MySARAssist.ViewModels.Urgency
{
    public enum UrgencyResult
    {
        NotDetermined,
        High,
        Intermediate,
        Low,
        WillNotRespond
    }

    public class UrgencyAnswerOption
    {
        public string Text { get; init; }
        public ICommand SelectCommand { get; init; }
    }

    public class UrgencyCalculatorViewModel : ObservableObject
    {
        private record QuestionDef(string Text, List<AnswerDef> Answers);
        private record AnswerDef(string Text, UrgencyResult? DirectResult);

        private readonly List<QuestionDef> _questions;
        private int _currentIndex;
        private UrgencyResult _result = UrgencyResult.NotDetermined;
        private bool _isShowingResult;

        public UrgencyCalculatorViewModel()
        {
            _questions = BuildQuestions();
            ResetCommand = new Command(Reset);
            LoadCurrentQuestion();
        }

        private static List<QuestionDef> BuildQuestions() =>
        [
            new("Does the subject pose a risk to searchers?",
            [
                new("Yes", UrgencyResult.WillNotRespond),
                new("No", null)
            ]),
            new("Are there known medical issues, significant injuries, or mental-state issues?",
            [
                new("Yes", UrgencyResult.High),
                new("No / Unknown", null)
            ]),
            new("Are there any high-hazard areas present?",
            [
                new("Yes", UrgencyResult.High),
                new("No / Unknown", null)
            ]),
            new("Is the subject's age a factor?",
            [
                new("Yes", UrgencyResult.High),
                new("Age is not remarkable", null)
            ]),
            new("Are there any hazards in the current or forecast weather?",
            [
                new("Yes", UrgencyResult.High),
                new("No", null)
            ]),
            new("Is diminishing daylight a factor in the response?",
            [
                new("Yes", UrgencyResult.High),
                new("No", null)
            ]),
            new("Are the subject's equipment/supplies sufficient?",
            [
                new("No", UrgencyResult.High),
                new("Yes / Unknown", null)
            ]),
            new("Are there other factors significantly impacting survival probability?",
            [
                new("Yes", UrgencyResult.Intermediate),
                new("No", UrgencyResult.Low)
            ])
        ];

        public ICommand ResetCommand { get; }

        private ObservableCollection<UrgencyAnswerOption> _currentAnswers = [];
        public ObservableCollection<UrgencyAnswerOption> CurrentAnswers
        {
            get => _currentAnswers;
            private set { _currentAnswers = value; OnPropertyChanged(); }
        }

        public string QuestionProgressText => $"Question {_currentIndex + 1} of {_questions.Count}";
        public double ProgressValue => (double)(_currentIndex + 1) / _questions.Count;
        public string QuestionText => _questions[_currentIndex].Text;

        public bool IsShowingQuestion => !_isShowingResult;
        public bool IsShowingResult => _isShowingResult;

        public string ResultTitle => _result switch
        {
            UrgencyResult.High => "HIGH URGENCY",
            UrgencyResult.Intermediate => "INTERMEDIATE URGENCY",
            UrgencyResult.Low => "LOW URGENCY",
            UrgencyResult.WillNotRespond => "SAR WILL NOT RESPOND",
            _ => string.Empty
        };

        public string ResultDescription => _result switch
        {
            UrgencyResult.High => "Immediate response is required. The subject faces significant risk to their survival.",
            UrgencyResult.Intermediate => "Prompt response is warranted. Monitor for changes that may escalate urgency.",
            UrgencyResult.Low => "Response can proceed in a measured manner. The subject is unlikely to be in immediate danger.",
            UrgencyResult.WillNotRespond => "SAR cannot safely respond under current conditions.",
            _ => string.Empty
        };

        public Color ResultBackgroundColor => _result switch
        {
            UrgencyResult.High => Color.FromArgb("#C0392B"),
            UrgencyResult.Intermediate => Color.FromArgb("#B07D0A"),
            UrgencyResult.Low => Color.FromArgb("#2E6B20"),
            UrgencyResult.WillNotRespond => Color.FromArgb("#1A1A1A"),
            _ => Colors.Transparent
        };

        private void LoadCurrentQuestion()
        {
            var question = _questions[_currentIndex];
            CurrentAnswers = new ObservableCollection<UrgencyAnswerOption>(
                question.Answers.Select(a => new UrgencyAnswerOption
                {
                    Text = a.Text,
                    SelectCommand = new Command(() => ProcessAnswer(a))
                }));
            OnPropertyChanged(nameof(QuestionText));
            OnPropertyChanged(nameof(QuestionProgressText));
            OnPropertyChanged(nameof(ProgressValue));
        }

        private void ProcessAnswer(AnswerDef answer)
        {
            if (answer.DirectResult.HasValue)
            {
                ShowResult(answer.DirectResult.Value);
            }
            else
            {
                _currentIndex++;
                LoadCurrentQuestion();
            }
        }

        private void ShowResult(UrgencyResult result)
        {
            _result = result;
            _isShowingResult = true;
            OnPropertyChanged(nameof(IsShowingResult));
            OnPropertyChanged(nameof(IsShowingQuestion));
            OnPropertyChanged(nameof(ResultTitle));
            OnPropertyChanged(nameof(ResultDescription));
            OnPropertyChanged(nameof(ResultBackgroundColor));
        }

        private void Reset()
        {
            _currentIndex = 0;
            _result = UrgencyResult.NotDetermined;
            _isShowingResult = false;
            LoadCurrentQuestion();
            OnPropertyChanged(nameof(IsShowingResult));
            OnPropertyChanged(nameof(IsShowingQuestion));
        }
    }
}
