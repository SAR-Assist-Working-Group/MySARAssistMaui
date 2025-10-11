using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using MySARAssist.Models.Events;
using MySarAssistModels.RADeMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MySARAssist.ViewModels.RADeMS
{
    public class RADeMSDetailsViewModel : ObservableObject
    {
        private RADeMSCategory? _rademsCateogry = null;
        private RADeMSScore _rademsScore = new RADeMSScore();
        public RADeMSCategory? rademsCateogry { get => _rademsCateogry; private set => _rademsCateogry = value; }
        public RADeMSScore rademsScore { get => _rademsScore; }

        RADeMSEventHandler? eventHandler = null;

        public string SetByName { get => rademsScore.SetByName; set => rademsScore.SetByName = value; }
        public string Comment { get => rademsScore.Comment; set => rademsScore.Comment = value; }
        public RADeMSDetailsViewModel()
        {
            ShareScoreCommand = new Command(async () => await OnShareScoreCommand());
            ViewCardCommand = new Command(async () => OnViewCardCommand());
            if (rademsCateogry != null) { rademsScore.CategoryID = rademsCateogry.ID; }

            if (App.CurrentPerson != null) { SetByName = App.CurrentPerson.Name; }
            OnPropertyChanged(nameof(ScoreColor));
            OnPropertyChanged(nameof(RademsResultText));
            OnPropertyChanged(nameof(ShortCodeText));

        }

     
        public void SetRademsType(string typeIdStr)
        {
            int typeId = 0;
            if (int.TryParse(typeIdStr, out typeId))
            {
                rademsCateogry = RADeMSTools.GetCategory(typeId);
                if (rademsCateogry != null) { rademsScore.CategoryID = rademsCateogry.ID; }


                OnPropertyChanged(nameof(CategoryTitle));
                OnPropertyChanged(nameof(OperationalRiskQuestions));
                OnPropertyChanged(nameof(ResponseCapacityQuestions));
                OnPropertyChanged(nameof(ShortCodeText));

            }

        }


        public string CategoryTitle
        {
            get
            {
                if (_rademsCateogry == null) { return string.Empty; }
                return _rademsCateogry.Name;
            }
        }

        private List<RADeMSQuestionViewModel>? _AllQuestionViewModels;
        public List<RADeMSQuestionViewModel> AllQuestionViewModels
        {
            get
            {
                if ((_AllQuestionViewModels == null || !_AllQuestionViewModels.Any()) && rademsCateogry != null && rademsCateogry.Questions != null)
                {
                    List<RADeMSQuestionViewModel> models = new List<RADeMSQuestionViewModel>();
                    foreach (RADeMSQuestion q in rademsCateogry.Questions)
                    {
                        models.Add(new RADeMSQuestionViewModel { question = q, SelectedValue = 0 });
                    }

                    foreach (RADeMSQuestionViewModel model in models)
                    {
                        model.ScoreChanged += Model_ScoreChanged;
                    }
                    _AllQuestionViewModels = models;
                }

                if (_AllQuestionViewModels != null)
                {
                    return _AllQuestionViewModels;
                }
                else { return new List<RADeMSQuestionViewModel>(); }
            }
        }

        private void Model_ScoreChanged(RADeMSEventArgs e)
        {
            if (e != null && rademsCateogry != null && rademsCateogry.Questions != null)
            {
                if (rademsCateogry.Questions.Any(o => o.ID == e.question.ID))
                {
                    //get the index of that question
                    RADeMSQuestion question = rademsCateogry.Questions.First(o => o.ID == e.question.ID);
                    int index = rademsCateogry.Questions.IndexOf(question);
                    rademsScore.Scores[index] = e.SelectedValue;

                    OnPropertyChanged(nameof(RademsResultText));
                    OnPropertyChanged(nameof(ScoreColor));
                    OnPropertyChanged(nameof(ShortCodeText));
                }
            }
        }

        public List<RADeMSQuestionViewModel> OperationalRiskQuestions
        {
            get
            {
                return AllQuestionViewModels.Where(o => o.question.IsOperationalRisk).ToList();
            }
        }
        public List<RADeMSQuestionViewModel> ResponseCapacityQuestions
        {
            get
            {
                return AllQuestionViewModels.Where(o => !o.question.IsOperationalRisk).ToList();
            }
        }
        public string RademsResultText
        {
            get { return rademsScore.ShortText; }
        }

        public string ShortCodeText
        {
            get
            {
                return RADeMSTools.GetScoreShortCode(rademsScore);
            }
        }

        public Color ScoreColor
        {
            get
            {
                try
                {
                    int[] scores = new int[] { rademsScore.OperationalRisk, rademsScore.ResponseCapacity };
                    System.Drawing.Color sdColor = RADeMSTools.GetGradientColor(scores);
                    return new Color(sdColor.R, sdColor.G, sdColor.B);
                } catch (Exception ex)
                {
                    ;
                }
                return Colors.Red;
            }
        }


        public Command ShareScoreCommand { get; }
        public Command ViewCardCommand { get; }

        private async Task OnShareScoreCommand()
        {
            try
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = GetFullTextForShare(),
                    Title = "Share Score"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void OnViewCardCommand()
        {
            await Shell.Current.GoToAsync($"{nameof(Views.RADeMS.RADeMSCardPage)}?OpRisk={rademsScore.OperationalRisk}&ResCap={rademsScore.ResponseCapacity}");
           
           
        }


        public string GetFullTextForShare()
        {
            StringBuilder full = new StringBuilder();

            full.Append("Operational Risk: "); full.Append(rademsScore.OperationalRisk); full.Append(", ");
            full.Append("Response Capacity: "); full.Append(rademsScore.ResponseCapacity);
            full.Append(Environment.NewLine);

            if (!string.IsNullOrEmpty(SetByName))
            {
                full.Append("By: "); full.Append(SetByName); full.Append(Environment.NewLine);
            }
            full.Append(Environment.NewLine);
            full.Append("RADeMS Score - "); full.Append(CategoryTitle); full.Append(Environment.NewLine);
            full.Append("Conducted: "); full.Append(rademsScore.LastUpdatedLocal.ToString("yyyy-MMM-dd HH:mm")); full.Append(Environment.NewLine);
            
            full.Append(Environment.NewLine);
            full.AppendLine("Detailed Score");
            full.AppendLine("Operational Risk");
            
            foreach(RADeMSQuestionViewModel q in OperationalRiskQuestions)
            {
                full.Append(q.question.QuestionText); full.Append(" - ");
                switch (q.SelectedValue)
                {
                    case 1:
                        full.Append(q.question.Option2LabelWithNum); break;
                    case 2:
                        full.Append(q.question.Option3LabelWithNum); break;
                    default:
                        full.Append(q.question.Option1LabelWithNum); break;
                }
                full.Append(Environment.NewLine);
            }
            full.Append(Environment.NewLine);

            full.AppendLine("Response Capacity");

            foreach (RADeMSQuestionViewModel q in ResponseCapacityQuestions)
            {
                full.Append(q.question.QuestionText); full.Append(" - ");
                switch (q.SelectedValue)
                {
                    case 1:
                        full.Append(q.question.Option2LabelWithNum); break;
                    case 2:
                        full.Append(q.question.Option3LabelWithNum); break;
                    default:
                        full.Append(q.question.Option1LabelWithNum); break;
                }
                full.Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(Comment))
            {
                full.Append(Environment.NewLine);
                full.AppendLine("Comments:");
                full.Append(Comment);
            }

            return full.ToString();
        }
    }
}
